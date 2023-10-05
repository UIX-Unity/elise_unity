using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : Singleton<FadePanel>
{
    private Animator ani;
    public bool isFirst = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void ScreenFade(bool isIn, float speed)
    {
        ani.speed = speed;

        if(isIn)
        {
            ani.Play("FadeIn");
        }
        else
        {
            ani.Play("FadeOut");
        }
    }
}