using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireKnightAudio : MonoBehaviour
{
    AudioSource audio;

    public AudioClip[] burstClips;
    public AudioClip[] sprayClips;

    public AudioClip awaken;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WakeUp()
    {
        audio.PlayOneShot(awaken);
    }

    public void BurstSound()
    {
        audio.PlayOneShot(burstClips[Random.Range(0, burstClips.Length)]);
    }

    public void SpraySound()
    {
        audio.PlayOneShot(sprayClips[Random.Range(0, sprayClips.Length)]);
    }
}
