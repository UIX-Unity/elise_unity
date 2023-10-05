using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongListen : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] ScrollInput input;

    private CircularMenu circleMenu;
    private GameSetting info;

    private bool test = false;

    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;

        circleMenu = GetComponent<CircularMenu>();
    }

    public void PlayMusic()
    {
        music.clip = info.selectedEpisode.musics[circleMenu.GetCurrentIndex()].music;
        StopAllCoroutines();
        StartCoroutine(Play());
        test = true;
    }

    public void PauseMusic()
    {
        if(!music.isPlaying || (circleMenu.GetCurrentIndex() != input.indexTemp))
        {
            if (!test)
                return;

            test = false;

            StopAllCoroutines();
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Play()
    {
        float timer = 0f;

        music.time = 40f;
        music.Play();

        while(timer <= 1.2f)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(0, 1, timer / 1.2f);

            yield return null;
        }

        test = true;

        yield return null;
    }

    private IEnumerator Pause()
    { 
        float timer = 0f;

        while (timer <= 2f)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(1, 0, timer / 2f);

            yield return null;
        }

        music.Stop();

        yield return null;
    }
}
