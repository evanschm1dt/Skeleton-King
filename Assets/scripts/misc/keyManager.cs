using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class keyManager : MonoBehaviour {

	public GameObject door;

    public GameObject keyCanvas;

	public int keysToFind;
    [HideInInspector]
    public int keysFound;

    int keysTotal;

	bool doorDestroyed = false;

    BoxCollider2D doorZone;

    public Text keyText;

    //Animator doorAnim;

	// Use this for initialization
	void Start () {
        //doorAnim = door.GetComponent<Animator>();
        doorZone = gameObject.GetComponent<BoxCollider2D>();
        keysTotal = keysToFind;

        RewriteText();

        keyCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (keysToFind);

		

		/*if (Input.GetKeyDown (KeyCode.O)) {
			keysToFind -= 1;
		}*/
	}

    public void RewriteText()
    {
        keyText.text = keysFound + "/" + keysTotal;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "necromancer")
        {
            keyCanvas.SetActive(true);
            RewriteText();
            if (keysToFind <= 0)
            {
                if (!doorDestroyed)
                {
                    //door.GetComponentInChildren<doorPart>().CreateReplacement();

                    foreach (Transform child in door.transform)
                    {
                        child.GetComponent<doorPart>().CreateReplacement();
                    }

                    Destroy(door);
                    AstarPath.active.Scan();
                    doorDestroyed = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "necromancer")
        {
            keyCanvas.SetActive(false);
        }
    }
}
