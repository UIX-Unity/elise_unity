using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Music : MonoBehaviour
{
    public AudioSource audioSource;
    AudioClip audioClip;

    public int Min { get; private set; }
    public int Sec { get; private set; }

    public float Bpm;
    public int Frequency;
    public float Offset;

    // 박자표
    public int TimeSignatures_numerator { get; set; } = 4;
    public int TimeSignatures_denominator { get; set; } = 4;

    // 1 마디
    public float BarPerSec { get; private set; }
    public int BarPerTimeSample { get; private set; }
    // 1 박자
    public float BeatPerSec { get; private set; }
    public int BeatPerTimeSample { get; private set; }
    // 32 비트
    public float BeatPerSec32rd { get; private set; }
    public int BeatPerTimeSample32rd { get; private set; }
    public float WholeTime { get; private set; }
    public float WholeTimeSample { get; private set; }
    public float timeRatio => (audioSource.time / WholeTime);
    public float timeRatioSample => (audioSource.time / WholeTime);
    public int musicTimeSample => audioSource.timeSamples;
    public int clipSample => audioSource.clip.samples;
    public int clipChannels => audioSource.clip.channels;


    // 클립 길이에 딱 맞게 자르면 오류가 발생하여 끄트머리 조금 싹뚝
    private float clipOffset = 0.0001f;
    public bool IsAudioPlay { get { return audioSource.isPlaying; } }

    public void SetMusic(AudioClip audioClip)
    {
        this.audioClip = audioClip;
        audioSource.clip = this.audioClip;
    }

    public void Init(float bpm, float offset)
    {
        Play();
        Pause();

        Bpm = bpm;
        Offset = offset;

        SetMusicLength();

        BarPerSec = 240f / Bpm; // 4/4기준 = 60*4, 3/4 = 60*3 추후 각 박자표에 대해 정의
        BarPerTimeSample = (int)(BarPerSec * Frequency);

        BeatPerSec = 60f / Bpm;
        BeatPerTimeSample = (int)(BeatPerSec * Frequency);

        BeatPerSec32rd = BeatPerSec / 8f;
        BeatPerTimeSample32rd = (int)(BeatPerSec32rd * Frequency);

        Debug.Log("1마디 : " + BarPerSec);
        Debug.Log("32비트: " + BeatPerSec32rd);
        Debug.Log("오프셋 : " + Offset);
    }

    public void Play()
    {

        //Debug.Log(audioSource.clip);
        //Debug.Log("현재 타임샘플 포지션 : " + audioSource.timeSamples);
        //Debug.Log("타임샘플 전체 : " + audioClip.samples);
        //Debug.Log("클립 주파수 : " + audioClip.frequency);

        //if (audioClip == null)
        //    Init();

        audioSource.volume = 0.2f;
        audioSource.Play();

        //sheetEditor.isPlay = true;
    }

    public void Stop()
    {
        if (audioClip != null)
        {
            audioSource.timeSamples = 0;
            audioSource.Stop();
        }

        //sheetEditor.isPlay = false;
    }

    public void Pause()
    {
        audioSource.Pause();

        //sheetEditor.isPlay = false;
    }

    public void ChangeTime(float timeValue)
    {
        float currentTime = WholeTime * timeValue;

        currentTime = Mathf.Clamp(currentTime, 0f, WholeTime);

        audioSource.time = currentTime; //Debug.Log("현재 음악 위치 " + audioSource.time);
    }

    public void AddTime(float time)
    {
        float currentTime = audioSource.time;

        currentTime += time;
        currentTime = Mathf.Clamp(currentTime, 0f, WholeTime);

        audioSource.time = currentTime; //Debug.Log("현재 음악 위치 " + audioSource.time);
    }

    void SetMusicLength()
    {
        Frequency = audioClip.frequency;

        WholeTime = audioClip.length - clipOffset;
        WholeTimeSample = WholeTime * Frequency;

        int audioLength = Convert.ToInt32(WholeTime);

        Min = audioLength / 60;
        Sec = audioLength - Min * 60;   
    }
}
