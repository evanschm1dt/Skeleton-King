using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class friendlyArrow : MonoBehaviour {

	[HideInInspector]
	public Vector2 movementDirection;

	public int arrowSpeed;

	public int arrowDamage;

	public GameObject hitNumber;

	[HideInInspector]
	public GameObject papa; //this is the skeleton that fired the arrow


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, Time.deltaTime * arrowSpeed, 0);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("enemy")) {

            if (other.GetComponent<archerMain>())
            {
                if (other.GetComponent<archerMain>().foe == null)
                {
                    other.GetComponent<archerMain>().foe = papa;
                }
            }

			if (other.GetComponent<guardPatrol> ()) {
           
				if (other.GetComponent<guardPatrol> ().foe == null)
                {
					other.GetComponent<guardPatrol> ().foe = papa;
					other.GetComponent<guardPatrol> ().patrolMode = false;
					other.GetComponent<guardPatrol> ().chaseMode = true;
					other.GetComponent<guardPatrol>().animator.SetLayerWeight(1, 1);
                    if (other.GetComponent<keyGuard>())
                    {
                        other.GetComponent<keyGuard>().CryForHelp();
                    }
                }
			}

			other.GetComponent<enemyHealthManager> ().enemyHealth -= arrowDamage;
            if (other.GetComponent<enemyHealthManager>().enemyHealth <= 0)
            {
                papa.GetComponent<skeletonArcher>().EndTarget();
            }
			//Instantiate (hitNumber, other.transform.position, Quaternion.identity);
		}
		if (other.gameObject.layer != 11) {
            gameObject.GetComponent<projectileAudio>().SpawnEndSound();
            Destroy (gameObject);
		} 
	}
}
