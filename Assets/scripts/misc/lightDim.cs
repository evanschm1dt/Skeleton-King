using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightDim : MonoBehaviour
{
    Light myLight;

    public float lightSpeed;
    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        myLight.intensity = Mathf.Lerp(myLight.intensity, 0, lightSpeed * Time.deltaTime);
    }
}
