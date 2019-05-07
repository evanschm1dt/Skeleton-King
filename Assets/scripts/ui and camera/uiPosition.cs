using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiPosition : MonoBehaviour {

//	public Sprite circleOn;
//	public Sprite circleOff;
	public SpriteRenderer spriteRend;

	[HideInInspector]
	public GameObject boss;

	[HideInInspector]
	public bool tooFar = false;

	public float spellRange = 0f;

	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("necromancer");
	}
	
	// Update is called once per frame
	void Update () {
		var pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		pos.z = -1;
		transform.position = pos;

        if (boss != null)
        {
            if (Vector3.Distance(transform.position, boss.transform.position) > spellRange)
            {
                //if (!tooFar) {
                tooFar = true;
                gameObject.GetComponent<Animator>().SetBool("inRange", false);
                //}
            }
            if (Vector3.Distance(transform.position, boss.transform.position) < spellRange)
            {
                //if (tooFar) {
                tooFar = false;
                gameObject.GetComponent<Animator>().SetBool("inRange", true);
                //}
            }
        }

	}
}
