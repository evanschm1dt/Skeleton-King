using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class keyManager : MonoBehaviour {

	public GameObject door;

	public int keysToFind;

	bool doorDestroyed = false;

    BoxCollider2D doorZone;

    //Animator doorAnim;

	// Use this for initialization
	void Start () {
        //doorAnim = door.GetComponent<Animator>();
        doorZone = gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (keysToFind);

		

		/*if (Input.GetKeyDown (KeyCode.O)) {
			keysToFind -= 1;
		}*/
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "necromancer")
        {
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
}
