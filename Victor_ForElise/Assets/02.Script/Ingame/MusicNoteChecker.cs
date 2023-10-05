using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MusicNoteChecker : MonoBehaviour
{
    public enum ENoteTiming
    {
        None,
        Perfect,
        Good,
        Bad,
        Miss
    }

    private Queue<NoteLine> m_ObjectPool = new Queue<NoteLine>();
    private List<NoteObject> CheckObjectList = new List<NoteObject>();

    private Dictionary<NoteObject, NoteLine> m_LineListMatchNote = new Dictionary<NoteObject, NoteLine>();

    [SerializeField]
    private TextMeshProUGUI debugTxt;
    [SerializeField]
    private TextMeshProUGUI comboTxt;

    [SerializeField]
    private NoteLine noteLine;

    [SerializeField]
    private Transform noteLineTrf;

    [SerializeField]
    private float PerfectTime = 0.05f;

    [SerializeField]
    private float BadTime = 0.2f;

    [SerializeField]
    private float GoodTime = 0.2f;

    [SerializeField]
    private float MakeTime = 0.5f;

    [SerializeField]
    private int MakeNoteInitCount = 5;

    [SerializeField]
    private float SwipeLength = 1f;

    private int combo;

    // Swipe Objects
    private Dictionary<int, ENoteTiming> SwipeTimingCacheDic = new Dictionary<int, ENoteTiming>();
    // Swipe Objects
    private Dictionary<NoteObject, int> SwipeObjectCacheDic = new Dictionary<NoteObject, int>();

    // LongNote Objects
    // 어떤 터치가 어떤 롱노트를 클릭중인지
    // touchId, longNoteKey
    private Dictionary<int, int> LongNoteObjectCacheDic = new Dictionary<int, int>();

    // Spawn LongNote Check
    private Dictionary<int, LinkedListNode<NoteData>> longNoteCurNode = new Dictionary<int, LinkedListNode<NoteData>>();

    // Touch LongNote Check
    private Dictionary<int, LinkedListNode<NoteData>> longNoteTouchNode = new Dictionary<int, LinkedListNode<NoteData>>();

    // LongNote Start
    // 어떤 터치가 진행중인지
    // longNoteIndex, longNoteStartBool
    private Dictionary<int, bool> LongNoteObjectStartDic = new Dictionary<int, bool>();

    // LongNoteLine TmpDic
    // 롱노트 라인 롱노트 인덱스 별 저장
    // longNoteKey, NoteLine
    private Dictionary<int, NoteLine> LongNoteLineDic = new Dictionary<int, NoteLine>();

    // Drag Input Dic
    private Dictionary<int, InputData> DragInputDataDic = new Dictionary<int, InputData>();

    private List<NoteObject> destroyList = new List<NoteObject>();


    private Queue<InputData> inputQueue = new Queue<InputData>();

    public void Initialize()
    {
        MakeNoteLineFirstTime(MakeNoteInitCount);

        GameManager.Instance.OnUpdate += OnUpdate;

        combo = 0;
    }

    public void MakeNoteLineFirstTime(int count)
    {
        for(int i=0;i<count; i++)
        {
            NoteLine noteLineTmp = Instantiate(noteLine, noteLineTrf);
            m_ObjectPool.Enqueue(noteLineTmp);
            noteLineTmp.gameObject.SetActive(false);
        }
    }

    public NoteLine InstantiateNoteLine(float duration, Vector3 pos, Quaternion rotate)
    {
        NoteLine noteLineTmp;
        if (m_ObjectPool.Count > 0)
        {
            noteLineTmp = m_ObjectPool.Dequeue();
            noteLineTmp.thisTrf.position = pos;
            noteLineTmp.thisTrf.rotation = rotate;
        }
        else
        {
            noteLineTmp = Instantiate(noteLine, pos, rotate, noteLineTrf);
        }

        noteLineTmp.Initialize(duration);
        noteLineTmp.gameObject.SetActive(true);

        return noteLineTmp;
    }

    private void DestroyLine(NoteLine noteLine)
    {
        noteLine.gameObject.SetActive(false);
        m_ObjectPool.Enqueue(noteLine);
    }

    private void OnUpdate(float deltaTime)
    {
        comboTxt.text = combo.ToString();

        List<NoteObject> noteObjects = GameManager.GetNoteMover.GetNoteObjectList;

        for (int i=0;i<noteObjects.Count;i++)
        {
            NoteObject noteObjectTmp = noteObjects[i];

            if (noteObjectTmp.key != NoteKeyCode.Line &&
                noteObjectTmp.key != NoteKeyCode.LongMesh)
            {
                if (noteObjectTmp.thisTrf.position.z <= GameManager.Instance.BarDistance * MakeTime &&
                    noteObjectTmp.thisTrf.position.z > GameManager.Instance.BarDistance * GoodTime)
                {
                    if (noteObjectTmp.key == NoteKeyCode.LongStart || noteObjectTmp.key == NoteKeyCode.LongEnd)
                    {
                        if (longNoteCurNode.ContainsKey(noteObjectTmp.data.LongNoteIndex).Equals(false))
                        {
                            longNoteCurNode.Add(noteObjectTmp.data.LongNoteIndex,
                                GameManager.Instance.noteContainer.GetLongNoteDataDic[noteObjectTmp.data.LongNoteIndex].First);

                            CheckObjectList.Add(noteObjectTmp);

                            Vector3 linePos = new Vector3(noteObjectTmp.thisTrf.position.x, noteObjectTmp.thisTrf.position.y, 0f);
                            NoteLine noteLineTmp = InstantiateNoteLine(GameManager.GetMusic.BarPerSec * MakeTime, linePos, noteObjectTmp.thisTrf.rotation);
                            m_LineListMatchNote.Add(noteObjectTmp, noteLineTmp);
                        }
                        else
                        {
                            if (longNoteCurNode[noteObjectTmp.data.LongNoteIndex].Next == null)
                            {
                                continue;
                            }

                            if (longNoteCurNode[noteObjectTmp.data.LongNoteIndex].Next.Value == noteObjectTmp.data)
                            {
                                longNoteCurNode[noteObjectTmp.data.LongNoteIndex] = longNoteCurNode[noteObjectTmp.data.LongNoteIndex].Next;
                                CheckObjectList.Add(noteObjectTmp);
                            }
                        }
                    }
                    else
                    {
                        if (CheckObjectList.Contains(noteObjectTmp).Equals(false))
                        {
                            Vector3 linePos = new Vector3(noteObjectTmp.thisTrf.position.x, noteObjectTmp.thisTrf.position.y, 0f);
                            NoteLine noteLineTmp = InstantiateNoteLine(GameManager.GetMusic.BarPerSec * MakeTime, linePos, noteObjectTmp.thisTrf.rotation);
                            m_LineListMatchNote.Add(noteObjectTmp, noteLineTmp);

                            CheckObjectList.Add(noteObjectTmp);
                        }
                    }

                }
            }
        }

        for (int i=0;i< CheckObjectList.Count;i++)
        {
            NoteObject noteObjectTmp = CheckObjectList[i];

            if (noteObjectTmp.key == NoteKeyCode.LongEnd)
            {
                continue;
            }

            if (noteObjectTmp.thisTrf.position.z < -GameManager.Instance.BarDistance * BadTime)
            {
                if(noteObjectTmp.key == NoteKeyCode.LongStart)
                {
                    if((noteObjectTmp.data != 
                        GameManager.Instance.noteContainer.GetLongNoteDataDic[noteObjectTmp.data.LongNoteIndex].First.Value))
                    {
                        continue;
                    }

                    if (LongNoteObjectStartDic.ContainsKey(noteObjectTmp.data.LongNoteIndex).Equals(false))
                    {
                        destroyList.Add(noteObjectTmp);
                        LongNoteObjectStartDic.Add(noteObjectTmp.data.LongNoteIndex, false);
                    }
                }
                else
                {
                    destroyList.Add(noteObjectTmp);
                }

                Debug.LogError(noteObjectTmp.key);
                NoteResultExecute(ENoteTiming.Miss);

                if(noteObjectTmp.key == NoteKeyCode.UpSlide ||
                noteObjectTmp.key == NoteKeyCode.RightSlide ||
                noteObjectTmp.key == NoteKeyCode.DownSlide ||
                noteObjectTmp.key == NoteKeyCode.LeftSlide)
                {
                    if(SwipeObjectCacheDic.ContainsKey(noteObjectTmp))
                    {
                        int key = SwipeObjectCacheDic[noteObjectTmp];
                        SwipeTimingCacheDic.Remove(key);
                        SwipeObjectCacheDic.Remove(noteObjectTmp);
                    }
                }

                if(m_LineListMatchNote.ContainsKey(noteObjectTmp))
                {
                    DestroyLine(m_LineListMatchNote[noteObjectTmp]);
                    m_LineListMatchNote.Remove(noteObjectTmp);
                }
            }
        }

        for(int i=0;i<destroyList.Count;i++)
        {
            CheckObjectList.Remove(destroyList[i]);

            GameManager.GetNoteMover.GetNoteObjectList.Remove(destroyList[i]);
            GameManager.GetMusicPool.DestroyObject(destroyList[i]);
        }
        destroyList.Clear();

        InputExecute();

        // LongNoteCheck
        Utility.ForeachNoGCValue(DragInputDataDic,
            value=>
            {
                CheckLongNote(value);
            });
        //foreach (var pair in DragInputDataDic)
        //{
        //    CheckLongNote(pair.Value);
        //}
    }

    public void NoteResultExecute(ENoteTiming noteTiming)
    {
        switch(noteTiming)
        {
            default:
            case ENoteTiming.None:
                break;
            case ENoteTiming.Perfect:
                debugTxt.text = "Perfect";
                combo++;
                break;
            case ENoteTiming.Good:
                debugTxt.text = "Good";
                combo++;
                break;
            case ENoteTiming.Bad:
                debugTxt.text = "Bad";
                combo++;
                break;
            case ENoteTiming.Miss:
                debugTxt.text = "Miss";
                combo = 0;
                break;
        }
    }

    public void OnInput(InputData inputData)
    {
        inputQueue.Enqueue(inputData);
    }
    private void InputExecute()
    {
        while (inputQueue.Count > 0)
        {
            InputData data = inputQueue.Dequeue();

            switch (data.inputPhase)
            {
                case EInputPhase.PointDown:
                    CheckTouchNote(data);
                    break;
                case EInputPhase.PointUp:
                    DisableTouchLongNote(data);

                    if (DragInputDataDic.ContainsKey(data.touchId))
                    {
                        DragInputDataDic.Remove(data.touchId);
                    }
                    break;
                case EInputPhase.Drag:
                    CheckSwipeNote(data);

                    if (DragInputDataDic.ContainsKey(data.touchId))
                    {
                        DragInputDataDic[data.touchId] = data;
                    }
                    else
                    {
                        DragInputDataDic.Add(data.touchId, data);
                    }
                    break;
            }
        }
    }

    private void DisableTouchLongNote(InputData inputData)
    {
        int longNoteKey;
        if(LongNoteObjectCacheDic.TryGetValue(inputData.touchId, out longNoteKey))
        {
            Debug.Log("DisableTouchLongNote");
            NoteResultExecute(ENoteTiming.Miss);

            NoteLine line;
            if(LongNoteLineDic.TryGetValue(longNoteKey, out line))
            {
                DestroyLine(line);
            }

            for (int i = 0; i < CheckObjectList.Count; i++)
            {
                NoteObject noteObject = CheckObjectList[i];

                if (noteObject.key != NoteKeyCode.LongStart && noteObject.key != NoteKeyCode.LongEnd)
                {
                    continue;
                }

                if (noteObject.data.LongNoteIndex == longNoteKey)
                {
                    destroyList.Add(noteObject);
                }
            }

            for (int i = 0; i < destroyList.Count; i++)
            {
                CheckObjectList.Remove(destroyList[i]);
            }
            destroyList.Clear();

            LongNoteObjectCacheDic.Remove(inputData.touchId);
        }
    }

    private void CheckTouchNote(InputData inputData)
    {
        for (int i = 0; i < CheckObjectList.Count; i++)
        {
            NoteObject noteObjectTmp = CheckObjectList[i];

            if(noteObjectTmp.key == NoteKeyCode.Line || 
                noteObjectTmp.key == NoteKeyCode.LongMesh ||
                noteObjectTmp.key == NoteKeyCode.LongEnd)
            {
                continue;
            }

            Vector3 objectPos;
            objectPos = noteObjectTmp.thisTrf.position;

            // 터치 판정 최소 기준
            if(objectPos.z > GameManager.Instance.BarDistance * GoodTime ||
                objectPos.z < GameManager.Instance.BarDistance * -BadTime)
            {
                continue;
            }

            float halfBoxSize = GameManager.GetBoxSize;

            if (inputData.curWorldPos.x <= objectPos.x + halfBoxSize &&
                inputData.curWorldPos.x > objectPos.x - halfBoxSize &&
                inputData.curWorldPos.y <= objectPos.y + halfBoxSize &&
                inputData.curWorldPos.y > objectPos.y - halfBoxSize)
            {
                ENoteTiming noteTimingTmp = CheckTiming(objectPos.z);

                if(noteTimingTmp == ENoteTiming.None)
                {
                    continue;
                }

                NoteLine noteLine;

                switch (noteObjectTmp.key)
                {
                    case NoteKeyCode.Default:
                        NoteResultExecute(noteTimingTmp);

                        destroyList.Add(noteObjectTmp);

                        if (m_LineListMatchNote.TryGetValue(noteObjectTmp, out noteLine).Equals(true))
                        {
                            DestroyLine(noteLine);
                            m_LineListMatchNote.Remove(noteObjectTmp);
                        }
                        break;
                    case NoteKeyCode.DownSlide:
                    case NoteKeyCode.LeftSlide:
                    case NoteKeyCode.RightSlide:
                    case NoteKeyCode.UpSlide:
                        if(SwipeObjectCacheDic.ContainsKey(noteObjectTmp).Equals(false))
                        {
                            SwipeObjectCacheDic.Add(noteObjectTmp, inputData.touchId);
                            SwipeTimingCacheDic.Add(inputData.touchId, noteTimingTmp);
                        }
                        break;
                    case NoteKeyCode.LongStart:
                        Debug.Log($"LongNoteObjectCacheDic:{LongNoteObjectCacheDic.ContainsKey(inputData.touchId).Equals(false)}");
                        Debug.Log($"LongNoteObjectStartDic:{LongNoteObjectStartDic.ContainsKey(noteObjectTmp.data.LongNoteIndex).Equals(false)}");
                        Debug.Log($"LongNoteIndex:{noteObjectTmp.data.LongNoteIndex}");
                        if (LongNoteObjectCacheDic.ContainsKey(inputData.touchId).Equals(false) &&
                            LongNoteObjectStartDic.ContainsKey(noteObjectTmp.data.LongNoteIndex).Equals(false))
                        {
                            NoteResultExecute(noteTimingTmp);

                            destroyList.Add(noteObjectTmp);

                            if (m_LineListMatchNote.TryGetValue(noteObjectTmp, out noteLine))
                            {
                                StartCoroutine(LongNoteLineMover(noteLine, noteObjectTmp.data.LongNoteIndex));
                                m_LineListMatchNote.Remove(noteObjectTmp);
                                LongNoteLineDic.Add(noteObjectTmp.data.LongNoteIndex, noteLine);
                            }

                            if(LongNoteObjectCacheDic.ContainsKey(inputData.touchId).Equals(false))
                            {
                                LongNoteObjectCacheDic.Add(inputData.touchId, noteObjectTmp.data.LongNoteIndex);
                            }
                            if(LongNoteObjectStartDic.ContainsKey(noteObjectTmp.data.LongNoteIndex).Equals(false))
                            {
                                LongNoteObjectStartDic.Add(noteObjectTmp.data.LongNoteIndex, false);
                            }
                        }
                        break;
                }

                break;
            }
        }

        for (int i = 0; i < destroyList.Count; i++)
        {
            CheckObjectList.Remove(destroyList[i]);
            GameManager.GetNoteMover.GetNoteObjectList.Remove(destroyList[i]);
            GameManager.GetMusicPool.DestroyObject(destroyList[i]);
        }
        destroyList.Clear();
    }

    public IEnumerator LongNoteLineMover(NoteLine line, int lineIndex)
    {
        while(true)
        {
            Vector3 objectPos = GameManager.Instance.GetLongNoteMeshPosAtZeroPoint(lineIndex);
            objectPos.z = 0f;

            line.thisTrf.position = objectPos;

            if (line.gameObject.activeInHierarchy == false)
            {
                break;
            }

            yield return null;
        }

        yield break;
    }

    private void CheckLongNote(InputData inputData)
    {
        int longNoteKey;
        if (LongNoteObjectCacheDic.TryGetValue(inputData.touchId, out longNoteKey).Equals(false))
        {
            return;
        }

        float halfBoxSize = GameManager.GetBoxSize;

        bool IsStart = false;
        if(LongNoteObjectStartDic.TryGetValue(longNoteKey, out IsStart).Equals(false) || IsStart == false)
        {
            if(GameManager.GetNoteMover.GetLongNoteMeshDic.ContainsKey(longNoteKey).Equals(false))
            {
                return;
            }

            NoteObject meshTmp = GameManager.GetNoteMover.GetLongNoteMeshDic[longNoteKey];

            if(meshTmp.thisTrf.position.z < -GameManager.Instance.BarDistance * BadTime)
            {
                if (longNoteTouchNode.ContainsKey(longNoteKey).Equals(false))
                {
                    LinkedListNode<NoteData> longData = GameManager.Instance.noteContainer.GetLongNoteDataDic[longNoteKey].First.Next;
                    longNoteTouchNode.Add(longNoteKey, longData);

                    LongNoteObjectStartDic[longNoteKey] = true;
                }
            }
            return;
        }

        Vector3 objectPos = GameManager.Instance.GetLongNoteMeshPosAtZeroPoint(longNoteKey);

        //Debug.LogError(objectPos);

        if (inputData.curWorldPos.x <= objectPos.x + halfBoxSize &&
                inputData.curWorldPos.x > objectPos.x - halfBoxSize &&
                inputData.curWorldPos.y <= objectPos.y + halfBoxSize &&
                inputData.curWorldPos.y > objectPos.y - halfBoxSize)
        {
            for (int i = 0; i < CheckObjectList.Count; i++)
            {
                NoteObject noteObject = CheckObjectList[i];

                if(noteObject.key != NoteKeyCode.LongStart && noteObject.key != NoteKeyCode.LongEnd)
                {
                    continue;
                }

                if (noteObject.thisTrf.position.z > 0f)
                {
                    continue;
                }

                // Data 비교
                if (longNoteTouchNode.ContainsKey(longNoteKey).Equals(true))
                {
                    LinkedListNode<NoteData> longData = longNoteTouchNode[longNoteKey];

                    if ((longData.Value != noteObject.data))
                    {
                        continue;
                    }

                    longNoteTouchNode[longNoteKey] = longData.Next;
                }

                NoteResultExecute(ENoteTiming.Perfect);

                destroyList.Add(noteObject);

                if (noteObject.key == NoteKeyCode.LongEnd)
                {
                    Debug.LogError("End LongNote");

                    int longNoteKeyTmp;
                    if (LongNoteObjectCacheDic.TryGetValue(inputData.touchId, out longNoteKeyTmp))
                    {
                        LongNoteObjectCacheDic.Remove(inputData.touchId);

                        NoteLine line;
                        if (LongNoteLineDic.TryGetValue(longNoteKeyTmp, out line))
                        {
                            DestroyLine(line);
                        }
                    }
                    break;
                }

            }

            for (int i=0;i<destroyList.Count;i++)
            {
                CheckObjectList.Remove(destroyList[i]);
                GameManager.GetNoteMover.GetNoteObjectList.Remove(destroyList[i]);
                GameManager.GetMusicPool.DestroyObject(destroyList[i]);
            }
            destroyList.Clear();
        }
        else
        {
            Debug.LogError("End LongNote");
            DisableTouchLongNote(inputData);
        }
    }

    private void CheckSwipeNote(InputData inputData)
    {
        for (int i = 0; i < CheckObjectList.Count; i++)
        {
            NoteObject noteObjectTmp = CheckObjectList[i];

            if ((noteObjectTmp.key == NoteKeyCode.UpSlide ||
                noteObjectTmp.key == NoteKeyCode.RightSlide ||
                noteObjectTmp.key == NoteKeyCode.DownSlide ||
                noteObjectTmp.key == NoteKeyCode.LeftSlide).Equals(false))
            {
                continue;
            }

            if(SwipeTimingCacheDic.ContainsKey(inputData.touchId).Equals(false) ||
                SwipeObjectCacheDic.ContainsKey(noteObjectTmp).Equals(false))
            {
                continue;
            }

            ENoteTiming noteTiming = SwipeTimingCacheDic[inputData.touchId];
            if(noteTiming == ENoteTiming.Miss || noteTiming == ENoteTiming.None)
            {
                continue;
            }

            bool IsSwipeDone = false;
            switch (noteObjectTmp.key)
            {
                case NoteKeyCode.UpSlide:
                    if (inputData.deltaPos.y > SwipeLength)
                    {
                        IsSwipeDone = true;
                    }
                    break;
                case NoteKeyCode.RightSlide:
                    if (inputData.deltaPos.x > SwipeLength)
                    {
                        IsSwipeDone = true;
                    }
                    break;
                case NoteKeyCode.DownSlide:
                    if (inputData.deltaPos.y < -SwipeLength)
                    {
                        IsSwipeDone = true;
                    }
                    break;
                case NoteKeyCode.LeftSlide:
                    if (inputData.deltaPos.x < -SwipeLength)
                    {
                        IsSwipeDone = true;
                    }
                    break;
            }

            if (IsSwipeDone.Equals(false))
            {
                return;
            }

            NoteResultExecute(noteTiming);

            GameManager.GetNoteMover.GetNoteObjectList.Remove(noteObjectTmp);
            GameManager.GetMusicPool.DestroyObject(noteObjectTmp);
            CheckObjectList.RemoveAt(i);
            DestroyLine(m_LineListMatchNote[noteObjectTmp]);
            m_LineListMatchNote.Remove(noteObjectTmp);

            // SwipeCache Remove
            SwipeTimingCacheDic.Remove(inputData.touchId);
            SwipeObjectCacheDic.Remove(noteObjectTmp);

            break;
        }
    }

    private ENoteTiming CheckTiming(float zPos)
    {
        ENoteTiming noteTiming;

        // Bad
        if (zPos < -GameManager.Instance.BarDistance * PerfectTime)
        {
            noteTiming = ENoteTiming.Bad;
        }
        // Perfect
        else if (zPos <= GameManager.Instance.BarDistance * PerfectTime)
        {
            noteTiming = ENoteTiming.Perfect;
        }
        // Good
        else if (zPos <= GameManager.Instance.BarDistance * GoodTime)
        {
            noteTiming = ENoteTiming.Good;
        }
        else
        {
            noteTiming = ENoteTiming.None;
        }

        return noteTiming;
    }
}
