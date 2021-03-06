﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class kingGem : MonoBehaviour
{
    AudioSource audio;
    
    public AudioClip buildup;
    public AudioClip awaken;
    bool playedBuildup;

    public GameObject necromancer;

    public GameObject camera;

    private Animator animator;

    public GameObject explosion;

    public Light gemLight;
    float lightIntensity;

    bool dormant = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        lightIntensity = gemLight.intensity;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dormant)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                animator.SetTrigger("awaken");
                audio.PlayOneShot(awaken);
                dormant = false;
            }
        }
    }

    public void IntensityUpDormant()
    {
        gemLight.intensity = lightIntensity + .25f;
    }

    public void IntensityDownDormant()
    {
        gemLight.intensity = lightIntensity;
    }

    public void IntensityUpActive()
    {
        gemLight.intensity = lightIntensity + 1f;
    }

    public void IntensityDownActive()
    {
        gemLight.intensity = lightIntensity;
    }

    public void PlayBuildup()
    {
        if (!playedBuildup)
        {
            audio.PlayOneShot(buildup);
            playedBuildup = true;
        }
    }

    public void GemExplode()
    {
        audio.Stop();
        GameObject myExplosion = Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        Destroy(myExplosion, 5f);
        gemLight.intensity = lightIntensity - 2.5f;
        gemLight.color = Color.white;
        camera.GetComponent<cameraController>().necromancer = necromancer;
        camera.GetComponent<cameraController>().noNecromancer = false;
        necromancer.GetComponent<necromancerMain>().direction = new Vector2(0, -1);
        necromancer.GetComponent<necromancerMain>().animator.SetTrigger("faceDown");
        //necromancer.GetComponent<necromancerMain>().AnimateMovement(necromancer.GetComponent<necromancerMain>().direction);
        Debug.Log("teleport");
        necromancer.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, .09f);
    }
}
