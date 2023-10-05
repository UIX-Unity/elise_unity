using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : BaseMonoSingleton<GameManager>
{
    [SerializeField]
    private float spawnDistance;
    public static float GetSpawnDistance => Instance.spawnDistance;

    [SerializeField]
    private float barDistanceOffset;
    public float BarDistance { get { return barDistanceOffset * speed; } }

    public float speed;

    [SerializeField]
    private float musicOffset { get { return GetMusic.Offset * GetMusic.BarPerTimeSample; } }

    [SerializeField]
    private MusicObjectPool m_MusicPool;
    public static MusicObjectPool GetMusicPool => Instance.m_MusicPool;

    public NoteContainer noteContainer { get; private set; }

    private NoteMover noteMover;
    public static NoteMover GetNoteMover => Instance.noteMover;

    [SerializeField]
    private Music m_Music;
    public static Music GetMusic => Instance.m_Music;

    [Header("비트별 메테리얼"), Space(10)]
    [SerializeField]
    private Material m_BeatDefaultMat;
    public Material BeatDefaultMat => m_BeatDefaultMat;

    [SerializeField]
    private Material m_Beat4Mat;
    public Material Beat4Mat => m_Beat4Mat;

    [SerializeField]
    private Material m_Beat8Mat;
    public Material Beat8Mat => m_Beat8Mat;

    [SerializeField]
    private Material m_Beat16Mat;
    public Material Beat16Mat => m_Beat16Mat;

    [SerializeField]
    private Material m_Beat32Mat;
    public Material Beat32Mat => m_Beat32Mat;

    [SerializeField]
    private Material m_emptyMat;
    public Material emptyMat => m_emptyMat;


    [SerializeField]
    private float BoxSize = 4f;
    public static float GetBoxSize => Instance.BoxSize;

    private Sheet m_Sheet;
    public static Sheet GetSheet => Instance.m_Sheet;
    public static void SetSheet(Sheet value) { Instance.m_Sheet = value; }

    public int BarCount { get; private set; }
    public static void AddBarCount(int value) { Instance.BarCount += value; } 

    public int BeatCount { get; private set; }

    public int BeforeBeatCount;

    public void AddBeatCount(float value, int beat)
    {
        int amountBeat = 32 / beat;

        ChangeBeatSelect(amountBeat);

        BeatCount += (int)(value * amountBeat);
    }

    public float BarPos { get; private set; }
    public int minBarCount { get; private set; }

    public bool IsMusicLoaded { get; private set; }

    public UnityAction<float> OnUpdate;
    public UnityAction OnBeatTick;
    public UnityAction OnTick;
    public UnityAction<int, float> OnResetPos;

    public void Initialize()
    {
        // This Init
        OnTick = null;

        IsMusicLoaded = false;

        BarCount = 0;
        BeatCount = 0;
        BarPos = 0f;
        BeforeBeatCount = 0;

        // Other Init
        minBarCount = (int)(spawnDistance / BarDistance);

        noteContainer = new NoteContainer();
        noteMover = new NoteMover();
        m_MusicPool.Initialize(minBarCount);
    }

    private void Update()
    {
        if (IsMusicLoaded.Equals(false))
        {
            return;
        }

        OnUpdate?.Invoke(Time.smoothDeltaTime);

        if (m_Music.IsAudioPlay.Equals(false))
        {
            return;
        }

        noteMover.UpdateNote(Time.smoothDeltaTime);

        CalculateBeat();

        if(BeforeBeatCount != BeatCount)
        {
            BeforeBeatCount = BeatCount;

            OnBeatTick?.Invoke();
        }

        if (m_Music.musicTimeSample >= m_Music.BarPerTimeSample * (BarCount+1))
        {
            NoteTik();
        }
    }

    // Vector3 = (xPos, yPos, LongNoteIndex)
    public Vector3 GetLongNoteMeshPosAtZeroPoint(int longNoteIndex)
    {
        int curWholeBeatCount = (BarCount * 32) + BeatCount;
        int dataWholeBeatCount = 0;
        int resultBeatCount = 0;

        Vector3 result = new Vector3(-100f, -100f, longNoteIndex);

        LinkedList<NoteData> linkedList;
        if (noteContainer.GetLongNoteDataDic.TryGetValue(longNoteIndex, out linkedList).Equals(false))
        {
            Debug.LogError("There is No Dictionary Data");
            return result;
        }
        LinkedListNode<NoteData> linkedListNode = linkedList.First;
        LinkedListNode<NoteData> nodeResult = null;

        //Debug.LogError($"First:{linkedListNode.Value.BarIndex} / {linkedListNode.Value.BeatIndex}");
        //Debug.LogError($"Cur:{BarCount} / {BeatCount}");

        while (linkedListNode != null)
        {
            NoteData dataTmp = linkedListNode.Value;

            dataWholeBeatCount = (dataTmp.BarIndex * 32) + dataTmp.BeatIndex;

            if (curWholeBeatCount >= dataWholeBeatCount)
            {
                resultBeatCount = dataWholeBeatCount;
                nodeResult = linkedListNode;
            }

            linkedListNode = linkedListNode.Next;
        }

        if (nodeResult == null)
        {
            result = new Vector3(-100f, -100f, longNoteIndex);
            return result;
        }

        if (nodeResult.Next == null && resultBeatCount != curWholeBeatCount)
        {
            Debug.LogError($"nodeResult.Next:{nodeResult.Next}");
            Debug.LogError($"resultBeatCount:{resultBeatCount}");
            Debug.LogError($"curWholeBeatCount:{curWholeBeatCount}");
            result = new Vector3(-100f, -100f, longNoteIndex);
            return result;
        }

        LinkedListNode<NoteData> nodeResultAfter;
        if (nodeResult.Next == null)
        {
            nodeResultAfter = nodeResult;
        }
        else
        {
            nodeResultAfter = nodeResult.Next;
        }

        int afterDataWholeBeatCount = (nodeResultAfter.Value.BarIndex * 32) + nodeResultAfter.Value.BeatIndex;

        // 노드간의 비트 차이
        int differenceTwoWholeBeat = afterDataWholeBeatCount - resultBeatCount;

        // 현재 비트의 위치
        int differecneCurWholeBeat = curWholeBeatCount - resultBeatCount;

        float ratioCurBeat;
        if (differenceTwoWholeBeat == 0 || differecneCurWholeBeat == 0)
        {
            ratioCurBeat = 0f;
        }
        else
        {
            ratioCurBeat = (float)differecneCurWholeBeat / differenceTwoWholeBeat;
        }

        result.x = Mathf.Lerp(nodeResult.Value.PosX, nodeResultAfter.Value.PosX, ratioCurBeat);
        result.y = Mathf.Lerp(nodeResult.Value.PosY, nodeResultAfter.Value.PosY, ratioCurBeat);

        return result;
    }

    // Vector3 = (xPos, yPos, LongNoteIndex)
    public Vector3[] GetLongNoteMeshPosAtZeroPoint()
    {
        List<Vector3> longNotePosTmp = new List<Vector3>();

        int curWholeBeatCount = (BarCount * 32) + BeatCount;
        int dataWholeBeatCount = 0;
        int resultBeatCount = 0;

        Utility.ForeachNoGCPair(noteContainer.GetLongNoteNodeDic, 
            pair=> 
        {
            LinkedListNode<NoteData> nodeResult = null;

            Vector3 longNotePosData = new Vector3(0f, 0f, 0f);
            longNotePosData.z = pair.Key;

            Utility.ForeachNoGCPair(pair.Value,
                valuePair =>
                {
                    Utility.ForeachNoGCPair(valuePair.Value,
                        values =>
                        {
                            dataWholeBeatCount = (valuePair.Key * 32) + values.Key;

                            if (dataWholeBeatCount <= curWholeBeatCount)
                            {
                                resultBeatCount = dataWholeBeatCount;
                                nodeResult = values.Value;
                            }
                        });
                });

            if (nodeResult == null)
            {
                return;
            }

            if (nodeResult.Next == null && resultBeatCount != curWholeBeatCount)
            {
                return;
            }

            LinkedListNode<NoteData> nodeResultAfter;
            if (nodeResult.Next == null)
            {
                nodeResultAfter = nodeResult;
            }
            else
            {
                nodeResultAfter = nodeResult.Next;
            }

            int afterDataWholeBeatCount = (nodeResultAfter.Value.BarIndex * 32) + nodeResultAfter.Value.BeatIndex;

            // 노드간의 비트 차이
            int differenceTwoWholeBeat = afterDataWholeBeatCount - resultBeatCount;

            // 현재 비트의 위치
            int differecneCurWholeBeat = curWholeBeatCount - resultBeatCount;

            float ratioCurBeat;
            if (differenceTwoWholeBeat == 0 || differecneCurWholeBeat == 0)
            {
                ratioCurBeat = 0f;
            }
            else
            {
                ratioCurBeat = (float)differecneCurWholeBeat / differenceTwoWholeBeat;
            }

            longNotePosData.x = Mathf.Lerp(nodeResult.Value.PosX, nodeResultAfter.Value.PosX, ratioCurBeat);
            longNotePosData.y = Mathf.Lerp(nodeResult.Value.PosY, nodeResultAfter.Value.PosY, ratioCurBeat);

            longNotePosTmp.Add(longNotePosData);
        });

        //foreach (var pair in noteContainer.GetLongNoteNodeDic)
        //{
        //    LinkedListNode<NoteData> nodeResult = null;

        //    Vector3 longNotePosData = new Vector3(0f,0f,0f);
        //    longNotePosData.z = pair.Key;

        //    foreach(var valuePair in pair.Value)
        //    {
        //        foreach(var values in valuePair.Value)
        //        {
        //            dataWholeBeatCount = (valuePair.Key * 32) + values.Key;

        //            if (dataWholeBeatCount <= curWholeBeatCount)
        //            {
        //                resultBeatCount = dataWholeBeatCount;
        //                nodeResult = values.Value;
        //            }
        //        }
        //    }

        //    if (nodeResult == null)
        //    {
        //        continue;
        //    }

        //    if (nodeResult.Next == null && resultBeatCount != curWholeBeatCount)
        //    {
        //        continue;
        //    }

        //    LinkedListNode<NoteData> nodeResultAfter;
        //    if (nodeResult.Next == null)
        //    {
        //        nodeResultAfter = nodeResult;
        //    }
        //    else
        //    {
        //        nodeResultAfter = nodeResult.Next;
        //    }

        //    int afterDataWholeBeatCount = (nodeResultAfter.Value.BarIndex * 32) + nodeResultAfter.Value.BeatIndex;

        //    // 노드간의 비트 차이
        //    int differenceTwoWholeBeat = afterDataWholeBeatCount - resultBeatCount;

        //    // 현재 비트의 위치
        //    int differecneCurWholeBeat = curWholeBeatCount - resultBeatCount;

        //    float ratioCurBeat;
        //    if (differenceTwoWholeBeat == 0 || differecneCurWholeBeat == 0)
        //    {
        //        ratioCurBeat = 0f;
        //    }
        //    else
        //    {
        //        ratioCurBeat = (float)differecneCurWholeBeat / differenceTwoWholeBeat;
        //    }

        //    longNotePosData.x = Mathf.Lerp(nodeResult.Value.PosX, nodeResultAfter.Value.PosX, ratioCurBeat);
        //    longNotePosData.y = Mathf.Lerp(nodeResult.Value.PosY, nodeResultAfter.Value.PosY, ratioCurBeat);

        //    longNotePosTmp.Add(longNotePosData);
        //}

        return longNotePosTmp.ToArray();
    }

    private void CalculateBeat()
    {
        BarPos = ((float)m_Music.musicTimeSample / m_Music.BarPerTimeSample) - BarCount;

        BeatCount = (int)(BarPos / 0.03125f);
    }

    private void NoteTik()
    {
        // Bar
        NoteObject noteObject = m_MusicPool.InstantiateObject(NoteKeyCode.Line,
            new Vector3(0f, 0f, spawnDistance), Quaternion.identity);
        noteObject.Initialize(BeatDefaultMat);
        noteMover.AddNoteList(noteObject);

        // Note
        int NextBarCount = BarCount + minBarCount + 1;

        List<NoteData> noteList;
        if(noteContainer.TryGetNoteListByBarIndex(NextBarCount, out noteList))
        {
            for (int i = 0; i < noteList.Count; i++)
            {
                NoteData data = noteList[i];

                float posZ = spawnDistance + ((BarDistance * 0.03125f) * data.BeatIndex);

                MakeNote(data, posZ, 0f);
            }
        }

        if (BeatCount >= 32)
        {
            BarCount++;
            BeatCount = 0;
        }

        OnTick?.Invoke();
    }

    public Material PickMaterial(int curBeat)
    {
        Material mat;

        if (curBeat == 0)
        {
            mat = BeatDefaultMat;
        }
        else if (curBeat % 2 == 1)
        {
            mat = Beat32Mat;
        }
        else if (curBeat % 4 == 2)
        {
            mat = Beat16Mat;
        }
        else if (curBeat % 8 == 4)
        {
            mat = Beat8Mat;
        }
        else
        {
            mat = Beat4Mat;
        }

        return mat;
    }

    private void MakeNote(NoteData noteData, float zPos, float barOffset)
    {
        switch (noteData.NoteType)
        {
            case NoteKeyCode.LongStart:
                if (noteMover.GetLongNoteMeshDic.ContainsKey(noteData.LongNoteIndex))
                {
                    // 롱노트의 시작 부분
                    NoteObject Note = m_MusicPool.InstantiateObject(NoteKeyCode.LongStart,
                       new Vector3(noteData.PosX, noteData.PosY, zPos), Quaternion.identity);

                    Note.Initialize(emptyMat);
                    Note.SetData(noteData);
                    noteMover.AddNoteList(Note);

                    NoteObject noteMesh = noteMover.GetLongNoteMeshDic[noteData.LongNoteIndex];

                    Utility.AddVertexToMesh(noteMesh, noteData);
                }
                else
                {
                    LinkedListNode<NoteData> longNoteNode;
                    if(noteContainer.GetLongNoteNodeDic.TryGetValue(noteData.LongNoteIndex, noteData.BarIndex, noteData.BeatIndex,
                        out longNoteNode).Equals(false))
                    {
                        Debug.LogError("Need To LongNote Init");
                        return;
                    }

                    Vector3 startPos;
                    LinkedListNode<NoteData> firstNode;

                    if (longNoteNode.Previous == null)
                    {
                        firstNode = null;
                        startPos = new Vector3(noteData.PosX, noteData.PosY, zPos);
                    }
                    else
                    {
                        firstNode = noteContainer.GetLongNoteDataDic[longNoteNode.Value.LongNoteIndex].First;
                        NoteData firstData = firstNode.Value;

                        // 바 오프셋값 + (첫번째 롱노트의 바 위치 값)
                        float startLongZPos = (barOffset + (BarDistance * (firstData.BarIndex - BarCount - 1))) + 
                            ((BarDistance * 0.03125f) * firstData.BeatIndex);
                        startPos = new Vector3(firstData.PosX, firstData.PosY, startLongZPos);
                    }


                    // 롱노트의 시작 부분
                    NoteObject startNote = m_MusicPool.InstantiateObject(NoteKeyCode.LongStart,
                        startPos, Quaternion.identity);

                    startNote.Initialize(BeatDefaultMat);
                    startNote.SetData(noteData);
                    noteMover.AddNoteList(startNote);

                    // 메쉬는 첫번째 버텍스가 항상 가운데
                    NoteObject noteMesh = m_MusicPool.InstantiateObject(NoteKeyCode.LongMesh,
                        startPos, Quaternion.identity);

                    // Vertex의 초기화
                    MeshFilter meshFilter = noteMesh.GetComponent<MeshFilter>();
                    meshFilter.mesh.triangles = null;
                    meshFilter.mesh.vertices = Utility.MakeSquareVertices(Vector3.zero, BoxSize);

                    if (longNoteNode.Previous != null)
                    {
                        while(firstNode != longNoteNode)
                        {
                            firstNode = firstNode.Next;

                            Utility.AddVertexToMesh(noteMesh, firstNode.Value);
                        }
                    }

                    noteMesh.SetData(noteData);
                    noteMover.AddNoteList(noteMesh);
                    noteMover.AddLongNoteMesh(noteData.LongNoteIndex, noteMesh);
                }

                break;
            case NoteKeyCode.LongEnd:
                Vector3 endPos = new Vector3(noteData.PosX, noteData.PosY, zPos);

                // 롱노트의 끝 부분
                NoteObject endNote = m_MusicPool.InstantiateObject(NoteKeyCode.LongEnd,
                    endPos, Quaternion.identity);

                endNote.Initialize(emptyMat);
                endNote.SetData(noteData);
                noteMover.AddNoteList(endNote);

                if (noteMover.GetLongNoteMeshDic.ContainsKey(noteData.LongNoteIndex))
                {
                    NoteObject endMesh = noteMover.GetLongNoteMeshDic[noteData.LongNoteIndex];

                    Utility.AddVertexToMesh(endMesh, noteData);
                }
                else
                {
                    LinkedListNode<NoteData> longNoteNode;
                    if (noteContainer.GetLongNoteNodeDic.TryGetValue(noteData.LongNoteIndex, noteData.BarIndex, noteData.BeatIndex,
                        out longNoteNode).Equals(false))
                    {
                        Debug.LogError("Need To LongNote Init");
                        return;
                    }

                    Vector3 startPos;
                    LinkedListNode<NoteData> firstNode;

                    if (longNoteNode.Previous == null)
                    {
                        Debug.LogError("You need to place 'start note'");
                    }

                    firstNode = noteContainer.GetLongNoteDataDic[longNoteNode.Value.LongNoteIndex].First;
                    NoteData firstData = firstNode.Value;

                    // 바 오프셋값 + (첫번째 롱노트의 바 위치 값)
                    float startLongZPos = (barOffset + (BarDistance * (firstData.BarIndex - BarCount - 1))) +
                        ((BarDistance * 0.03125f) * firstData.BeatIndex);
                    startPos = new Vector3(firstData.PosX, firstData.PosY, startLongZPos);

                    // 메쉬는 첫번째 버텍스가 항상 가운데
                    NoteObject noteMesh = m_MusicPool.InstantiateObject(NoteKeyCode.LongMesh,
                        startPos, Quaternion.identity);

                    // Vertex의 초기화
                    MeshFilter meshFilter = noteMesh.GetComponent<MeshFilter>();
                    meshFilter.mesh.triangles = null;
                    meshFilter.mesh.vertices = Utility.MakeSquareVertices(Vector3.zero, BoxSize);

                    if (longNoteNode.Previous != null)
                    {
                        while (firstNode != longNoteNode)
                        {
                            firstNode = firstNode.Next;

                            Utility.AddVertexToMesh(noteMesh, firstNode.Value);
                        }
                    }

                    noteMesh.SetData(noteData);
                    noteMover.AddNoteList(noteMesh);
                    noteMover.AddLongNoteMesh(noteData.LongNoteIndex, noteMesh);
                }
                break;
            default:
                NoteObject noteObject = m_MusicPool.InstantiateObject(noteData.NoteType,
        new Vector3(noteData.PosX, noteData.PosY, zPos), Quaternion.identity);

                noteObject.Initialize(PickMaterial(noteData.BeatIndex));
                noteObject.SetData(noteData);
                noteMover.AddNoteList(noteObject);
                break;
        }
    }

    public void LoadData(string stageName)
    {
        if (IsMusicLoaded)
        {
            Debug.Log("Already Load");
            return;
        }

        if(DataManager.sheetDataStorage.TryGetSheetData(stageName.ToLower(), out m_Sheet).Equals(false))
        {
            Debug.LogError("Cannot Load SheetData");
        }

        UnityEngine.Object[] objectTmp;
        DataManagement.LoadAssetBundleFromStreamingAsset(stageName.ToLower(), out objectTmp);

        // Convert Asset
        AudioClip clipTmp;
        Sprite sprite;
        if (DataManagement.ConvertAssetBundle(objectTmp, out clipTmp, out sprite).Equals(false))
        {
            Debug.LogError("Cannot Convert");
            return;
        }
        
        // Set Clip
        m_Music.SetMusic(clipTmp);
        InitMusic();

        SyncSheetAndNoteContainer();

        InitMusicSample();
        ResetBarPos();
    }
    
    public void SyncSheetAndNoteContainer()
    {
        noteContainer.SheetDataToNoteContainer(m_Sheet);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        if (IsMusicLoaded)
        {
            GetMusic.Play();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        if (IsMusicLoaded)
        {
            GetMusic.Pause();

            ReCalculateTimeSample();
            InitMusicSample();
            ResetBarPos();
        }
    }

    public void ChangeBeatSelect(int amountBeat)
    {
        int curBeat = Instance.BeatCount;

        Instance.BeatCount -= curBeat % amountBeat;
    }

    public void ReCalculateTimeSample()
    {
        int calculateTimeSample;

        // 마디당 샘플 + 비트당 샘플(32비트 기준)
        calculateTimeSample = (BarCount * m_Music.BarPerTimeSample) + (BeatCount * m_Music.BeatPerTimeSample32rd);

        calculateTimeSample = (int)Mathf.Clamp(calculateTimeSample, 0f, GetMusic.WholeTimeSample);

        m_Music.audioSource.timeSamples = calculateTimeSample;
    }

    public void ResetBarPos()
    {
        noteMover.RemoveAllNoteList();

        // 한마디 에서의 위치 거리
        float offsetPos = BarDistance * (1f - BarPos);
        int barCount = minBarCount;

        for(int i=-2;i< barCount; i++)
        {
            float zPos = offsetPos + (BarDistance * i);

            if(zPos >= 0f)
            {
                Vector3 pos = new Vector3(0f, 0f, zPos);

                NoteObject tmp = m_MusicPool.InstantiateObject(NoteKeyCode.Line, pos, Quaternion.identity);
                tmp.Initialize(BeatDefaultMat);
                noteMover.AddNoteList(tmp);
            }
        }

        for (int i = 0; i < barCount + 1; i++)
        {
            // Note
            int barNum = BarCount + i;

            List<NoteData> noteList;
            if (noteContainer.TryGetNoteListByBarIndex(barNum, out noteList))
            {
                for (int j = 0; j < noteList.Count; j++)
                {
                    NoteData data = noteList[j];

                    // 한마디 에서의 위치 거리
                    float posZ = (offsetPos + (BarDistance * (i - 1))) + ((BarDistance * 0.03125f) * data.BeatIndex);

                    if(data.NoteType == NoteKeyCode.LongStart || data.NoteType == NoteKeyCode.LongEnd)
                    {
                        MakeNote(data, posZ, offsetPos);
                    }
                    else
                    {
                        if (posZ >= 0f)
                        {
                            MakeNote(data, posZ, offsetPos);
                        }
                    }
                }
            }
        }

        OnResetPos?.Invoke(barCount, offsetPos);
    }

    public void InitMusicSample()
    {
        if(m_Music.musicTimeSample == 0)
        {
            BarCount = 0;
            BeatCount = 0;
            BarPos = 0f;
        }
        else
        {
            BarCount = m_Music.musicTimeSample / m_Music.BarPerTimeSample;
            BarPos = ((float)m_Music.musicTimeSample / m_Music.BarPerTimeSample) - BarCount;

            BeatCount = (int)(BarPos / 0.03125f);
        }
        
        BeforeBeatCount = BeatCount;
    }

    public void InitMusic()
    {
        m_Music.Init(GetSheet.BPM, GetSheet.Offset);
        IsMusicLoaded = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            Time.timeScale = 0f;

            if (IsMusicLoaded)
            {
                GetMusic.Pause();
            }
        }
        else
        {
            Time.timeScale = 1f;

            if (IsMusicLoaded)
            {
                GetMusic.Play();
            }
        }
    }
}
