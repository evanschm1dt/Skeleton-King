using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zShifter : MonoBehaviour
{

    float baseZPos;

    Vector3 previousPos;
    // Start is called before the first frame update
    void Start()
    {
        baseZPos = transform.position.z;
        previousPos = transform.position;
        zShift();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousPos != transform.position)
        {
            zShift();
            previousPos = transform.position;
        }
        //zShift();
    }

    void zShift()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, (baseZPos + (transform.position.y / 10000)));
    }
}
