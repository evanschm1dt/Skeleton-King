using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corpseInteraction : MonoBehaviour {

	[HideInInspector]
	public GameObject targetCorpse;

	[HideInInspector]
	public GameObject boss;

	[HideInInspector]
	public bool hasCorpse = false;

	[HideInInspector]
	public bool deliverCorpse = false;

	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("necromancer");
	}
	
	// Update is called once per frame
	void Update () {
		if (targetCorpse != null) {
			if (Vector3.Distance(transform.position, boss.transform.position) < 1 && deliverCorpse) {
				targetCorpse.transform.parent = null;
				targetCorpse = null;
				hasCorpse = false;
				deliverCorpse = false;
			}


		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject == targetCorpse && targetCorpse != null) {
			other.gameObject.transform.parent = transform;
			hasCorpse = true;
			Debug.Log ("got a corpse");
		}
	}
}
