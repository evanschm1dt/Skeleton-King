using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerTileMaster : MonoBehaviour
{

    [HideInInspector]
    public float activeTimer;

    public float inactiveTimerBase;
    float inactiveTimer;

    bool tilesOff;

    bool setOneActive;

    public GameObject[] setOne;
    public GameObject[] setTwo;
    // Start is called before the first frame update
    void Start()
    {
        inactiveTimer = inactiveTimerBase;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTimer > 0 && !tilesOff)
        {
            activeTimer -= Time.deltaTime;
        }

        if (activeTimer <= 0 && !tilesOff)
        {
            Deactivate();
        }

        if (tilesOff)
        {
            if (inactiveTimer > 0)
            {
                inactiveTimer -= Time.deltaTime;
            }

            if (inactiveTimer <= 0)
            {
                Activate();
            }
        }
    }

    void Deactivate()
    {
        inactiveTimer = inactiveTimerBase;
        tilesOff = true;
        if (setOneActive)
        {
            foreach (GameObject tile in setOne)
            {
                tile.GetComponent<dangerTile>().active = false;
            }
        }
        else if (!setOneActive)
        {
            foreach (GameObject tile in setTwo)
            {
                tile.GetComponent<dangerTile>().active = false;
            }
        }
    }

    void Activate()
    {
        tilesOff = false;
        setOneActive = !setOneActive;

        if (setOneActive)
        {
            foreach (GameObject tile in setOne)
            {
                tile.GetComponent<dangerTile>().Activate();
            }
        }
        else if (!setOneActive)
        {
            foreach (GameObject tile in setTwo)
            {
                tile.GetComponent<dangerTile>().Activate();
            }
        }
    }
}
