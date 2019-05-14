using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameWall : MonoBehaviour
{

    public bool active;

    bool firing;

    public float onTimeBase;
    public float offTimeBase;

    float onTime;
    float offTime;
    public float timeStart;

    ParticleSystem particles;

    public int damage;

    public Light myLight;

    public bool lightControl;

    float lightIntensity;

    public float lightSpeed;

    bool audioMaker;



    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<flameAudio>())
        {
            audioMaker = true;
        }else
        {
            audioMaker = false;
        }

        if (!active)
        {
            firing = false;
        }

        if (lightControl)
        {
            lightIntensity = myLight.intensity;
            if (!firing || !active)
            {
                myLight.intensity = 0;
            }
        }

        onTime = onTimeBase;
        offTime = timeStart;

        particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            if (!firing)
            {
                if (lightControl)
                {
                    myLight.intensity = Mathf.Lerp(myLight.intensity, 0, lightSpeed * Time.deltaTime);
                }
                offTime -= Time.deltaTime;
                if (offTime <= 0)
                {
                    offTime = offTimeBase;
                    firing = true;
                    particles.Play();
                    if (audioMaker)
                    {
                        gameObject.GetComponent<flameAudio>().StartFire();
                    }
                }
            }

            if (firing)
            {
                if (lightControl)
                {
                    //myLight.intensity = lightIntensity;
                    myLight.intensity = Mathf.Lerp(myLight.intensity, lightIntensity, lightSpeed * Time.deltaTime);
                }

                onTime -= Time.deltaTime;
                if (onTime <= 0)
                {
                    onTime = onTimeBase;
                    firing = false;
                    particles.Stop();
                    if (audioMaker)
                    {
                        gameObject.GetComponent<flameAudio>().StopFire();
                    }

                }
            }
        }
        else
        {
            if (firing)
            {
                firing = false;
                particles.Stop();
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("skeleton"))
        {
            other.GetComponent<undeadHealthManager>().DamageMe(damage);
        }

        if (other.gameObject.CompareTag("enemy"))
        {
            if (other.GetComponent<enemyHealthManager>().flameResistant == false)
            {
                other.GetComponent<enemyHealthManager>().DamageMe(damage);
            }
        }

    }
}
