using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textScene : MonoBehaviour
{

    public GameObject text2;
    public GameObject text3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            text2.SetActive(!text2.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            text3.SetActive(!text3.activeInHierarchy);
        }
    }
}
