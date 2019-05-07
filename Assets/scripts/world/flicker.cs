using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flicker : MonoBehaviour
{

    Light myLight;

    float lightMax;
    public float lightMin;

    bool lightDown = true;

    float flickerSpeed;
    public float minFlickerSpeed;
    public float maxFlickerSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light>();
        lightMax = myLight.intensity;

        flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(lightDown);

        if (lightDown)
        {
            myLight.intensity = Mathf.Lerp(myLight.intensity, lightMin, flickerSpeed * Time.deltaTime);

            if (myLight.intensity <= lightMin + .1f)
            {
                flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
                lightDown = false;
            }
        }

        if (!lightDown)
        {
            myLight.intensity = Mathf.Lerp(myLight.intensity, lightMax, flickerSpeed * Time.deltaTime);

            if (myLight.intensity >= lightMax - .1f)
            {
                flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
                lightDown = true;
            }
        }
    }
}
