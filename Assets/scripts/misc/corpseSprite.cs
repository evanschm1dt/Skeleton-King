using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corpseSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void animEnd()
    {
        gameObject.GetComponentInParent<corpse>().ReanimateComplete();
    }

    public void animEndConsume()
    {
        gameObject.GetComponentInParent<corpse>().ConsumeComplete();
    }

    public void ReleaseParticles()
    {
        gameObject.GetComponentInParent<corpse>().EmitParticles();
    }
}
