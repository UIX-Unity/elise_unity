using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour
{
    public Animator ani;
    public Animator sub;

    public UnityEngine.GameObject panel;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (!panel.activeSelf)
                GameStart();
            else
                StartGame();
        }
	}

    void GameStart()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("ShowEnd"))
        {
            panel.SetActive(true);
            Debug.Log("FUNCTION MOVE");
        }
    }

    void StartGame()
    {
        ani.Play("Hide Panel");

        sub.Play("UGUI Fadeout");
        Debug.Log("FUNCTION MOVE");
        StartCoroutine(Test1());
    }

    private IEnumerator Test1()
    {
        while (true)
        {
            yield return null;

            if(ani.GetCurrentAnimatorStateInfo(0).IsName("HideEnd"))
            {
                SceneManager.LoadScene(3);
            }
        }
    }
}
