using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireKnightSpray : MonoBehaviour
{

    public int sprayDamage; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("skeleton"))
        {
            other.GetComponent<undeadHealthManager>().DamageMe(sprayDamage);
        }
        if (other.gameObject.CompareTag("enemy"))
        {
            other.GetComponent<enemyHealthManager>().DamageMe(sprayDamage * 2);
        }
    }
}
