using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCode : MonoBehaviour
{
    GameSetting info;
    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
    }

    public void AnimationStart()
    {
        info.buttonControl = true;
    }
    public void AnimationEnd()
    {
        info.buttonControl = false;
    }
}
