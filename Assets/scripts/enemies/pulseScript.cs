using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulseScript : MonoBehaviour
{
    [Range(0, 10)]
    public float growthSpeed;

    public int damage;
    // Start is called before the first frame update

    [Range(0, 5)]
    public float lifeTimer;

    CircleCollider2D myCollider;

    void Start() {
        myCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        gameObject.transform.localScale += new Vector3(1, 1, 1) * growthSpeed * Time.deltaTime;
        //myCollider.radius += growthSpeed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("skeleton"))
        {
            col.gameObject.GetComponent<undeadHealthManager>().unitHealth -= damage;

        }

    }
}
