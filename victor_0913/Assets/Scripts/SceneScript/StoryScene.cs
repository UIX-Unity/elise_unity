using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using PlayFab;
using PlayFab.ClientModels;
public class StoryScene : MonoBehaviour
{
    GameSetting info;
    private void Start()
    {
        info = Resources.Load("GameSetting")as GameSetting;
        MainMusic.GetInstance.PlayMusic();
        FadePanel.GetInstance.ScreenFade(true, 1f);
    }
    public void EnterLobby()
    {
        StartCoroutine(Lobby());
    }

    private IEnumerator Lobby()
    {
        FadePanel.GetInstance.ScreenFade(false, 1f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
