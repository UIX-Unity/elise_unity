using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColibrationMove : MonoBehaviour
{
    GameSetting info;

    public float mSpeed;
    public float pSpeed;
    public float speed;

    public AudioSource audio;

    public ParticleSystem particle;
    //public GameObject eff;

    public int audioch;
    public int particleCh;
    // Start is called before the first frame update
    void Start()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        audioch = 2;
        particleCh = 2;
        mSpeed = -speed;
        pSpeed = speed;
        particle.Clear();
        audio.GetComponent<AudioSource>().volume = info.sound;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        if(transform.rotation.z < -info.noteSync && audioch == 0)
        {
            audio.Play();
            audioch = 2;
        }
        else if (transform.rotation.z > info.noteSync && audioch == 1)
        {
            audio.Play();
            audioch = 2;
        }

        if (transform.rotation.z < 0.0f && particleCh == 0)
        {
            particle.Play();
            particleCh = 2;
        }
        else if (transform.rotation.z > 0.0f && particleCh == 1)
        {
            particle.Play();
            particleCh = 2;
        }

        if (transform.rotation.z > 0.5f)
        {
            audioch = 0;
            particleCh = 0;
            audio.Stop();
            particle.Clear();
            speed = mSpeed;
        }
        else if(transform.rotation.z < -0.5f )
        {
            audioch = 1;
            particleCh = 1; 
            audio.Stop();
            particle.Clear();
            speed = pSpeed;
        }
    }
}
