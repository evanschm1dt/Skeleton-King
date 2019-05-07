using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerMain : MonoBehaviour {

	public bool swivel;
	public float swivelMax;

	public int meleeDamage;
	public GameObject hitNumber;

	public float range;

	float meleeRange = 1;

	bool firing = false;

	int rotationSpeed = 50;

	float startingRotation;

	bool rotatingFwd = true;

	float shotInterval = 2f;

	float aimSpeed = 50f;

	private SpriteRenderer spriteRend;
	public Sprite archerNormal;
	public Sprite archerFighting;

	public Rigidbody2D myRigidbody2D;

	public Transform mySprite;

	public float myCurrentAngle;

	public GameObject arrow;

	[HideInInspector]
	public GameObject foe;

	public LayerMask layerMask;


	// Use this for initialization
	void Start () {
		myCurrentAngle = mySprite.localEulerAngles.z+180f;

		spriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();

		startingRotation = myCurrentAngle;
	}
	
	// Update is called once per frame
	void Update () {


		//myCurrentAngle = Vector2.Angle(myRigidbody2D.velocity, Vector2.right);


		shotInterval -= Time.deltaTime;

		Sight ();

		if (foe != null) {
			firing = true;
			spriteRend.sprite = archerFighting;
			Vector2 aimDirection = foe.transform.position - transform.position;
			float aimAngle = (Mathf.Atan2 (-aimDirection.y, -aimDirection.x) * Mathf.Rad2Deg) - 90;
			//Quaternion aimRotation = Quaternion.AngleAxis (aimAngle, Vector3.forward);
			//transform.rotation = Quaternion.Slerp (transform.rotation, aimRotation, aimSpeed * Time.deltaTime);
			myCurrentAngle = Mathf.LerpAngle(myCurrentAngle,aimAngle,aimSpeed * Time.deltaTime);
		
			if (Vector2.Distance (transform.position, foe.transform.position) < range && firing) {
				if (Vector2.Distance (transform.position, foe.transform.position) > meleeRange) {
					if (shotInterval <= 0) {
						Instantiate (arrow, transform.position, mySprite.rotation);
						shotInterval = 2;
					}
				} else if (Vector2.Distance (transform.position, foe.transform.position) <= meleeRange) {
					if (shotInterval <= 0) {
						foe.GetComponent<undeadHealthManager> ().unitHealth -= meleeDamage;
						Instantiate (hitNumber, foe.transform.position, Quaternion.identity);
						shotInterval = 3;
					}
				}
			}
		}

		//Debug.Log (Mathf.DeltaAngle(startingRotation, transform.localEulerAngles.z));
	//	Debug.Log (transform.localEulerAngles.z);
		if (foe == null) {
			firing = false;
			spriteRend.sprite = archerNormal;
			if (rotatingFwd) {
				myCurrentAngle += Time.deltaTime * rotationSpeed;
				//myRigidbody2D.MoveRotation (myRigidbody2D.rotation + Time.deltaTime * rotationSpeed);
			} else {
				myCurrentAngle -= Time.deltaTime * rotationSpeed;
				//myRigidbody2D.MoveRotation (myRigidbody2D.rotation + Time.deltaTime * -rotationSpeed);
			}

			if (Mathf.DeltaAngle (startingRotation, myCurrentAngle) > swivelMax) {
				rotatingFwd = false;
			} else if (Mathf.DeltaAngle (startingRotation, myCurrentAngle) < -swivelMax) {
				rotatingFwd = true;
			}
		}
		mySprite.localEulerAngles = new Vector3(0, 0, myCurrentAngle+180f	);
	}

	private void Sight (){



		int RaysToShoot = 20;




		GameObject newTarget = null;
		float nearestDistance = 1000000000000f;

		float angle = -myCurrentAngle; //however you are storing that
		float coneWidth=20f;//20 degrees     

		for (int currentRayAngle = Mathf.RoundToInt(angle-coneWidth/2); currentRayAngle < Mathf.RoundToInt(angle+coneWidth/2); currentRayAngle++) {
			float x = - Mathf.Sin (currentRayAngle*Mathf.Deg2Rad);
			float y = -Mathf.Cos (currentRayAngle*Mathf.Deg2Rad);
		//	i += 1;


			Vector2 dir = new Vector2 (x, y);
			RaycastHit2D hit;
			Debug.DrawRay (transform.position, (Vector2)dir, Color.red);
			hit = Physics2D.Raycast ((Vector2)transform.position, dir, range, layerMask);
			//if (Physics.Raycast (transform.position, dir, out hit, 3f, layerMask)) {
			if (hit.collider != null) {

				if (hit.collider.gameObject.CompareTag("skeleton")) {
					foe = hit.collider.gameObject;
					//firing = true;

					//here is how to do your cool stuff ;)
					//Debug.Log("hit " + hit.collider.name);
					if (hit.distance < nearestDistance) {
						nearestDistance = hit.distance;
						newTarget = hit.collider.gameObject;
					}
				} else {
					//firing = false;


				}
			}
		}
	}
}
