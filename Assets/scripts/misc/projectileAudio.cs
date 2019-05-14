using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileAudio : MonoBehaviour
{
    AudioSource audio;

    GameObject listener;

    float listenerDistance;
    float maxVolume;
    public float maxDistance;

    public AudioClip[] startSounds;

    public GameObject endSound;

    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.Find("listener");

        audio = GetComponent<AudioSource>();

        maxVolume = audio.volume;

        PlayStartSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayStartSound()
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

            audio.PlayOneShot(startSounds[Random.Range(0, startSounds.Length)]);
        
    }

    public void SpawnEndSound()
    {
        Instantiate(endSound, transform.position, Quaternion.identity);
    }
    
}
