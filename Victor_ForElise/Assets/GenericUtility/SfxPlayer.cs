using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public string sfxKey { get; private set; }
    public float delay { get; private set; }

    [SerializeField]
    private AudioSource m_AudioSource;
    public AudioSource audioSource => m_AudioSource;

    private System.Func<SfxPlayer, bool> skipCondition;

    public void SetRandomPitch(float minValue, float maxValue)
    {
        audioSource.pitch = Random.Range(minValue, maxValue);
    }

    public void Play(string sfxKey, float delay = 0, System.Func<SfxPlayer, bool> skipCondition = null)
    {
        this.sfxKey = sfxKey;
        this.delay = delay;
        this.skipCondition = skipCondition;

        StopAllCoroutines();

        AudioClip clip;
        ResourceManager.audioResourceStorage.TryGetAudioClip(sfxKey, out clip);
        audioSource.clip = clip;
        if (delay == 0)
        {
            audioSource.Play();
        }
        else
        {
            StartCoroutine(DelayedPlay(delay));
        }


        if(audioSource.clip != null)
        {
            StartCoroutine(DestroyDelay(audioSource.clip.length + 0.2f));
        }
        else
        {
            DestroySelf();
        }
    }

    public IEnumerator DelayedPlay(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.Play();
    }

    private IEnumerator DestroyDelay(float delay = 2f)
    {
        float timer = delay;
        do
        {
            yield return null;

            if (skipCondition != null && skipCondition(this))
            {
                break;
            }

            timer -= Time.deltaTime;
        }
        while (timer > 0);

        DestroySelf();
    }

    public void DestroySelf()
    {
        sfxKey = string.Empty;
        audioSource.clip = null;
        audioSource.pitch = 1f;
        skipCondition = null;

        //SpawnMaster.Destroy(gameObject, "SfxPlayer");
    }
}
