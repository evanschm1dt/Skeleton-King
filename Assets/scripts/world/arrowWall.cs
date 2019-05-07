using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowWall : MonoBehaviour
{

    [HideInInspector]
    public bool active;

    public float arrowTimerBase;
    float arrowTimer;

    public GameObject arrow;

    public bool tripleArrow;

    // Start is called before the first frame update
    void Start()
    {
        arrowTimer = arrowTimerBase;
    }

    // Update is called once per frame
    void Update()
    {
     //   active = false;

        if (arrowTimer > 0)
        {
            arrowTimer -= Time.deltaTime;
        }

        if (arrowTimer <= 0 && active)
        {
            if (!tripleArrow) {
                GameObject myArrow = Instantiate(arrow, new Vector3(transform.position.x, transform.position.y, (transform.position.z + 1)), gameObject.transform.rotation);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), myArrow.GetComponent<Collider2D>());
            } else if (tripleArrow)
            {
                GameObject myArrow1 = Instantiate(arrow, new Vector3(transform.position.x - .25f, transform.position.y, (transform.position.z + 1)), gameObject.transform.rotation);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), myArrow1.GetComponent<Collider2D>());

                GameObject myArrow2 = Instantiate(arrow, new Vector3(transform.position.x, transform.position.y, (transform.position.z + 1)), gameObject.transform.rotation);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), myArrow2.GetComponent<Collider2D>());

                GameObject myArrow3 = Instantiate(arrow, new Vector3(transform.position.x + .25f, transform.position.y, (transform.position.z + 1)), gameObject.transform.rotation);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), myArrow3.GetComponent<Collider2D>());
            }
            
            arrowTimer = arrowTimerBase;
        }
    }
}
