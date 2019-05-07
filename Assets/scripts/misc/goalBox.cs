using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goalBox : MonoBehaviour
{

    [HideInInspector]
    public GameObject king;

    GameObject manager;

    float endTimer;

    bool ending;

    public System.String levelName;
    // Start is called before the first frame update
    void Start()
    {
        king = GameObject.Find("necromancer");

        manager = GameObject.Find("manager");

        endTimer = manager.GetComponent<controller>().fadeOutTime + 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ending)
        {
            endTimer -= Time.deltaTime;
            if (endTimer <= 0)
            {
                SceneManager.LoadScene(levelName);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == king)
        {
            //SceneManager.LoadScene(levelName);
            manager.GetComponent<controller>().fadeOut = true;
            ending = true;
        }
    }
}
