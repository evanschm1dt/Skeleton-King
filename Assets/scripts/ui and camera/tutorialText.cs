using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tutorialText : MonoBehaviour
{

    public GameObject[] texts;

    public GameObject[] textColliders;

    public GameObject mainDoor;

    int textIndex;

    GameObject manager;

    float textAppearTimer;

    float textVanishTimer;
    public float textVanishTimerBase;

    bool usingTextTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        textAppearTimer = 0;

        textVanishTimer = textVanishTimerBase;

        manager = GameObject.Find("manager");
    }

    // Update is called once per frame
    void Update()
    {

        if (textAppearTimer > 0)
        {
            textAppearTimer -= Time.deltaTime;
        }

        if (textAppearTimer <= 0 && usingTextTimer)
        {
            texts[textIndex].SetActive(true);
        }



        if (textIndex == 0)
        {


            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                texts[0].SetActive(false);
                textAppearTimer = 4.25f;
                textIndex = 1;
            }
        }

        if (textIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                texts[1].SetActive(false);
                textAppearTimer = 1.25f;
                textIndex = 2;
            }

        }

        if (textIndex == 2)
        {
            if (manager.GetComponent<controller>().selectableObjects.Count > 0)
            {
                texts[2].SetActive(false);
                textAppearTimer = 1.25f;
                textIndex = 3;
            }
        }

        if (textIndex == 3)
        {
            if (manager.GetComponent<controller>().selectedSkeletons.Count > 0)
            {
                texts[3].SetActive(false);
                textAppearTimer = 1f;
                textIndex = 4;
            }
        }

        if (textIndex == 4)
        {
            if (manager.GetComponent<controller>().selectedSkeletons.Count > 0 && Input.GetMouseButtonDown(1))
            {
                texts[4].SetActive(false);
                //textAppearTimer = 1f;
                usingTextTimer = false;
                textIndex = 5;
            }
        }

        if (textIndex == 5)
        {
            if (textColliders[0].GetComponent<tutorialCollider>().active)
            {
                texts[5].SetActive(true);
                textVanishTimer -= Time.deltaTime;

                if (textVanishTimer < 0)
                {
                    if (manager.GetComponent<controller>().selectedSkeletons.Count > 0 && Input.GetMouseButtonDown(1))
                    {
                        texts[5].SetActive(false);
                        textVanishTimer = textVanishTimerBase;
                        textIndex = 6;
                    }
                }
                
            }
        }

        

        if (textIndex == 6)
        {
            if (textColliders[1].GetComponent<tutorialCollider>().active)
            {
                texts[6].SetActive(true);
                
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    texts[6].SetActive(false);
                    textVanishTimer = textVanishTimerBase;
                    textIndex = 7;
                }
                

            }
        }

        if (textIndex == 7)
        {
            
                texts[7].SetActive(true);
                textVanishTimer -= Time.deltaTime;

                if (textVanishTimer < 0)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        texts[7].SetActive(false);
                        textVanishTimer = textVanishTimerBase;
                        textIndex = 8;
                    }
                }
        }

        if (textIndex == 8)
        {
            if (textColliders[2].GetComponent<tutorialCollider>().active)
            {
                texts[8].SetActive(true);
                //textVanishTimer -= Time.deltaTime;

                // if (textVanishTimer < 0)
                //{
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    texts[8].SetActive(false);
                    textVanishTimer = 5;
                    textIndex = 9;
                }
                //}

            }
        }

        if (textIndex == 9)
        {
            if (mainDoor != null && mainDoor.GetComponent<keyManager>().keysToFind < 1)
            {
                texts[9].SetActive(true);
                textVanishTimer -= Time.deltaTime;

                if (textVanishTimer <= 0)
                {
                    texts[9].SetActive(false);
                }
            }
        }
    }
}
