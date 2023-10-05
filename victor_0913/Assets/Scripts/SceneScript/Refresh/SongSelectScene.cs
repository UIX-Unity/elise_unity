using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectScene : Singleton<SongSelectScene>
{
    public Animator ani;
    public Animator sub;

    static int theme;

    public void SetTheme(int type)
    {
        theme = type;
    }

    public int GetTheme()
    {
        return theme;
    }

    public void SongSelectSceneMove()
    {
        if(!ani.GetCurrentAnimatorStateInfo(0).IsName("Hide Panel"))
        {
            ani.Play("Hide Panel");
            StartCoroutine(OpenSelectScene());
        }
    }

    private IEnumerator OpenSelectScene()
    {
        while(true)
        {
            yield return null;

            if(ani.GetCurrentAnimatorStateInfo(0).IsName("HideEnd"))
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
