using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewGame;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// This class controls all the NewGame Scene (ex: the UI data initiation , handle endgame result , handle the music ,...) 
/// We can see this type of manager each scene
/// </summary>
public enum GameState { Playing, Pause, Result }
public class NewGameScene : Singleton<NewGameScene>
{
    [SerializeField] new AudioSource audio;
    [SerializeField] Parser parser;
    [SerializeField] Animator animator;

    [Header("Pause Panel")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] UISprite pauseFill;

    [Header("Game UI")]
    [SerializeField] UILabel progressLabel;
    [SerializeField] UILabel comboLabel;
    [SerializeField] UILabel difficultyLabel;
    [SerializeField] UILabel requireComboLabel;
    [SerializeField] UILabel artistLabel;
    [SerializeField] UILabel songNameLabel;
    [SerializeField] UISprite backgroundSprite;

    [Header("Result Panel")]
    [SerializeField] UILabel resultArtistLabel;
    [SerializeField] UILabel resultSongNameLabel;
    [SerializeField] UILabel resultComboLabel;
    [SerializeField] UILabel progressCount;
    [SerializeField] UILabel progressPercent;
    [SerializeField] UILabel progressCompare;
    [SerializeField] UILabel resultDifficultyLabel;
    [SerializeField] UILabel resultRequireComboLabel;

    [SerializeField] int combo;
    private GameSetting info;
    private Music selectedMusic;
    private GameState gameState = GameState.Pause;
    private int hitCount;
    private bool pauseHold;
    private float currentTimer;

    public GameState GameState => gameState;
    public int FullCount { get; set; }
    public int CurrentCombo { get => combo; set => combo = value; }
    public int CurrentHitCount { get => hitCount; set => hitCount = value; }
    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        Application.targetFrameRate = 60;
        //MainMusic.GetInstance.PauseMusic();

        SetUI();
    }

    private void SetUI()
    {
        selectedMusic = info.selectedMusic;
        audio.clip = selectedMusic.music;
        songNameLabel.text = selectedMusic.composer;
        artistLabel.text = selectedMusic.name;

        float maxMusicScore = info.selectedMusic.maxMusicScore[info.lv];
        requireComboLabel.text = maxMusicScore.ToString();
        
        comboLabel.text = "0";
        progressLabel.text = "0.00%";
        SetDifficultyLevel(info.lv);

    }

    private IEnumerator Start()
    {
        FadePanel.GetInstance.ScreenFade(true, 2f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartMusic());
        animator.Play("UIFadeIn");
    }

    private void Update()
    {
        //Debug.Log("" + audio.time + "/" + " " + audio.clip.length);
        ShowResultPanel();
        PauseBtnHold();
    }

    private void ShowResultPanel()
    {
        if (audio.time >= audio.clip.length && gameState != GameState.Result) // FullCount == parser.GetFullCount()
        {
            gameState = GameState.Result;
            ShowResult();
        }
    }

    private void PauseBtnHold()
    {
        if (pauseHold)
        {
            if (currentTimer <= 1f)
            {
                currentTimer += Time.deltaTime;
                pauseFill.fillAmount = Mathf.Lerp(0, 1f, currentTimer / 1f);
            }
            else
            {
                Debug.Log("Open Pause Panel!");
                gameState = GameState.Result;
                pausePanel.gameObject.SetActive(true);
                audio.Pause();
                OnPauseRelease();
            }
        }
    }

    private IEnumerator StartMusic()
    {
        audio.volume = 0f;
        audio.Play();
        gameState = GameState.Playing;
        float sound = info.sound;
        float timer = 0f;
        while (timer <= 2f)
        {
            timer += Time.deltaTime;

            audio.volume = Mathf.Lerp(0f, sound, timer / 2f);
            yield return null;
        }

    }

    private void ShowResult()
    {
        StartCoroutine(Result());
    }

    private IEnumerator Result()
    {
        yield return new WaitForSeconds(2f);
        audio.Stop();
        audio.time = 0f;
        animator.Play("UIFadeOut");
        yield return new WaitForSeconds(2f);
        animator.Play("ResultOpen");
        resultArtistLabel.text = artistLabel.text;
        resultSongNameLabel.text = songNameLabel.text;
        resultComboLabel.text = comboLabel.text;
        progressCount.text = $"{hitCount} / {parser.GetFullCount()}";
        progressPercent.text = progressLabel.text;
        resultDifficultyLabel.text = difficultyLabel.text;
        resultRequireComboLabel.text = requireComboLabel.text;
    }

    private void SetDifficultyLevel(int level)
    {
        switch (info.lv)
        {
            case 0:
                difficultyLabel.text = "Easy";
                break;
            case 1:
                difficultyLabel.text = "Normal";
                break;
            case 2:
                difficultyLabel.text = "Hard";
                break;
        }
    }

    public void UpdateAccuracy()
    {
        float progress = (float)hitCount / parser.GetFullCount() * 100f;
        progressLabel.text = progress.ToString("F2") + "%";
    }

    public void UpdateCombo()
    {
        comboLabel.text = CurrentCombo.ToString();
    }
    //Events
    public void OnPauseHold()
    {
        pauseHold = true;
    }

    public void OnDragOut()
    {
        if (pauseHold)
        {
            ReleaseHandler();
        }
    }

    public void OnPauseRelease()
    {
        ReleaseHandler();
    }

    private void ReleaseHandler()
    {
        currentTimer = 0f;
        pauseFill.fillAmount = 0f;
        pauseHold = false;
    }

    public void Unpause()
    {
        gameState = GameState.Playing;
        audio.Play();
        pausePanel.gameObject.SetActive(false);
    }

    public void RetryGame()
    {
        StartCoroutine(Retry());
    }

    private IEnumerator Retry()
    {
        FadePanel.GetInstance.ScreenFade(false, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(6);
    }
    public void NextTune()
    {
        StartCoroutine(Next());
    }
    private IEnumerator Next()
    {
        FadePanel.GetInstance.ScreenFade(false, 1.5f);
        yield return new WaitForSeconds(1.5f);
        int temp = info.musicIndex;
        temp++;
        if (temp > info.selectedEpisode.musics.Length - 1)
        {
            temp = info.selectedEpisode.musics.Length - 1;
        }
        else
        {
            info.musicIndex = temp;
        }
        SceneManager.LoadScene(6);
    }
    public void ReturnLobby()
    {
        StartCoroutine(Lobby());
    }

    private IEnumerator Lobby()
    {
        FadePanel.GetInstance.ScreenFade(false, 1.5f);
        yield return new WaitForSeconds(1.5f);
        MainMusic.GetInstance.PlayMusic();
        SceneManager.LoadScene(2);
    }

    public void UpdateUI()
    {
        UpdateCombo();
        UpdateAccuracy();
    }

}
