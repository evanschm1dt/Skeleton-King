using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallEdgeGen : MonoBehaviour
{

    public GameObject wallEdge;

    new Vector3 createPos;
    // Start is called before the first frame update
    void Start()
    {
        createPos = transform.position;
        createPos.z = 0.8f;
        var myEdge = Instantiate(wallEdge, createPos, Quaternion.identity);
        myEdge.transform.parent = gameObject.transform;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
