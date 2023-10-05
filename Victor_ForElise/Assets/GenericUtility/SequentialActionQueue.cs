using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace SqeuentialActionQueue
{
    
    public interface ISequentialAction
    {
        void ActionForStart();
        bool ExitCondition();
        void ActionForEnd();
    }

    public delegate bool ConditionAction();


    /// <summary>
    /// 1. EnqueueAction()을 통해 Action들을 등록한다.
    /// 2. SequentialInvoke()를 통해 등록한 Action들을 순차적으로 실행한다.
    /// 3. SkipCurrentInvoke() 또는 SkipCurrentInvoke()를 통해 실행중인 Action들을 건너뛸 수 있다.
    /// </summary>
    public class SequentialActionQueue
    {
        private MonoBehaviour mCoroutineCaller;
        public  MonoBehaviour coroutineCaller
        {
            get
            {
                return mCoroutineCaller;
            }
            private set
            {
                mCoroutineCaller = value;
            }
        }

        public  bool isInvoked { get; private set; }

        private SequentialAction        mCurSequentialAction;
        private UnityAction             mEndCallback;
        private Queue<SequentialAction> mQueue = new Queue<SequentialAction>();
        private IEnumerator             mCurExitConditionEnumerator;

        /// <summary>
        /// 현재 Queue에 있는 Action과 실행되고 있는 Action의 수
        /// </summary>
        public int actionCount { get { return mQueue.Count + ((mCurSequentialAction == null) ? 0 : 1); } }

        public void Initialize()
        {
            if (coroutineCaller != null && mCurExitConditionEnumerator != null)
            {
                coroutineCaller.StopCoroutine(mCurExitConditionEnumerator);
            }

            isInvoked = false;
            mCurSequentialAction = null;
            coroutineCaller = null;
            mQueue.Clear();
        }

        public void Release()
        {
            Initialize();
        }

        public void EnqueueActionByTime(SequentialActionByTime _seqActionByTime)
        {
            mQueue.Enqueue(_seqActionByTime);
        }

        public void EnqueueAction(SequentialAction _seqAction)
        {
            mQueue.Enqueue(_seqAction);
        }

        /// <summary>
        /// 현재 실행중인 Action을 멈추고 다음 Action을 실행한다.
        /// </summary>
        public void SkipCurrentInvoke()
        {
            if (!isInvoked) return;

            if (coroutineCaller != null && mCurExitConditionEnumerator != null)
            {
                coroutineCaller.StopCoroutine(mCurExitConditionEnumerator);
            }

            ExecuteOneAction();
        }

        /// <summary>
        /// 실행중인 모든 Action을 멈춘다.
        /// </summary>
        public void SkipAllInvokes()
        {
            if (!isInvoked) return;

            if (coroutineCaller != null && mCurExitConditionEnumerator != null)
            {
                coroutineCaller.StopCoroutine(mCurExitConditionEnumerator);
            }

            Initialize();
        }

        /// <summary>
        /// 등록한 Action들을 순차적으로 호출한다.
        /// </summary>
        /// <param name="_coroutineCaller">
        /// Coroutine을 호출하는 MonoBehaviour 객체
        /// </param>
        /// <param name="_endCallback">
        /// 모든 Aciton이 호출된 후 실행될 콜백
        /// (마무리 작업을 할 때 사용)
        /// </param>
        public void SequentialInvoke(MonoBehaviour _coroutineCaller, UnityAction _endCallback = null)
        {
            if (_coroutineCaller == null)
            {
                return;
            }
            else if (isInvoked)
            {
                return;
            }
            
            coroutineCaller = _coroutineCaller;
            mEndCallback = _endCallback;
            isInvoked       = true;
            ExecuteOneAction();
        }

        private void ExecuteOneAction()
        {
            if (coroutineCaller == null || !coroutineCaller.gameObject.activeInHierarchy)
            {
                Debug.Log($"{GetType()} : {MethodBase.GetCurrentMethod().Name}, coroutineCaller is null reference.");
                return;
            }
            else if (mQueue.Count == 0)
            {
                mCurSequentialAction = null;

                // 더 이상 큐에 들어있는 Action이 없을 때
                // Invoke()를 호출할 때 등록한 Callback을 호출한다.
                if (mEndCallback != null)
                {
                    mEndCallback();
                }
                
                Release();
                return;
            }
            
            mCurSequentialAction = mQueue.Dequeue();

            if (mCurSequentialAction.ActionForStart != null)
            {
                mCurSequentialAction.ActionForStart();
            }

            mCurExitConditionEnumerator = CheckExitCondition();
            coroutineCaller.StartCoroutine(mCurExitConditionEnumerator);
        }
        
        IEnumerator CheckExitCondition()
        {
            if (mCurSequentialAction.ExitCondition != null)
            {
                do
                {
                    yield return null;
                }
                while (!mCurSequentialAction.ExitCondition());
            }
            else
            {
                yield return null;
            }

            ExecuteOneAction();
        }
    }

    public class SequentialActionByTime : SequentialAction
    {
        public SequentialActionByTime(UnityAction _actionForStart, float _waitForExitTime = 0f) : base(_actionForStart, null)
        {
            waitForExitTime = _waitForExitTime;
            curWaitingTime = 0;
            
            base.exitCondition = () =>
            {
                if (curWaitingTime <= 0)
                {
                    waitForExitTime += Time.deltaTime;
                    curWaitingTime = waitForExitTime;
                }

                curWaitingTime -= Time.deltaTime;
                return (curWaitingTime <= 0);
            };
        }

        /// <summary>
        /// 다음 액션으로 넘어가기위해 기다려야할 시간
        /// </summary>
        private float waitForExitTime;

        /// <summary>
        /// 다음 액션으로 넘어가기위해 기다린 시간
        /// </summary>
        private float curWaitingTime;

        public void SetWaitForExitTime(float _waitForExitTime)
        {
            waitForExitTime = _waitForExitTime;
        }
    }

    public class SequentialAction
    {
        public SequentialAction(UnityAction _actionForStart, ConditionAction _exitCondition = null)
        {
            actionForStart = _actionForStart;
            exitCondition = _exitCondition;
        }

        protected UnityAction actionForStart;
        public UnityAction ActionForStart
        {
            get
            {
                return actionForStart;
            }
            set
            {
                actionForStart = value;
            }
        }

        protected ConditionAction exitCondition;
        public ConditionAction ExitCondition
        {
            get
            {
                return exitCondition;
            }
            set
            {
                exitCondition = value;
            }
        }
    }
}

