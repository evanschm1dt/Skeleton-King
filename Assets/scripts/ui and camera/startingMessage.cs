using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startingMessage : MonoBehaviour
{

    bool counting;

    public float countdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            counting = true;
        }

        if (counting)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            Destroy(gameObject);
        }
    }
}
