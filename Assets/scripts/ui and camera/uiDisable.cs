using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiDisable : MonoBehaviour
{

    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Canvas.SetActive(!Canvas.activeInHierarchy);
        }
    }
}
