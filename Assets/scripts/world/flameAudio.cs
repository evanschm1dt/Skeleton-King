using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameAudio : MonoBehaviour
{
    AudioSource audio;

    GameObject listener;

    float listenerDistance;
    float maxVolume;
    public float maxDistance;

    public AudioClip fire;

    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.Find("listener");

        audio = GetComponent<AudioSource>();

        maxVolume = audio.volume;
    }

    // Update is called once per frame
    void Update()
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
    }

    public void StartFire()
    {
        audio.Play();
    }

    public void StopFire()
    {
        audio.Stop();
    }
}
