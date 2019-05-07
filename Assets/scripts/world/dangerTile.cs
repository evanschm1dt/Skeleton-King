using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerTile : MonoBehaviour
{

    public float onTimerBase;
    float onTimer;

   // public float offTimerBase;
  //  float offTimer;

    public bool active;

    private Animator animator;

    public int damage;

    public GameObject papa;

    // Start is called before the first frame update
    void Start()
    {
        onTimer = onTimerBase;
      //  offTimer = offTimerBase;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animate();

       /* if (!active)
        {
            if (offTimer > 0)
            {
                offTimer -= Time.deltaTime;
                if (offTimer <= 0)
                {
                    offTimer = offTimerBase;
                    active = true;

                }
            }
        }*/

        if (active)
        {
            if (onTimer > 0)
            {
                onTimer -= Time.deltaTime;
                if (onTimer <= 0)
                {
                    onTimer = onTimerBase;
                    active = false;

                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("undeadFeet"))
        {
            if (active)
            {
                other.GetComponentInParent<undeadHealthManager>().DamageMe(damage);
            }
        }
    }

    private void Animate()
    {
        if (active)
        {
            animator.SetBool("active", true);
        } else if (!active)
        {
            animator.SetBool("active", false);
        }
    }

    public void Activate()
    {
        active = true;
        papa.GetComponent<dangerTileMaster>().activeTimer = onTimerBase;
    }
}
