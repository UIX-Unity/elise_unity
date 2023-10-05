using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using PlayFab;
using PlayFab.ClientModels;
public enum Accuracy
{
    PERFECT = 0,
    FINE,
    BAD,
    MISS
}

[Serializable]
public struct KeyEffect
{
    public ParticleSystem[] effect;
}

public class BMSPlayer : MonoBehaviour
{
    [SerializeField] UnityEngine.GameObject gameSecne;
    [SerializeField] UnityEngine.GameObject totalControl;
    [SerializeField] NoteManager noteManager;
    GameSetting info;

    [SerializeField] NoteData[] noteDatas = new NoteData[6];
    [SerializeField] float hitLine;
    [SerializeField] UISprite lineSprite;
    [SerializeField] UnityEngine.GameObject hitLineObject;

    [SerializeField] float accuracy = 0;
    [SerializeField] int totalNoteCount = 0;

    [SerializeField] List<KeyLightEffect> effect;
    [SerializeField] List<bool> isPress;

    [Header("Accuracy Effect")]
    [SerializeField] Animator accEffect;
    [SerializeField] UILabel comboLabel;
    [SerializeField] UILabel accLabel;
    [SerializeField] GameEffectColor effectColor;

    [SerializeField] int combo;
    [SerializeField] int bestCombo;

    [Header("Result Panel")]
    [SerializeField] Animator resultOpenAnimation;
    [SerializeField] ParticleSystem[] gearEffect;
    [SerializeField] UIPanel resultPanel;
    [SerializeField] UILabel resultAccLabel;

    [SerializeField] UILabel perfectLabel;
    [SerializeField] UILabel greatLabel;
    [SerializeField] UILabel goodLabel;
    [SerializeField] UILabel missLabel;
    [SerializeField] UILabel totalResultLabel;
    [SerializeField] UILabel comboResultLabel;
    //이펙트잔상제거용
    [SerializeField] UnityEngine.GameObject effSetFlase;

    [Header("Accuracy Label")]
    [SerializeField] UILabel accuracyLabel;

    [Header("Timerbar")]
    [SerializeField] UISlider musicSlider;
    [SerializeField] AudioSource music;

    [Header("Pause")]
    [SerializeField] UnityEngine.GameObject PausePanel;

    [Header("판정이펙트")]
    [SerializeField] ParticleSystem[] hitEffect; // 0 perfect 1 great 2 good
    [SerializeField] KeyEffect[] keyEffect;

    public int perfectCount = 0;
    public int greatCount = 0;
    public int goodCount = 0;
    public int missCount = 0;

    public bool isStart = false;
    public bool isPause = false;

    public bool expbutton = false;
    public bool stopCheck = false;

    //플레이팹점수 불러오기용
    private float tempScore;
    //첫 점수
    private bool fristPlay;

    [SerializeField] UnityEngine.GameObject rankObject;
    [SerializeField] UnityEngine.GameObject expObject;

    [SerializeField] UILabel[] expLabel;

    IEnumerator corutine;

    int keyNumberCheck;
    bool[] longNoteCheck;
    float timeTarget;
    float[] time;


    public int fullNoteCount = 0;

    private void Start()
    {
        StartCoroutine(MusicSlider());
        info = Resources.Load("GameSetting") as GameSetting;
        noteManager.bgmSync -= info.noteSync * noteManager.moveValueNoneTime;

        totalControl.SetActive(false);

        longNoteCheck = new bool[6];

        time = new float[6];

        for (int i = 0; i < longNoteCheck.Length; i++)
        {
            longNoteCheck[i] = false;
        }

        timeTarget = 0.2f;
        //Debug.Log(fullNoteCount);


        tempScore = 0;
        fristPlay = true;
        //데이터 받아오기
        if (info.loginCheck == true)
        {
            //최대 진행률 검사
            //3개의 함수가 들어있음
            //네트워크에서 받아오는 변수의 값이 함수안에서는 고정이지만 밖으로나오면 바뀌여버린다.
            MaxMusicSocreLoad();
        }
    }

    public void SetInputDatas(NoteData[] noteDatas)
    {
        this.noteDatas = noteDatas.Clone() as NoteData[];
    }

    public void Pause()
    {
        if (!isStart)
            return;

        if (!isPause)
        {
            music.Pause();
            gameSecne.GetComponent<GameScene>().buttonCheck = true;
            PausePanel.SetActive(true);
        }
        else
        {
            music.Play();
            gameSecne.GetComponent<GameScene>().buttonCheck = false;
            PausePanel.SetActive(false);
        }

        isPause = !isPause;
    }


    private IEnumerator MusicSlider()
    {
        while (musicSlider.value < 1f)
        {
            musicSlider.value = (music.time / music.clip.length);
            yield return null;
        }


        gearEffect[0].Pause();
        gearEffect[1].Pause();
        effSetFlase.SetActive(false);

        resultOpenAnimation.Play("ResultOpen");
        ResultUpdate();
    }

    private void ResultUpdate()
    {
        

        float thistime = 0;
        float time = 0.4f;


        while (thistime < time)
        {
            thistime += Time.deltaTime;
        }
        //perfectLabel.text = perfectCount.ToString();
        //greatLabel.text = greatCount.ToString();
        //goodLabel.text = goodCount.ToString();
        //missLabel.text = missCount.ToString();
        //totalResultLabel.text = totalNoteCount.ToString();
        //comboResultLabel.text = bestCombo.ToString();
        corutine = TotalSocre();
        StartCoroutine(corutine);
        //StartCoroutine(PerfectCoroutine(perfectCount, perfectLabel));
        //StartCoroutine(PerfectCoroutine(greatCount, greatLabel));
        //StartCoroutine(PerfectCoroutine(goodCount, goodLabel));
        //StartCoroutine(PerfectCoroutine(missCount, missLabel));
        //StartCoroutine(PerfectCoroutine(totalNoteCount, totalResultLabel));
        //StartCoroutine(PerfectCoroutine(bestCombo, comboResultLabel));
    }

    public void StopTotalSocre()
    {
        if (corutine != null)
        {
            StopCoroutine(corutine);
            perfectLabel.text = perfectCount.ToString();
            greatLabel.text = greatCount.ToString();
            goodLabel.text = goodCount.ToString();
            missLabel.text = missCount.ToString();
            totalResultLabel.text = totalNoteCount.ToString();
            comboResultLabel.text = bestCombo.ToString();
            rankObject.SetActive(true);
            StartCoroutine(expStart());
        }
    }

    //IEnumerator PerfectCoroutine(float scoreTarget, UILabel textlabel)
    //{

    //    float duration = 3f; // 카운팅에 걸리는 시간 설정.
    //    float current = 0;
    //    float offset = (scoreTarget - current) / duration;

    //    while (current < scoreTarget)
    //    {
    //        current += offset * Time.deltaTime;
    //        textlabel.text = ((int)current).ToString();
    //        yield return null;
    //    }


    //    current = scoreTarget;
    //    textlabel.text = ((int)current).ToString();
    //}
    public void SetStatExp()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "exp", info.exp.ToString("N2") }} };
        PlayFabClientAPI.UpdateUserData(request, (result) => Debug.Log("데이터 저장 성공"), (error) => Debug.Log("데이터 저장 실패"));

    }
    public void MaxMusicSocreLoad()
    {
        

        Debug.Log("---------------------------------------");
        var request = new GetUserDataRequest() { PlayFabId = info.myId };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            foreach (var eachData in result.Data)
            {
                
                if (eachData.Key == "music"+"0"+info.episodeIndex.ToString() + "0" + info.musicIndex.ToString() + "0" + info.lv.ToString())
                {
                    tempScore = float.Parse(eachData.Value.Value);
                    fristPlay = false;
                    return;
                }

            }
        }, (error) => Debug.Log("데이터 불러오기실패"));
      
    }
    public void SetMaxMusicSocreSave()
    {


        if (tempScore <= float.Parse(accuracyLabel.text.Replace("%", "")))
        {
            var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "music" + "0" + info.episodeIndex.ToString() + "0" + info.musicIndex.ToString() + "0" + info.lv.ToString(), accuracyLabel.text.Replace("%", "") } } };
            PlayFabClientAPI.UpdateUserData(request, (result) =>
            {
                SetEpiData();
            }
            , (error) => Debug.Log("데이터 저장 실패"));
            //데이터 저장이 끝나고 난후 에피소드 진행도!
       
          
        }

        Debug.Log("---------------------------------------");
    }
    //첫플레이와 로컬데이터 저장
    public void SetLocalDataSave()
    {

        //if (info.episodes[info.episodeIndex].musics[info.musicIndex].firstPlay[info.lv] == false)
        //값을 불러왔을때 첫플레이면 0 
        if (fristPlay == true)
        {
            info.exp = Mathf.Min(100, info.exp + info.firstExp + (float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp));
            expLabel[0].text = "First Play !";
            expLabel[1].text = ((float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp) + 20).ToString("N2");
            info.episodes[info.episodeIndex].musics[info.musicIndex].firstPlay[info.lv] = true;

        }
        else
        {
            //exp획득량
            info.exp = Mathf.Min(100, info.exp + (float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp));
            expLabel[1].text = (float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp).ToString("N2");

        }
        SetMaxMusicSocreSave();
    }
    public void SetEpiData()
    {
        //곡진행율의 합
        float maxScore = 0;
            //데이터 불러오기
            var request = new GetUserDataRequest() { PlayFabId = info.myId };
            PlayFabClientAPI.GetUserData(request, (result) =>
            {
                
                bool sample = false;
                foreach (var eachData in result.Data)
                {
                    
                    Debug.Log("Test : " +eachData.Value.Value[0]);
                    if (Equals(eachData.Key[0], 'm'))
                    {
                        Debug.Log("Test5 : Log!");
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (Equals(eachData.Key, "music" + "0" + info.episodeIndex.ToString() + "0" + i.ToString() + "0" + j.ToString())) ;
                                {
                                    Debug.Log("maxScore: " + maxScore);
                                    maxScore += float.Parse(eachData.Value.Value);
                                    sample = true;
                                    break;
                                }
                            }
                            if (Equals(sample, true))
                            {
                                sample = false;
                                break;
                            }
                        }
                    }

                }
                //데이터 저장
                //에피소드 진행률 네트워크 저장
                var request1 = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "episode" + "0" + info.episodeIndex.ToString(), maxScore.ToString() } } };
                PlayFabClientAPI.UpdateUserData(request1, (result1) => Debug.Log("데이터 저장 성공"), (error) => Debug.Log("데이터 저장 실패"));
            //에피소드 진행률 저장
            info.epiMaxMusicSocre[info.episodeIndex] = maxScore;


            }, (error) => Debug.Log("데이터 불러오기실패"));
        

    }
    IEnumerator expStart()
    {
        
        yield return new WaitForSeconds(1f);

        //반대사용
        expObject.SetActive(true);

        if (info.rankRange[0] < float.Parse(accuracyLabel.text.Replace("%", "")))
        {
            expLabel[0].text = "S Rank";
        }
        else if (info.rankRange[1] < float.Parse(accuracyLabel.text.Replace("%", "")) &&
            float.Parse(accuracyLabel.text.Replace("%", "")) < info.rankRange[0])
        {
            expLabel[0].text = "A Rank";
        }
        else if (info.rankRange[2] < float.Parse(accuracyLabel.text.Replace("%", "")) &&
             float.Parse(accuracyLabel.text.Replace("%", "")) < info.rankRange[1])
        {
            expLabel[0].text = "B Rank";
        }
        else if (info.rankRange[3] < float.Parse(accuracyLabel.text.Replace("%", "")) &&
            float.Parse(accuracyLabel.text.Replace("%", "")) < info.rankRange[2])
        {
            expLabel[0].text = "C Rank";
        }
        else if (info.rankRange[4] < float.Parse(accuracyLabel.text.Replace("%", "")) &&
           float.Parse(accuracyLabel.text.Replace("%", "")) < info.rankRange[3])
        {
            expLabel[0].text = "D Rank";
        }
        else
        {
            expLabel[0].text = "F Rank";
        }

        //데이터 세이브
        if (info.loginCheck == true)
        {
            SetLocalDataSave();

            //데이터 저장
            SetStatExp();
           
        }
        else
        {
            if (info.episodes[info.episodeIndex].musics[info.musicIndex].firstPlay[info.lv] == false)
            {
                info.exp = Mathf.Min(100, info.exp + info.firstExp + (float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp));
                expLabel[0].text = "First Play !";
                expLabel[1].text = ((float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp) + 20).ToString("N2");
                info.episodes[info.episodeIndex].musics[info.musicIndex].firstPlay[info.lv] = true;

            }
            else
            {
                //exp획득량
                info.exp = Mathf.Min(100, info.exp + (float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp));
                expLabel[1].text = (float.Parse(accuracyLabel.text.Replace("%", "")) / info.rankPlusExp).ToString("N2");

            }
        }
        

        
    


        float color = expObject.GetComponent<UIPanel>().alpha = 0;
        //Color color = new Color(0, 0, 0, 1);
        float t = 0f;
        float time = 1f;
        while (color < 1)
        {
            t += Time.deltaTime / time;
            color = Mathf.Lerp(0, 1, t);

            expObject.GetComponent<UIPanel>().alpha = color;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        expbutton = true;
    }
    public void expEndFade()
    {
        StartCoroutine(expEnd());
    }
    IEnumerator expEnd()
    {
        yield return new WaitForSeconds(1f);

        float color = expObject.GetComponent<UIPanel>().alpha = 1;
        //Color color = new Color(0, 0, 0, 1);
        float t = 0f;
        float time = 1f;
        while (color > 0)
        {
            t += Time.deltaTime / time;
            color = Mathf.Lerp(1, 0, t);

            expObject.GetComponent<UIPanel>().alpha = color;
            yield return null;
        }

        expObject.SetActive(false);
        gameSecne.GetComponent<GameScene>().buttonCheck = true;
    }
    //스코어 랭크 연출
    IEnumerator TotalSocre()
    {

        totalResultLabel.text = totalNoteCount.ToString();
        yield return new WaitForSeconds(1f);
        totalControl.SetActive(true);

        float duration = 1f; // 카운팅에 걸리는 시간 설정.
        float current = 0;
        float offset = (perfectCount - current) / duration;

        while (current < perfectCount)
        {
            current += offset * Time.deltaTime;
            perfectLabel.text = ((int)current).ToString();
            yield return null;
        }
        current = perfectCount;
        perfectLabel.text = ((int)current).ToString();


        current = 0;
        offset = (greatCount - current) / duration;

        while (current < greatCount)
        {
            current += offset * Time.deltaTime;
            greatLabel.text = ((int)current).ToString();
            yield return null;
        }
        current = greatCount;
        greatLabel.text = ((int)current).ToString();


        current = 0;
        offset = (goodCount - current) / duration;

        while (current < goodCount)
        {
            current += offset * Time.deltaTime;
            goodLabel.text = ((int)current).ToString();
            yield return null;
        }
        current = goodCount;
        goodLabel.text = ((int)current).ToString();


        current = 0;
        offset = (missCount - current) / duration;

        while (current < missCount)
        {
            current += offset * Time.deltaTime;
            missLabel.text = ((int)current).ToString();
            yield return null;
        }
        current = missCount;
        missLabel.text = ((int)current).ToString();



        current = 0;
        offset = (bestCombo - current) / duration;

        while (current < bestCombo)
        {
            current += offset * Time.deltaTime;
            comboResultLabel.text = ((int)current).ToString();
            yield return null;
        }
        current = bestCombo;
        comboResultLabel.text = ((int)current).ToString();

        stopCheck = true;
        rankObject.SetActive(true);
        StopTotalSocre();
    }


    public float GetHitLinePosition()
    {
        return hitLine;
    }

    private IEnumerator AccuracyLabelUpdate()
    {
        //디모 방식 0.00%에서 시작
        float timer = 0f;
        float tempAcc = float.Parse(accuracyLabel.text.Replace("%", "")) / 100;

        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            float acc = Mathf.Lerp(tempAcc, Mathf.Max(0, accuracy / (float)fullNoteCount), timer / 0.2f);
         
            accuracyLabel.text = string.Format("{0:F2}%", Math.Max(0.00f,Math.Min(100.00f,Math.Round(acc * 100, 2))));
            yield return null;
        }

        //ous방식  100.00% 에서 시작
        //float timer = 0f;
        //float tempAcc = float.Parse(accuracyLabel.text.Replace("%", "")) / 100;

        //while (timer < 0.2f)
        //{
        //    timer += Time.deltaTime;
        //    float acc = Mathf.Lerp(tempAcc, accuracy / totalNoteCount, timer / 0.2f);
        //    accuracyLabel.text = string.Format("{0:F2}%", Math.Round(acc * 100, 2));
        //    yield return null;
        //}
    }

    private void SetBestCombo()
    {
        if (combo > bestCombo)
        {
            bestCombo = combo;
        }
    }

    public void PlayAccuracyAnimation(Accuracy type, int keyNumber)
    {
        switch (type)
        {
            case Accuracy.PERFECT: // PERFECT
                combo++;
                perfectCount++;
                accuracy += 1f;
                totalNoteCount++;
                comboLabel.text = combo.ToString();
                hitEffect[1].Clear();
                hitEffect[2].Clear();
                hitEffect[3].Clear();
                hitEffect[0].Play();
                keyEffect[0].effect[keyNumber].Play();
                SetBestCombo();

                accEffect.gameObject.SetActive(false);
                accEffect.gameObject.SetActive(true);

                break;

            case Accuracy.FINE: // GREAT
                combo++;
                greatCount++;
                accuracy += 0.7f;
                totalNoteCount++;
                comboLabel.text = combo.ToString();
                hitEffect[0].Clear();
                hitEffect[2].Clear();
                hitEffect[3].Clear();
                hitEffect[1].Play();
                keyEffect[1].effect[keyNumber].Play();
                SetBestCombo();

                accEffect.gameObject.SetActive(false);
                accEffect.gameObject.SetActive(true);
                break;

            case Accuracy.BAD: // GOOD
                combo++;
                goodCount++;
                accuracy += 0.4f;
                totalNoteCount++;
                comboLabel.text = combo.ToString();

                // accLabel.text = "BAD";

                hitEffect[1].Clear();
                hitEffect[0].Clear();
                hitEffect[3].Clear();
                hitEffect[2].Play();
                keyEffect[2].effect[keyNumber].Play();
                SetBestCombo();
                accEffect.gameObject.SetActive(false);
                accEffect.gameObject.SetActive(true);

                break;

            case Accuracy.MISS:
                missCount++;
                //comboLabel.gradientTop = effectColor.comboColorUpGradient;
                //comboLabel.gradientBottom = effectColor.comboColorDownGradient;
                //accLabel.gradientTop = effectColor.comboColorUpGradient;
                //accLabel.gradientBottom = effectColor.comboColorDownGradient;
                combo = 0;
                totalNoteCount++;
                comboLabel.text = combo.ToString();

                //accLabel.text = "MISS";

                hitEffect[1].Clear();
                hitEffect[0].Clear();
                hitEffect[2].Clear();
                hitEffect[3].Play();

                //accEffect.gameObject.SetActive(false);
                //accEffect.gameObject.SetActive(true);
                break;
        }
    }

    public void PlayNoteAnimation(int keyNumber, bool isPerfect)
    {
        int state = noteManager.GetActicveCurrentIndex(noteDatas[keyNumber].noteList[0], keyNumber);

        if (state != -1)
        {
            noteManager.AnimationCoroutineCall(state, isPerfect);
        }

        StartCoroutine(AccuracyLabelUpdate());
    }

    private void HitLineCalculate(int keyNumber)
    {
        if (isPause)
            return;

        if (isPress[keyNumber])
            return;

        isPress[keyNumber] = true;

        if (noteDatas[keyNumber].noteList.Count == 0)
            return;

        if (noteDatas[keyNumber].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * info.noteRange[0] /*0.088f*/) &&
            noteDatas[keyNumber].noteList[0].position > hitLine - (noteManager.moveValueNoneTime * info.noteRange[0]))
        {
            PlayAccuracyAnimation(Accuracy.PERFECT, keyNumber);
            PlayNoteAnimation(keyNumber, true);
            noteDatas[keyNumber].noteList.RemoveAt(0);
        }
        else if (noteDatas[keyNumber].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * info.noteRange[1]) &&
            noteDatas[keyNumber].noteList[0].position > hitLine - (noteManager.moveValueNoneTime * info.noteRange[1]))
        {
            PlayAccuracyAnimation(Accuracy.FINE, keyNumber);
            PlayNoteAnimation(keyNumber, false);
            noteDatas[keyNumber].noteList.RemoveAt(0);
        }
        else if (noteDatas[keyNumber].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * info.noteRange[2]))
        {
            PlayAccuracyAnimation(Accuracy.BAD, keyNumber);
            PlayNoteAnimation(keyNumber, false);
            noteDatas[keyNumber].noteList.RemoveAt(0);
        }
    }
    private void LongNoteCheck(int keyNumber)
    {
        if (noteDatas[keyNumber].noteList.Count == 0)
            return;

        if ( noteDatas[keyNumber].noteList[0].channelNumber == 6)
            longNoteCheck[keyNumber] = true;
        else
            longNoteCheck[keyNumber] = false;
    }
    private void LongNotNotCheck(int keyNumber)
    {
        if (noteDatas[keyNumber].noteList.Count == 0)
            return;
        longNoteCheck[keyNumber] = false;
    }
    private void LongNoteCalculate(int keyNumber)
    {
        if (isPause)
            return;

        if (!isPress[keyNumber])
            return;

        isPress[keyNumber] = false;

        if (noteDatas[keyNumber].noteList.Count == 0)
            return;

        if (noteDatas[keyNumber].noteList[0].channelNumber == 6)
        {
            if (noteDatas[keyNumber].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * info.noteRange[0]) &&
                noteDatas[keyNumber].noteList[0].position > hitLine - (noteManager.moveValueNoneTime * info.noteRange[0]))
            {
                PlayAccuracyAnimation(Accuracy.PERFECT, keyNumber);
                PlayNoteAnimation(keyNumber, true);
                noteDatas[keyNumber].noteList.RemoveAt(0);
            }
            else if (noteDatas[keyNumber].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * info.noteRange[1]) &&
                noteDatas[keyNumber].noteList[0].position > hitLine - (noteManager.moveValueNoneTime * info.noteRange[1]))
            {
                PlayAccuracyAnimation(Accuracy.FINE, keyNumber);
                PlayNoteAnimation(keyNumber, false);
                noteDatas[keyNumber].noteList.RemoveAt(0);
            }
            else if (noteDatas[keyNumber].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * info.noteRange[2]))
            {
                PlayAccuracyAnimation(Accuracy.BAD, keyNumber);
                PlayNoteAnimation(keyNumber, false);
                noteDatas[keyNumber].noteList.RemoveAt(0);
            }
            else
            {
                PlayAccuracyAnimation(Accuracy.MISS, keyNumber);
                PlayNoteAnimation(keyNumber, false);
                noteDatas[keyNumber].noteList.RemoveAt(0);
            }
        }
    }
  
    void Update()
    {
        if (!(noteDatas[0].noteList.Count == 0))
        {
            if (longNoteCheck[0] == true && noteDatas[0].noteList[0].channelNumber == 6)
            {
                if (noteDatas[0].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * 0.001))
                {
                    longNoteCheck[0] = false;
                }
                time[0] += Time.deltaTime;
                if (time[0] > timeTarget)
                {
                    keyEffect[0].effect[0].Play();
                    time[0] = 0;
                }
            }
        }
        if (!(noteDatas[1].noteList.Count == 0))
        {
            if (longNoteCheck[1] == true && noteDatas[1].noteList[0].channelNumber == 6)
            {
                if (noteDatas[1].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * 0.001))
                {
                    longNoteCheck[1] = false;
                }
                time[1] += Time.deltaTime;
                if (time[1] > timeTarget)
                {
                    keyEffect[0].effect[1].Play();
                    time[1] = 0;
                }
            }
        }
        if (!(noteDatas[2].noteList.Count == 0))
        {
            if (longNoteCheck[2] == true && noteDatas[2].noteList[0].channelNumber == 6)
            {
                if (noteDatas[2].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * 0.001))
                {
                    longNoteCheck[2] = false;
                }
                time[2] += Time.deltaTime;
                if (time[2] > timeTarget)
                {
                    keyEffect[0].effect[2].Play();
                    time[2] = 0;
                }
            }
        }
        if (!(noteDatas[3].noteList.Count == 0))
        {
            if (longNoteCheck[3] == true && noteDatas[3].noteList[0].channelNumber == 6)
            {
                if (noteDatas[3].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * 0.001))
                {
                    longNoteCheck[3] = false;
                }
                time[3] += Time.deltaTime;
                if (time[3] > timeTarget)
                {
                    keyEffect[0].effect[3].Play();
                    time[3] = 0;
                }
            }
        }
        if (!(noteDatas[4].noteList.Count == 0))
        {
            if (longNoteCheck[4] == true && noteDatas[4].noteList[0].channelNumber == 6)
            {
                if (noteDatas[4].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * 0.001))
                {
                    longNoteCheck[4] = false;
                }
                time[4] += Time.deltaTime;
                if (time[4] > timeTarget)
                {
                    time[4] = 0;
                    keyEffect[0].effect[4].Play();
                }
            }
        }
        if (!(noteDatas[5].noteList.Count == 0))
        {
            if (longNoteCheck[5] == true && noteDatas[5].noteList[0].channelNumber == 6)
            {
                if (noteDatas[5].noteList[0].position < hitLine + (noteManager.moveValueNoneTime * 0.001))
                {
                    longNoteCheck[5] = false;
                }
                time[5] += Time.deltaTime;
                if (time[5] > timeTarget)
                {
                    keyEffect[0].effect[5].Play();
                    time[5] = 0;
                }
            }
        }
    }



    public void KeyDown1()
    {
        HitLineCalculate(0);
    }

    public void KeyDown2()
    {
        HitLineCalculate(1);
    }

    public void KeyDown3()
    {
        HitLineCalculate(2);
    }

    public void KeyDown4()
    {
        HitLineCalculate(3);
    }

    public void KeyDown5()
    {
        HitLineCalculate(4);
    }

    public void KeyDown6()
    {
        HitLineCalculate(5);
    }

    public void KeyPrees1()
    {
        LongNoteCheck(0);
    }

    public void KeyPrees2()
    {
        LongNoteCheck(1);
    }

    public void KeyPrees3()
    {
        LongNoteCheck(2);
    }

    public void KeyPrees4()
    {
        LongNoteCheck(3);
    }

    public void KeyPrees5()
    {
        LongNoteCheck(4);
    }

    public void KeyPrees6()
    {
        LongNoteCheck(5);
    }

    public void KeyPreesUp1()
    {
        LongNotNotCheck(0);
    }

    public void KeyPreesUp2()
    {
        LongNotNotCheck(1);
    }

    public void KeyPreesUp3()
    {
        LongNotNotCheck(2);
    }

    public void KeyPreesUp4()
    {
        LongNotNotCheck(3);
    }

    public void KeyPreesUp5()
    {
        LongNotNotCheck(4);
    }

    public void KeyPreesUp6()
    {
        LongNotNotCheck(5);
    }


    public void KeyUp1()
    {
        LongNoteCalculate(0);
    }

    public void KeyUp2()
    {
        LongNoteCalculate(1);
    }

    public void KeyUp3()
    {
        LongNoteCalculate(2);
    }

    public void KeyUp4()
    {
        LongNoteCalculate(3);
    }

    public void KeyUp5()
    {
        LongNoteCalculate(4);
    }

    public void KeyUp6()
    {
        LongNoteCalculate(5);
    }
}