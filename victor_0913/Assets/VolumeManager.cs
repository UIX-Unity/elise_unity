using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    GameSetting info;

	// Use this for initialization
	void Start ()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        GetComponent<AudioSource>().volume = info.sound;
	}
}
