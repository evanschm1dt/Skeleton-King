using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pylon : MonoBehaviour
{

    public LayerMask layerMask;

    public float pulseIntervalBase;

    float pulseInterval;

    public GameObject pulse;

    ParticleSystem particles;

    public int particleNumber;

    [HideInInspector]
    public bool active;

    public float sightRange;

    public int damage;

    private Animator animator;

    public Light myLight;

    public float lightHigh;

    float lightLow;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        pulseInterval = pulseIntervalBase;

        particles = GetComponent<ParticleSystem>();

        lightLow = myLight.intensity;
       
    }

    // Update is called once per frame
    void Update()
    {

        Sight();

        Animate();

        if (!active)
        {
            if (pulseInterval >= .25f)
            {
                pulseInterval -= Time.deltaTime;
            }

            if (myLight.intensity != lightLow)
            {
                myLight.intensity = lightLow;
            }
        }

        if (active)
        {

            pulseInterval -= Time.deltaTime;

            if (myLight.intensity != lightHigh)
            {
                myLight.intensity = lightHigh;
            }
        }
        

        if (pulseInterval <= 0 && active)
        {
           // Instantiate(pulse, transform.position, Quaternion.identity);
            pulseInterval = pulseIntervalBase;
            particles.Emit(particleNumber);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("skeleton"))
        {
            other.GetComponent<undeadHealthManager>().DamageMe(damage);
        }
        if (other.gameObject.CompareTag("undeadProjectile"))
        {
            Debug.Log("hi");
            other.GetComponent<playerProjectile>().DestroyWithParticles();
        }

    }

    private void Animate()
    {
        if (active)
        {
            animator.SetBool("isActive", true);
        } else if (!active)
        {
            animator.SetBool("isActive", false);
        }
    }

    private void Sight()
    {

        active = false;

        int RaysToShoot = 360;

        float angle = 0;

        GameObject newTarget = null;
        float nearestDistance = 1000000000000f;
        for (int i = 0; i < RaysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            Vector2 dir = new Vector2(x, y);
            RaycastHit2D hit;
            //Debug.DrawRay (transform.position, (Vector2)dir, Color.red);
            hit = Physics2D.Raycast((Vector2)transform.position, dir, sightRange, layerMask);
            //if (Physics.Raycast (transform.position, dir, out hit, 3f, layerMask)) {
            if (hit.collider != null)
            {

                if (hit.collider.gameObject.CompareTag("skeleton"))
                {
                    if (!active)
                    {
                        active = true;
                    }

                    if (hit.distance < nearestDistance)
                    {
                        nearestDistance = hit.distance;
                        newTarget = hit.collider.gameObject;
                    }
                }
            }
            //}
        }

    }
}
