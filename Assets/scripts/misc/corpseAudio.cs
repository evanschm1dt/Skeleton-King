using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corpseAudio : MonoBehaviour
{
    AudioSource audio;

    GameObject listener;

    float listenerDistance;
    float maxVolume;
    public float maxDistance;

    public AudioClip[] deathSounds;

    public AudioClip[] purpleFire;
    public AudioClip[] consume;

    public AudioClip resurrectionCue;

    public bool presentAtStart;


    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.Find("listener");

        audio = GetComponent<AudioSource>();

        maxVolume = audio.volume;

        if (!presentAtStart)
        {
            PlayDeathSound();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayDeathSound()
    {
        listenerDistance = Vector2.Distance(transform.position, listener.transform.position);

        if (listenerDistance <= 1)
        {
            audio.volume = maxVolume;
        }
        else if (listenerDistance > 3)
        {
            audio.volume = maxVolume * ((maxDistance - listenerDistance) / maxDistance);
        }

        if (listenerDistance >= maxDistance)
        {
            audio.volume = 0;
        }

        audio.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
    }

    public void PlayResurrectionCue()
    {
        listenerDistance = Vector2.Distance(transform.position, listener.transform.position);

        if (listenerDistance <= 1)
        {
            audio.volume = maxVolume;
        }
        else if (listenerDistance > 3)
        {
            audio.volume = maxVolume * ((maxDistance - listenerDistance) / maxDistance);
        }

        if (listenerDistance >= maxDistance)
        {
            audio.volume = 0;
        }

        //audio.PlayOneShot(resurrectionCue);
        audio.PlayOneShot(purpleFire[Random.Range(0, purpleFire.Length)]);
    }

    public void PlayConsume()
    {
        listenerDistance = Vector2.Distance(transform.position, listener.transform.position);

        if (listenerDistance <= 1)
        {
            audio.volume = maxVolume;
        }
        else if (listenerDistance > 3)
        {
            audio.volume = maxVolume * ((maxDistance - listenerDistance) / maxDistance);
        }

        if (listenerDistance >= maxDistance)
        {
            audio.volume = 0;
        }

        audio.PlayOneShot(consume[Random.Range(0, consume.Length)]);
    }
}
