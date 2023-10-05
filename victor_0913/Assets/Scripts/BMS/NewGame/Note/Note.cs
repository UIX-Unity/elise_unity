using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NewGame
{
    public class Note : PoolableObject
    {
        protected NoteVisual visual;
        protected GameSetting info;
        //Top left , top right , bottom left , bottom right
        protected Vector2[] cornerPositionData = { new Vector2(-25f, 25f), new Vector2(25f, 25f), new Vector2(-25f, -25f), new Vector2(25f, -25f) };
        //Up, down , left , right
        protected Vector2[] screenStickPositionData = { new Vector2(0, 50f), new Vector2(0f, -50f), new Vector2(-50f, 0f), new Vector2(50f, 0) };
        protected Vector3 pressedPosition, releasePosition;

        //Float
        protected float moveValue;
        private float showRange = 2000f;
        private float fadingPercent;
        private float predictedX = 0f, predictedY = 0f, predictedZ = 0f;
        //Bool
        protected bool pressed = false;
        protected bool scoreCounted = false;
        private bool show = false;
        private UIEventTrigger uIEventTrigger;
        private EventDelegate playerPress;
        private EventDelegate playerRelease;
        private EventDelegate playerDragOut;
        //Debug
        private NoteData noteData;
        private void Awake()
        {
            visual = this.GetComponent<NoteVisual>();
            uIEventTrigger = visual.UIEventTrigger;
            info = Resources.Load("GameSetting") as GameSetting;
            playerPress = new EventDelegate(this, nameof(PlayerPress));
            playerRelease = new EventDelegate(this, nameof(PlayerRelease));
            playerDragOut = new EventDelegate(this, nameof(DragOutHandler));
            moveValue = info.BPM / (4 * 60f) * 1000f * info.speed * Time.fixedDeltaTime;
        }
        private void OnEnable()
        {
            show = false;
            visual.SetUp();

            //Attach events
            uIEventTrigger?.onRelease.Add(playerRelease);
            uIEventTrigger?.onPress.Add(playerPress);
            uIEventTrigger?.onDragOut.Add(playerDragOut);
        }

        private void Update()
        {
            //Handle when note reach show range
            if (show)
            {
                Vector3 showPosisiton = new Vector3(visual.NotePosition.x, visual.NotePosition.y, showRange);
                fadingPercent = Mathf.Clamp01(Vector3.Distance(visual.NotePosition, showPosisiton) / Vector3.Distance(showPosisiton, Vector3.zero));
                visual.noteAnimated.alpha = fadingPercent;

                float penaltyValue = 0.5f;
                visual.screenBtnAnimated.alpha = fadingPercent - penaltyValue;

                float startLerpValue = 0.5f;
                if (fadingPercent >= startLerpValue)
                {
                    for (int i = 0; i < visual.screenStickArray.Length; i++)
                    {
                        visual.screenStickArray[i].localPosition = Vector3.Lerp(screenStickPositionData[i], Vector3.zero, fadingPercent);
                    }
                }
            }
            if (visual.NotePosition.z <= showRange && visual.NotePosition.z > 0f && !show)
            {
                show = true;
                InShowRangeHandler();
            }
            if (visual.NotePosition.z < 0f && show)
            {
                PostShowRangeHandler();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (NewGameScene.GetInstance.GameState != GameState.Playing) return;
            Vector3 notePosition = visual.NotePosition;
            predictedX = Mathf.MoveTowards(notePosition.x, notePosition.x, moveValue);
            predictedY = Mathf.MoveTowards(notePosition.y, notePosition.y, moveValue);
            predictedZ = notePosition.z - moveValue;

            Vector3 predictedPosition = new Vector3(predictedX, predictedY, predictedZ);
            visual.NotePosition = predictedPosition;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            //Detach events
            uIEventTrigger?.onRelease.Remove(playerRelease);
            uIEventTrigger?.onPress.Remove(playerPress);
            uIEventTrigger?.onDragOut.Add(playerDragOut);
        }

        //Initialize data to note object
        public virtual void Init(NoteData noteData)
        {
            this.noteData = noteData;
            for (int i = 0; i < visual.glowingEffect.Length; i++)
            {
                visual.glowingEffect[i].color = noteData.noteColor;
            }
            visual.NotePosition = noteData.notePosition;
            visual.ScreenButtonPosition = new Vector3(noteData.notePosition.x, noteData.notePosition.y, 0f);
        }

        protected virtual void InShowRangeHandler()
        {
            //if(info.lv == 0) easy mode
            visual.screenStickHolder.gameObject.SetActive(true);
            visual.noteObject.gameObject.SetActive(true);
            visual.screenButton.gameObject.SetActive(true);
        }

        private void PostShowRangeHandler()
        {
            if (visual.playingAnimation) return;
            ReturnToPool();
        }

        protected virtual void ReturnToPool()
        {
            if (!scoreCounted)
            {
                FailedHit();
            }
            NewGameScene.GetInstance.FullCount++;

            show = false;
            transform.SetParent(Parent.GetHolder().transform, false);
            this.transform.localPosition = Vector3.zero;
            this.gameObject.SetActive(false);
        }

        //TODO:make player press and release handler
        private void PlayerPress()
        {
            float pressAvaiablePercent = 0.8f;
            if (fadingPercent >= pressAvaiablePercent)
            {
                pressed = true;
                visual.screenStickHolder.gameObject.SetActive(false);
                Debug.Log("pressed!");
                PressHandler();
            }
            else
            {
                Debug.Log("failed!");
                visual.PlayAnimation(false);
            }
        }

        protected virtual void PressHandler() { }

        private void PlayerRelease()
        {
            if (pressed)
            {
                pressed = false;
                Debug.Log("release!");
                ReleaseHandler();
            }
        }

        protected virtual void ReleaseHandler() { }

        protected SwipeDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            SwipeDirection draggedDir;
            if (positiveX > positiveY)
            {
                draggedDir = (dragVector.x > 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {
                draggedDir = (dragVector.y > 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }
            Debug.Log(draggedDir);
            return draggedDir;
        }
        protected virtual void DragOutHandler()
        {
            Debug.Log("Drag Out!");
        }
        protected void SuccessHit()
        {
            NewGameScene scene = NewGameScene.GetInstance;
            scoreCounted = true;
            scene.CurrentCombo++;
            scene.CurrentHitCount++;
            scene.UpdateUI();
        }
        protected void FailedHit()
        {
            NewGameScene scene = NewGameScene.GetInstance;
            scene.CurrentCombo = 0;
            scene.UpdateUI();
        }
    }
}

