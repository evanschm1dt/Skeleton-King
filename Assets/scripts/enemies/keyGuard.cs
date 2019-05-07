using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyGuard : MonoBehaviour {

	public GameObject myDoor;

    public GameObject[] myBodyguards;

	bool keyTaken = false;

    public GameObject key;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponent<enemyHealthManager> ().isDead) {
            //	Debug.Log ("ded");
            //if (!keyTaken) {
            //myDoor.GetComponent<keyManager> ().keysToFind -= 1;
            //keyTaken = true;
            //}
            GameObject myKey = Instantiate(key, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            myKey.GetComponent<key>().door = myDoor;
		}

	}

    public void CryForHelp()
    {
        
        foreach (GameObject bodyguard in myBodyguards)
        {
            if (bodyguard.GetComponent<guardPatrol>())
            {
                
                if (bodyguard.GetComponent<guardPatrol>().foe == null)
                {
                    
                    bodyguard.GetComponent<guardPatrol>().foe = gameObject.GetComponent<guardPatrol>().foe;
                    bodyguard.GetComponent<guardPatrol>().patrolMode = false;
                    bodyguard.GetComponent<guardPatrol>().chaseMode = true;
                    bodyguard.GetComponent<guardPatrol>().animator.SetLayerWeight(1, 1);
                }
            }

            if (bodyguard.GetComponent<archerMain>())
            {
                bodyguard.GetComponent<archerMain>().foe = gameObject.GetComponent<guardPatrol>().foe;
            }
        }
    }
}
