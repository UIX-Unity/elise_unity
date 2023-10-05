using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] GameSetting info;

    [SerializeField] NoteData[] noteDatas = new NoteData[6];
    [SerializeField] List<NoteObject> notePooling = new List<NoteObject>();
    [SerializeField] List<NoteObject> activePooling = new List<NoteObject>();

    [SerializeField] NoteData[] inputDatas = new NoteData[6];

    [SerializeField] UnityEngine.GameObject noteObject;

    [SerializeField] List<Transform> notePos = new List<Transform>();

    [Header("풀링 상태 확인용")]
    [SerializeField] Transform poolingBank;
    [SerializeField] Transform activeBank;

    [Header("노래를 재생하는 오브젝트")]
    [SerializeField] UnityEngine.GameObject bgmNote;

    [Header("재생할 오디오소스")]
    [SerializeField] AudioSource audio;

    [Header("메소드 호출을 위해 가져오는 클래스")]
    [SerializeField] BMSPlayer player;

    [Header("연출")]
    [SerializeField] UISprite[] gear;
    [SerializeField] Animator hitAnimation;
    public bool autoMode = false;

    public float moveValue;
    private Vector3 moveVector;

    public float moveValueNoneTime;


    private float showPos = 2000f;

    private float resetPos = -2400f;

    public float bgmSync = -2050f;

    //[SerializeField] GameObject PausePanel;

    //public void Pause()
    //{
    //    if(BMSPlayer.GetInstance.isStart)
    //    {
    //        BMSPlayer.GetInstance.isStart = false;
    //        a.Pause();
    //        PausePanel.SetActive(true);
    //    }
    //    else if (!music.activeSelf)
    //    {
    //        a.Play();
    //        PausePanel.SetActive(false);
    //        BMSPlayer.GetInstance.isStart = true;
    //    }
    //}
    private void Awake()
    {
        Application.targetFrameRate = 60;
        audio.clip = info.selectedEpisode.musics[info.musicIndex].music;
        InitializePooling(50);
        showPos *= info.speed * 2;
        moveValueNoneTime = info.BPM / (4f * 60f) * 1000f * info.speed;
        moveValue = (info.BPM / (4f * 60f) * 1000f * info.speed) * Time.fixedDeltaTime;
        moveVector = new Vector3(0, moveValue, 0);

        autoMode = false;

    }
    void FixedUpdate()
    {
        //gear[0].transform.Rotate(Vector3.forward * Time.fixedDeltaTime * Combo / 20);
        //gear[1].transform.Rotate(Vector3.back * Time.fixedDeltaTime * Combo / 20);

        if (player.isStart && !player.isPause)
        {
            if (bgmNote != null)
            {
                bgmNote.transform.localPosition -= moveVector;
                if (bgmNote.transform.localPosition.y < bgmSync)
                {
                    audio.Play();
                    Destroy(bgmNote);
                }
            }

            NoteMove();
        }
    }

    public int GetActicveCurrentIndex(Note note, int keyNumber)
    {
        for (int i = 0; i < activePooling.Count; i++)
        {
            if (activePooling[i].note.noteNumber == note.noteNumber)
                return i;
        }

        return -1;
    }

    public void AnimationCoroutineCall(int index, bool isPerfect)
    {
        StartCoroutine(AnimationCoroutineController(index, isPerfect));
    }

    private IEnumerator AnimationCoroutineController(int index, bool isPerfect)
    {
        yield return StartCoroutine(AnimationCoroutine(index, isPerfect));
    }

    private IEnumerator AnimationCoroutine(int index, bool isPerfect)
    {
        NoteObject noteObject = activePooling[index];

        activePooling.Remove(noteObject);

        if (isPerfect)
        {
            noteObject.noteObject.transform.localPosition = new Vector2(
                noteObject.noteObject.transform.localPosition.x,
                player.GetHitLinePosition());
        }

        float timer = 0f;

        while (timer <= 0.2f)
        {
            timer += Time.fixedDeltaTime;
            float scale = Mathf.Lerp(1, 1.6f, timer / 0.2f);
            float alpha = Mathf.Lerp(1, 0, timer / 0.2f);

            noteObject.noteObject.transform.localScale = new Vector3(scale, scale, scale);
            noteObject.noteObject.GetComponent<UISprite>().color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        noteObject.noteObject.transform.parent = poolingBank;
        noteObject.noteObject.SetActive(false);
        noteObject.longObject.gameObject.SetActive(false);
        noteObject.noteObject.transform.localScale = Vector3.one;
        noteObject.noteObject.GetComponent<UISprite>().color = Vector4.one;

        notePooling.Add(noteObject);

        yield return null;
    }

    //private IEnumerator End()
    //{
    //    yield return new WaitForSeconds(5f);
    //    resultAni.Play("ResultOpen");
    //}

    private void InitializePooling(int count)
    {
        for (int i = 0; i < count; i++)
        {
            UnityEngine.GameObject temp = Instantiate(noteObject, poolingBank) as UnityEngine.GameObject;
            NoteObject noteTemp = new NoteObject();
            noteTemp.noteObject = temp;
            noteTemp.longObject = temp.transform.GetChild(0).GetComponent<UISprite>();
            notePooling.Add(noteTemp);
        }
    }
    public void CreateNote(Note note)
    {
        if (notePooling.Count == 0)
        {
            UnityEngine.GameObject temp = Instantiate(noteObject, activeBank) as UnityEngine.GameObject;
            temp.transform.localPosition = new Vector3(notePos[note.keyNumber - 1].transform.localPosition.x, note.position);
            temp.SetActive(true);
            NoteObject noteTemp = new NoteObject();
            noteTemp.noteObject = temp;
            noteTemp.longObject = temp.transform.GetChild(0).GetComponent<UISprite>();
            noteTemp.note = note;
            activePooling.Add(noteTemp);

            inputDatas[note.keyNumber - 1].noteList.Add(note);

            if (note.channelNumber == 6)
            {
                noteTemp.longObject.gameObject.SetActive(true);
                noteTemp.longObject.height = note.longNoteSize;
            }
        }
        else
        {
            notePooling[0].noteObject.transform.localPosition = new Vector3(notePos[note.keyNumber - 1].transform.localPosition.x, note.position);
            notePooling[0].noteObject.SetActive(true);
            notePooling[0].noteObject.transform.parent = activeBank;
            notePooling[0].note = note;

            inputDatas[note.keyNumber - 1].noteList.Add(note);

            if (note.channelNumber == 6)
            {
                notePooling[0].longObject.gameObject.SetActive(true);
                notePooling[0].longObject.height = note.longNoteSize;
            }

            activePooling.Add(notePooling[0]);
            notePooling.RemoveAt(0);
        }
    }

    public void AddNote(Note note)
    {
        noteDatas[note.keyNumber - 1].noteList.Add(note);
    }


    private void BackToPooling(NoteObject obj)
    {
        obj.noteObject.transform.parent = poolingBank;
        obj.noteObject.SetActive(false);
        obj.longObject.gameObject.SetActive(false);
        notePooling.Add(obj);
        activePooling.Remove(obj);
    }

    // private void BackToPooling(int index)
    // {
    //     activePooling[index].noteObject.transform.parent = poolingBank;
    //     activePooling[index].noteObject.SetActive(false);
    //     activePooling[index].longObject.gameObject.SetActive(false);
    //     notePooling.Add(activePooling[index]);
    //     activePooling.RemoveAt(index);
    // }

    public void NoteMove()
    {
        for (int i = 0; i < activePooling.Count; i++)
        {
            activePooling[i].noteObject.transform.localPosition -= moveVector;

            if (activePooling[i].noteObject.transform.localPosition.y < player.GetHitLinePosition() -
                (moveValueNoneTime * 0.7f))
            {
                BackToPooling(activePooling[i]);
                i--;
                continue;
            }
        }

        // Pooling Note Move
        for (int i = 0; i < noteDatas.Length; i++)
        {
            for (int k = 0; k < noteDatas[i].noteList.Count; k++)
            {
                DecreasePosition(noteDatas[i].noteList, k, moveValue);
            }
        }

        if (autoMode)
        {
            for (int i = 0; i < noteDatas.Length; i++)
            {
                for (int k = 0; k < inputDatas[i].noteList.Count; k++)
                {
                    if (inputDatas[i].noteList[k].position < player.GetHitLinePosition())
                    {
                        player.PlayNoteAnimation(i, true);
                        player.PlayAccuracyAnimation(Accuracy.PERFECT, i);
                        inputDatas[i].noteList.Remove(inputDatas[i].noteList[k]);
                        k--;
                        continue;
                    }

                    DecreasePosition(inputDatas[i].noteList, k, moveValue);
                }
            }
        }
        else
        {
            for (int i = 0; i < noteDatas.Length; i++)
            {
                for (int k = 0; k < inputDatas[i].noteList.Count; k++)
                {
                    if (inputDatas[i].noteList[k].position < player.GetHitLinePosition() - (moveValueNoneTime * 0.4f))
                    {
                        player.PlayAccuracyAnimation(Accuracy.MISS, i);
                        player.PlayNoteAnimation(i, false);
                        inputDatas[i].noteList.Remove(inputDatas[i].noteList[k]);
                        k--;
                        continue;
                    }

                    DecreasePosition(inputDatas[i].noteList, k, moveValue);
                }
            }
        }

        NoteCreateCheck();
    }

    private void NoteCreateCheck()
    {

        for (int i = 0; i < noteDatas.Length; i++)
        {
            if (noteDatas[i].noteList.Count == 0)
                continue;

            if (noteDatas[i].noteList[0].position < showPos)
            {
                CreateNote(noteDatas[i].noteList[0]);
                noteDatas[i].noteList.Remove(noteDatas[i].noteList[0]);
            }
        }
    }

    private void SortNoteList(NoteData note)
    {
        for (int i = 0; i < note.noteList.Count; i++)
        {
            note.noteList.Sort(delegate (Note A, Note B)
            {
                if (A.position < B.position) return -1;
                else if (A.position > B.position) return 1;
                return 0;
            });
        }
    }

    public void SortNoteAll()
    {
        for (int i = 0; i < noteDatas.Length; i++)
        {
            SortNoteList(noteDatas[i]);
        }
    }

    private void DecreasePosition(List<Note> note, int index, float value)
    {
        Note temp = note[index];
        temp.position -= value;
        note[index] = temp;
    }

    private void SetChannel(List<Note> note, int index, int value)
    {
        Note temp = note[index];
        temp.channelNumber = value;
        temp.longNoteSize = (int)(note[index].position - note[index - 1].position);
        note[index] = temp;
    }

    public void ChannelSetting()
    {
        for (int i = 0; i < noteDatas.Length; i++)
        {
            for (int k = 0; k < noteDatas[i].noteList.Count; k++)
            {
                if (noteDatas[i].noteList.Count == 0) return;

                if (noteDatas[i].noteList[k].channelNumber == 5)
                {
                    SetChannel(noteDatas[i].noteList, k + 1, 6);
                }
            }
        }
    }

    public void SetPlayerInputData()
    {
        player.SetInputDatas(inputDatas);
    }
}