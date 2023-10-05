using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : Singleton<MainMusic>
{
    public AudioSource audio;
    public float sound;

    GameSetting info;
    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;
    }

    private void Start()
    {
        sound = info.sound;
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic()
    {
        if(!audio.isPlaying)
        {
            StartCoroutine(Play());
        }
    }

    private IEnumerator Play()
    {
        audio.volume = 0f;
        audio.Play();

        float timer = 0f;

        while(timer <= 2f)
        {
            timer += Time.deltaTime;

            audio.volume = Mathf.Lerp(0f, sound, timer / 2f);
            yield return null;
        }
    }

    public void PauseMusic()
    {
        if(audio.isPlaying)
        {
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        audio.volume = sound;

        float timer = 0f;

        while(timer <= 0.6f)
        {
            timer += Time.deltaTime;

            audio.volume = Mathf.Lerp(sound, 0f, timer / 0.6f);
            yield return null;
        }
        audio.Pause();
    }
}