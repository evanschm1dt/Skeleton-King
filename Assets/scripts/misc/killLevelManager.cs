using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killLevelManager : MonoBehaviour {

	public GameObject target;




	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (target.GetComponent<enemyHealthManager> ().isDead == true) {
			gameObject.GetComponent<controller> ().youWin = true;
		}
	}
}
