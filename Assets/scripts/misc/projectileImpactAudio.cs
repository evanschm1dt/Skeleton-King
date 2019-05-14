using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileImpactAudio : MonoBehaviour
{
    AudioSource audio;

    GameObject listener;

    float listenerDistance;
    float maxVolume;
    public float maxDistance;

    public AudioClip[] endSounds;
    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.Find("listener");

        audio = GetComponent<AudioSource>();

        maxVolume = audio.volume;

        PlayEndSound();

        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEndSound()
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

        audio.PlayOneShot(endSounds[Random.Range(0, endSounds.Length)]);

    }
}
