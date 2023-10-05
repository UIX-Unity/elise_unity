using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    private GameSetting info;

    [SerializeField] UISprite bgImg;
    [SerializeField] UnityEngine.GameObject[] difImg;
    [SerializeField] UILabel difLabel;
    [SerializeField] UILabel songTitle;

    [SerializeField] BMSPlayer player;


    public bool buttonCheck = false;
    private void Awake()
    {

        info = Resources.Load("GameSetting") as GameSetting;
        bgImg.atlas = info.selectedEpisode.episodeAtlas;
        bgImg.spriteName = info.selectedEpisode.musics[info.musicIndex].backgroundPath;
        songTitle.text = info.selectedEpisode.musics[info.musicIndex].name;
        switch (info.lv)
        {
            case 0:
                difImg[0].SetActive(true);
                difImg[1].SetActive(false);
                difImg[2].SetActive(false);
                difLabel.text = "Easy";
                break;

            case 1:
                difImg[0].SetActive(false);
                difImg[1].SetActive(true);
                difImg[2].SetActive(false);
                difLabel.text = "Normal";
                break;

            case 2:
                difImg[0].SetActive(false);
                difImg[1].SetActive(false);
                difImg[2].SetActive(true);
                difLabel.text = "Hard";
                break;
        }
        //if(!info.debugMode)
        //{
           
        //}
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        FadePanel.GetInstance.ScreenFade(true, 2f);
        yield return new WaitForSeconds(2f);
        player.isStart = true;
    }

    public void RetryGame()
    {
        if(buttonCheck == true)
        {
            StartCoroutine(Retry());
        }
    }

    private IEnumerator Retry()
    {
        //if(!info.debugMode)
        //{
            FadePanel.GetInstance.ScreenFade(false, 1.5f);
            yield return new WaitForSeconds(1.5f);
        //}
        SceneManager.LoadScene(3);
    }

    public void LobbyScene()
    {
        if(buttonCheck == true)
        {
            StartCoroutine(Lobby());
        }
    }

    private IEnumerator Lobby()
    {
        //if(!info.debugMode)
        //{
            FadePanel.GetInstance.ScreenFade(false, 1.5f);
            yield return new WaitForSeconds(1.5f);
        //}
        MainMusic.GetInstance.PlayMusic();
        SceneManager.LoadScene(2);
    }
}