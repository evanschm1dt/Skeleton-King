using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public bool active;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("aaaaaaaaaaaaaaaa");
        if (other.name == "necromancer")
        {
            active = true;
        }
    }
}
