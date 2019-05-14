using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{
    AudioSource audio;

    GameObject listener;

    float listenerDistance;
    float maxVolume;
    public float maxDistance;

    public AudioClip[] footsteps;
    public AudioClip[] attacks;

    public AudioClip raiseDead;

   


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

        //Debug.Log(listenerDistance/maxDistance);

    }


    public void PlayFootstep()
    {
        listenerDistance = Vector2.Distance(transform.position, listener.transform.position);

        if (listenerDistance <= 1)
        {
            audio.volume = maxVolume;
        } else if (listenerDistance > 3)
        {
            audio.volume = maxVolume * ((maxDistance - listenerDistance) / maxDistance);
        }

        if (listenerDistance >= maxDistance)
        {
            audio.volume = 0;
        }

        audio.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }

    public void PlayAttack()
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

        audio.PlayOneShot(attacks[Random.Range(0, attacks.Length)]);
    }

    public void PlayRaiseDead()
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

        audio.PlayOneShot(raiseDead);
    }
}
