using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    public UnityEngine.GameObject[] EpisodePanel;
    public Animator cameraAni;

    public Animator[] panelAnimation;

    public UISprite[] test;

    public ParticleSystem selectEffect;

    public UnityEngine.GameObject circleParticle;

    //옵션 ui 스토리 ui를 묵어둔 오브젝트
    public UnityEngine.GameObject uiOtionStory;
    //에피소드 사운드 모음
    public UnityEngine.GameObject[] epListSound;


    //위오브젝트  활성화 비활성화 컨트롤
    public bool uiOtionStoryCotrol = true;


    GameSetting info;

    [SerializeField] UILabel episodeTitle;

    [Header("곡 선택")]
    [SerializeField] UnityEngine.GameObject[] songSelectPanel;
    [SerializeField] Animator[] songSelectPanelAnimation;

    [SerializeField] UnityEngine.GameObject speedPanel;
    [SerializeField] Animator speedPanelAnimation;

    [Header("난이도 이펙트")]
    [SerializeField] ParticleSystem difficultGearParticle;
    [SerializeField] ParticleSystem difficultTextParticle;
    [SerializeField] UILabel difficultLabel;
    [SerializeField] string[] difficult;

    [Header("옵션패널")]
    [SerializeField] UnityEngine.GameObject optionPanel;
    [SerializeField] UnityEngine.GameObject optionSelectPanel;
    [SerializeField] UnityEngine.GameObject optionCalPanel;
    [SerializeField] UnityEngine.GameObject optionVolPanel;

    [Header("테마 선택 이펙트 잔상 수리용")]
    [SerializeField] UnityEngine.GameObject[] themeBugFixObject;

    [Header("곡 선택 이펙트 잔상 수리용")]
    [SerializeField] UnityEngine.GameObject[] songBugFixObject;

    [Header("난이도 오브젝트")]
    [SerializeField] UnityEngine.GameObject[] difficultObj;

    [Header("볼륨")]
    [SerializeField] UISlider volumeSlider;
    [SerializeField] UISprite VolumeSprite;

    [SerializeField] UILabel[] songTitleLabel;

    [Header("Calibration")]
    [SerializeField] UISlider calSlider;
    [SerializeField] UISprite calSprite;
    [SerializeField] UILabel calText;


    /// <summary>
    /// testmod잠금
    /// </summary>
    public int testmod = 0;
    public bool testrock = true;
    private void Awake()
    {
        FadePanel.GetInstance.ScreenFade(true, 1.5f);
        info = Resources.Load("GameSetting") as GameSetting;
    }

    private void Start()
    {
        uiOtionStoryCotrol = true;

        if (!info.debugMode)
        {
            MainMusic.GetInstance.PlayMusic();
        }

        volumeSlider.value = info.sound;


        calSlider.value = info.noteSync+0.5f;
        calText.text = info.noteSync.ToString();
    }

    public void VolumeChanged()
    {

        MainMusic.GetInstance.audio.volume = info.sound;
        MainMusic.GetInstance.sound = info.sound;
        info.sound = volumeSlider.value;
    }

    public void VolumeUp()
    {
        int tempSound = Mathf.FloorToInt(info.sound * 100);
        if(tempSound % 10 == 0)
        {
            tempSound += 10;
        }
        else
        {
            tempSound += (10 - tempSound % 10);
        }
        info.sound = tempSound / 100f;

        // if (info.sound == (Mathf.Floor(info.sound * 10f) / 10f))
        // {
        //     info.sound += 0.1f;
        //     Debug.Log("Else");
        // }
        // else if(info.sound > Mathf.Floor(info.sound * 10f) / 10f)
        // {
        //     info.sound = float.Parse((Mathf.Ceil(info.sound * 10f) / 10f).ToString());
        // }

        info.sound = Mathf.Min(1, info.sound);
        MainMusic.GetInstance.audio.volume = info.sound;
        MainMusic.GetInstance.sound = info.sound;
        volumeSlider.value = info.sound;
    }

    public void VolumeDown()
    {
        int tempSound = Mathf.CeilToInt(info.sound * 100);
        Debug.Log(tempSound);
        if (tempSound % 10 == 0)
        {
            tempSound -= 10;
        }
        else
        {
            tempSound -= (10+ tempSound % 10);
        }
        info.sound = tempSound / 100f;

        // if((info.sound * 100) % 10 != 0)
        // {
        //     info.sound = Mathf.Floor(info.sound * 10) / 10;
        // }
        // else
        // {
        //     info.sound -= 0.1f;
        // }

        info.sound = Mathf.Max(0, info.sound);
        MainMusic.GetInstance.audio.volume = info.sound;
        MainMusic.GetInstance.sound = info.sound;
        volumeSlider.value = info.sound;
    }

    /// <summary>
    /// 에피소드 패널 여는 함수
    /// </summary>
    public void OpenEpisodePanel()
    {
        if (info.buttonControl == false)
        {
            info.buttonControl = true;
            EpisodePanel[0].GetComponent<ThemeScroll>().enabled = true;

            for (int i = 0; i < themeBugFixObject.Length; i++)
            {
                themeBugFixObject[i].SetActive(true);
            }
            for (int i = 0; i < EpisodePanel.Length; i++)
            {
                EpisodePanel[i].SetActive(true);
                //설정 스토리창 set false;
                uiOtionStory.SetActive(false);
                //
            }
            circleParticle.SetActive(true);
        }
    }

    /// <summary>
    /// 에피소드 패널 닫는 함수
    /// </summary>
    public void CloseEpisodePanel()
    {
        if(info.buttonControl == false)
        {
            //테스트 모드 잠금
            if (info.epiRock[info.episodeIndex] == true)
            {
                EpisodePanel[0].GetComponent<ThemeScroll>().enabled = false;
                StartCoroutine(Close());
            }
        }

    }
    public void TestModCloseEpisodePanel()
    {
     
        if(info.buttonControl == false)
        {
            StartCoroutine(Close());
        }
        
    }

    public void OpenSongSelectPanel()
    {
        if (info.buttonControl == false)
        {
            //테스트 모드 잠금
            if (info.epiRock[info.episodeIndex] == true)
            {

                //설정 스토리창 set false;
                uiOtionStory.SetActive(false);
                uiOtionStoryCotrol = false;

                //곡고르는 창으로들어갈시 메인음막 음소거
                MainMusic.GetInstance.PauseMusic();
                //

                for (int i = 0; i < songTitleLabel.Length; i++)
                {
                    songTitleLabel[i].text = info.episodes[info.episodeIndex].musics[i].name;
                }

                switch (info.lv)
                {
                    case 0:
                        difficultObj[0].SetActive(true);
                        difficultObj[1].SetActive(false);
                        difficultObj[2].SetActive(false);
                        break;

                    case 1:
                        difficultObj[0].SetActive(false);
                        difficultObj[1].SetActive(true);
                        difficultObj[2].SetActive(false);
                        break;

                    case 2:
                        difficultObj[0].SetActive(false);
                        difficultObj[1].SetActive(false);
                        difficultObj[2].SetActive(true);
                        break;
                }
                difficultGearParticle.Play();
                difficultTextParticle.Play();
                difficultLabel.text = difficult[info.lv];

                for (int i = 0; i < songBugFixObject.Length; i++)
                {
                    songBugFixObject[i].SetActive(true);
                }

                for (int i = 0; i < songSelectPanel.Length; i++)
                {
                    songSelectPanel[i].SetActive(true);
                }

            }
        }

    }

    public void CloseSongSelectPanel()
    {
        if (info.buttonControl == false)
        {
            //곡을고르는 창에서 에피소드창으로 넘어갈시 메인음악 play
            MainMusic.GetInstance.PlayMusic();

            uiOtionStoryCotrol = true;
            StartCoroutine(CloseSongSelect());
        }
    }

    public void OpenSpeedPanel()
    {
        if (info.buttonControl == false)
        {
            speedPanel.SetActive(true);
        }
    }

    public void CloseSpeedPanel()
    {
        if (info.buttonControl == false)
        {
            StartCoroutine(CloseSpeed());
        }
    }

    public void OpenOptionPanel()
    {
        if(info.buttonControl == false)
        {
            optionPanel.SetActive(true);
        }
    }

    public void CloseOptionPanel()
    {
        if (info.buttonControl == false)
        {
            if (optionSelectPanel.activeSelf)
            {
                StartCoroutine(CloseOption());
            }

            if (optionCalPanel)
            {
                MainMusic.GetInstance.PlayMusic();
                //optionCalPanel.SetActive(false);
                //optionSelectPanel.SetActive(true);
                StartCoroutine(CloseCal());
            }

            if (optionVolPanel)
            {
                //optionVolPanel.SetActive(false);
                //optionSelectPanel.SetActive(true);
                StartCoroutine(CloseVol());

            }
        }
    }

    public void OpenCalPanel()
    {
        if (info.buttonControl == false)
        {
            optionSelectPanel.SetActive(false);
            optionCalPanel.SetActive(true);
            MainMusic.GetInstance.PauseMusic();

            calSprite.enabled = false;
        }
    }

    public void OpenVolumePanel()
    {
        if (info.buttonControl == false)
        {
            optionSelectPanel.SetActive(false);
            optionVolPanel.SetActive(true);

            VolumeSprite.enabled = false;
        }
    }

    private IEnumerator Close()
    {
        circleParticle.SetActive(false);
       
        for (int i = 0; i < themeBugFixObject.Length; i++)
        {
            themeBugFixObject[i].SetActive(false);
        }

        for (int i=0; i< panelAnimation.Length; i++)
        {
            panelAnimation[i].Play("ThemeClose");
        }

        yield return new WaitForSeconds(0.6f);

        info.buttonControl = false;
        //EpisodePanel[0].GetComponent<ThemeScroll>().percentage = 0;
        //EpisodePanel[0].GetComponent<ThemeScroll>().destPercent = 0;
        //EpisodePanel[0].GetComponent<ThemeScroll>().index = 0;
        //EpisodePanel[0].GetComponent<ThemeScroll>().indexTemp = 0;

        info.episodeIndex = 0;
        for (int i = 0; i < EpisodePanel.Length; i++)
        {
            EpisodePanel[i].SetActive(false);
        }
        if(Equals(uiOtionStoryCotrol, true))
        {
            uiOtionStory.SetActive(true);
        }
    }

    private IEnumerator CloseSongSelect()
    {
     
        for (int i = 0; i < songBugFixObject.Length; i++)
        {
            songBugFixObject[i].SetActive(false);
        }

        for (int i = 0; i < songSelectPanelAnimation.Length; i++)
        {
            songSelectPanelAnimation[i].Play("ThemeClose");
        }
        for(int i =0;i< epListSound[0].GetComponent<EpSoundListControl>().songListSound.Length; i++)
        {
            epListSound[0].GetComponent<EpSoundListControl>().songListSound[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < songSelectPanel.Length; i++)
        {
            songSelectPanel[i].SetActive(false);
        }
    }

    private IEnumerator CloseSpeed()
    {
        speedPanelAnimation.Play("ThemeClose");

        yield return new WaitForSeconds(0.6f);
        info.buttonControl = false;
        speedPanel.SetActive(false);
    }

    private IEnumerator CloseOption()
    {
        optionPanel.GetComponent<Animator>().Play("ThemeClose");

        yield return new WaitForSeconds(0.6f);
        info.buttonControl = false;
        optionPanel.SetActive(false);
    }
    private IEnumerator CloseCal()
    {
        optionCalPanel.GetComponent<Animator>().Play("ThemeClose");

        yield return new WaitForSeconds(0.6f);
        info.buttonControl = false;
        optionCalPanel.SetActive(false);
        optionSelectPanel.SetActive(true);
    }
    private IEnumerator CloseVol()
    {
        optionVolPanel.GetComponent<Animator>().Play("ThemeClose");

        yield return new WaitForSeconds(0.6f);
        info.buttonControl = false;
        optionVolPanel.SetActive(false);
        optionSelectPanel.SetActive(true);
    }

    public void Enter()
    {
        if (info.buttonControl == false)
        {
            epListSound[info.episodeIndex].GetComponent<EpSoundListControl>().EndMusic(info.musicIndex);
            StartCoroutine(EnterGame());

        }
    }

    private IEnumerator EnterGame()
    {
        //if(!info.debugMode)
        //{
            //이전에 메인음악을 종료시킬때는 노래를 고른후 게임창으로 넘어갈때 음악을 줄였음 --
            //MainMusic.GetInstance.PauseMusic();
            FadePanel.GetInstance.ScreenFade(false, 1.5f);
            yield return new WaitForSeconds(1.5f);
        //}
        info.buttonControl = true;
        SceneManager.LoadScene(3);
    }

    public void ChangeDifficult()
    {
        //테스트 모드
        if (testrock == false)
        {


            if (info.lv == 2)
            {
                info.lv = 0;
            }
            else
            {
                info.lv++;
            }

            switch (info.lv)
            {
                case 0:
                    difficultObj[0].SetActive(true);
                    difficultObj[1].SetActive(false);
                    difficultObj[2].SetActive(false);
                    break;

                case 1:
                    difficultObj[0].SetActive(false);
                    difficultObj[1].SetActive(true);
                    difficultObj[2].SetActive(false);
                    break;

                case 2:
                    difficultObj[0].SetActive(false);
                    difficultObj[1].SetActive(false);
                    difficultObj[2].SetActive(true);
                    break;
            }
            difficultGearParticle.Play();
            difficultTextParticle.Play();
            difficultLabel.text = difficult[info.lv];

        }
    }

    public void CalChanged()
    {
        info.noteSync =  calSlider.value - 0.5f; 
        calText.text = info.noteSync.ToString("N2");
    }
    public void CalibrationDown()
    {
        //int noteSync = Mathf.CeilToInt(info.noteSync * 100);
        //Debug.Log(noteSync);
        //Debug.Log("Test");
        //if (noteSync % 10 == 0)
        //{
        //    noteSync -= 10;
        //}
        //else
        //{
        //    noteSync -= (10 + noteSync % 10);
        //}

        //info.noteSync = noteSync / 100f;

        //info.noteSync = Mathf.Max(-1f, info.noteSync);

        //calSlider.value = info.noteSync + 0.5f; 

        //calText.text = info.noteSync.ToString();
        if(info.noteSync >-1)
        {
            info.noteSync -= 0.01f;
            
        }
        info.noteSync = Mathf.Max(-0.5f, info.noteSync);
        calSlider.value = info.noteSync + 0.5f;
        calText.text = info.noteSync.ToString("N2");
    }
    public void CalibrationUp()
    {
        //int noteSync = Mathf.FloorToInt(info.noteSync * 100);
        //if (noteSync % 10 == 0)
        //{
        //    noteSync += 10;
        //}
        //else
        //{
        //    noteSync += (10 - noteSync % 10);
        //}
        //info.noteSync = noteSync / 100f;


        //info.noteSync = Mathf.Min(1, info.noteSync);

        //calSlider.value = info.noteSync + 0.5f; ;

        //calText.text = info.noteSync.ToString();

        if (info.noteSync < 1f)
        {
            info.noteSync += 0.01f;
        }
        info.noteSync = Mathf.Min(0.5f, info.noteSync);
        calSlider.value = info.noteSync + 0.5f;
        calText.text = info.noteSync.ToString("N2");
    }
   
}