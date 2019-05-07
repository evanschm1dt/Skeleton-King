﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorPart : MonoBehaviour
{
    public GameObject myReplacement;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateReplacement()
    {
       GameObject replacement = Instantiate(myReplacement, transform.position, transform.rotation);
        replacement.transform.localScale = gameObject.transform.localScale;
    }
}