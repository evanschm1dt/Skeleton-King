using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour {

	int arrowSpeed = 12;
	public int arrowDamage;

	public GameObject [] hitNumbers;

	// Use this for initialization
	void Start () {
		//arrowDamage = Random.Range (2, 4);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, Time.deltaTime * arrowSpeed, 0);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("skeleton")) {
			
			other.GetComponent<undeadHealthManager> ().unitHealth -= arrowDamage;
			//Instantiate (hitNumbers [arrowDamage], other.transform.position, Quaternion.identity);
		}
		if (other.gameObject.layer != 11) {
            gameObject.GetComponent<projectileAudio>().SpawnEndSound();
			Destroy (gameObject);
		} 
	}
}
