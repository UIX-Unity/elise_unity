using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpSoundListControl : MonoBehaviour
{
    private GameSetting info;

    [SerializeField] public UnityEngine.GameObject[] songListSound;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i< songListSound.Length;i++)
        {
            songListSound[i].GetComponent<AudioSource>().time += 2.2f;
        }
        info = Resources.Load("GameSetting") as GameSetting;
    }
    void OnEnable()
    {
        for (int i = 0; i < songListSound.Length; i++)
        {
            songListSound[i].SetActive(false);
        }
    }

    public void PlayMusic(int index)
    {
        //Debug.Log("TEST1");
       // if (!songListSound[index].GetComponent<AudioSource>().isPlaying)
        //{
            StartCoroutine(Play(index));
       
        //}
    }

    private IEnumerator Play(int index)
    {
        songListSound[index].GetComponent<AudioSource>().Play();
        songListSound[index].GetComponent<AudioSource>().volume = 0f;

        float timer = 0f;

        while (timer <= 2f)
        {
            timer += Time.deltaTime;

            songListSound[index].GetComponent<AudioSource>().volume = Mathf.Lerp(0f, info.sound, timer / 2f);
            yield return null;
        }
    }

    public void StopMusic(int index)
    {
        songListSound[index].SetActive(false);
        //if (songListSound[index].GetComponent<AudioSource>().isPlaying)
        //{
        //    StartCoroutine(Stop(index));
        //}
    }

    private IEnumerator Stop(int index)
    {
        songListSound[index].GetComponent<AudioSource>().volume = info.sound;

        

        float timer = 0f;

        while (timer <= 0.5f)
        {
            timer += Time.deltaTime;
            songListSound[index].GetComponent<AudioSource>().volume = Mathf.Lerp(info.sound, 0f, timer / 0.5f);
            yield return null;
        }
        //songListSound[index].GetComponent<AudioSource>().Pause();
        songListSound[index].GetComponent<AudioSource>().Stop();
    }

    public void EndMusic(int index)
    {
        songListSound[index].SetActive(false);
        //if (songListSound[index].GetComponent<AudioSource>().isPlaying)
        //{
        //    StartCoroutine(EndPause(index));
        //}
    }

    private IEnumerator EndPause(int index)
    {
        songListSound[index].GetComponent<AudioSource>().volume = info.sound;



        float timer = 0f;

        while (timer <= 0.5f)
        {
            timer += Time.deltaTime;
            songListSound[index].GetComponent<AudioSource>().volume = Mathf.Lerp(info.sound, 0f, timer / 0.5f);
            yield return null;
        }
        //songListSound[index].GetComponent<AudioSource>().Pause();
        songListSound[index].GetComponent<AudioSource>().Stop();
        gameObject.SetActive(false);
    }

}
