using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public UILabel progress;
    public Animator ani;
    
	// Use this for initialization
	void Start ()
    {
        MainMusic.GetInstance.PlayMusic();
        FadePanel.GetInstance.ScreenFade(true, 1);
        StartCoroutine(LoadLobby());
	}
    
    public IEnumerator LoadLobby()
    {
        yield return new WaitForSeconds(1.5f);

        AsyncOperation async = SceneManager.LoadSceneAsync(2);
        async.allowSceneActivation = false;

        float timer = 0f;
        float amount = 0f;

        while(!async.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if(async.progress >= 0.9f)
            {
                amount = Mathf.Lerp(amount, 1f, timer);
                progress.text = string.Format("{0:##}%", amount * 100);
                if (amount == 1.0f)
                {
                    Debug.Log("AG");
                    timer = 1.3f;
                    break;
                }
            }
            else
            {
                amount = Mathf.Lerp(amount, async.progress, timer);
                progress.text = string.Format("{0:##}%", amount * 100);
                if(amount >= async.progress)
                {
                    timer = 0f;
                }
            }
        }

        FadePanel.GetInstance.ScreenFade(false, 1);

        yield return new WaitForSeconds(1.5f);

        async.allowSceneActivation = true;
    }
}