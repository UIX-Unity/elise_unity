using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;



public class StartScene : MonoBehaviour
{
    public bool isDraw = false;

    private void Start()
    {
        StartCoroutine(StartSceneReady());
        MainMusic.GetInstance.PlayMusic();
    }
    // Update is called once per frame
    void Update ()
    {
        if (!isDraw)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartSceneClose());
        }
    }

    IEnumerator StartSceneReady()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator StartSceneClose()
    {
        FadePanel.GetInstance.ScreenFade(false, 1);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}