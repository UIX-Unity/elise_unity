using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySetting : MonoBehaviour
{
    public GameSetting info;
    public UIKeyBinding[] keySet;
    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
#if UNITY_EDITOR
        for (int i=0;i<keySet.Length;i++)
        {
            keySet[i].keyCode = (KeyCode)info.Key[i];
        }
#endif
    }


}
