using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kingExplode : MonoBehaviour
{

    public bool king;
    // Start is called before the first frame update
    void Start()
    {
        if (!king)
        {
            Destroy(gameObject, 3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        if (king)
        {
            if (other.gameObject.CompareTag("skeleton"))
            {
                other.GetComponent<undeadHealthManager>().unitHealth -= 3;
            }

            if (other.gameObject.CompareTag("enemy"))
            {
                if (other.GetComponent<enemyHealthManager>().boss == false)
                {
                    other.GetComponent<enemyHealthManager>().enemyHealth = 0;
                }
            }

            if (other.gameObject.CompareTag("corpseParticleCollider"))
            {
                other.GetComponentInParent<corpse>().Consume();
            }
        }

    }
}
