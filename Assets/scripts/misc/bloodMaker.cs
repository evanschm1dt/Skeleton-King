using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodMaker : MonoBehaviour
{

    ParticleSystem particles;
    public int particleNumber;

    // Start is called before the first frame update
    void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToCorpse(Vector3 corpsePos)
    {
        transform.position = corpsePos;

        particles.Emit(particleNumber);
    }
}
