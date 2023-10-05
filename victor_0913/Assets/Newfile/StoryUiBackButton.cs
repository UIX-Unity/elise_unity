using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
public class StoryUiBackButton : MonoBehaviour
{
    GameSetting info;

    private void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
    }
    public void UiStoryBackButton()
    {
        if(info.buttonControl == false)
        {
            info.buttonControl = true;
            StartCoroutine(StartSceneClose());
        }
    }
    IEnumerator StartSceneClose()
    {
        FadePanel.GetInstance.ScreenFade(false, 1);
        yield return new WaitForSeconds(2f);
        info.buttonControl = false;
        SceneManager.LoadScene(2);
    }
}
