using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class skeletonArcher : MonoBehaviour {

	public float sightRange;

	public LayerMask layerMask;

	[HideInInspector]
	public GameObject aimTarget;

	public GameObject arrow;

	[HideInInspector]
	public bool enemyInSight;
	[HideInInspector]
	public bool halt;

	public float stopMovingDistance; // distance at which archer will stop and fire when targetting an enemy.  should be lower than sight range
	public float startMovingDistance; // distance at which archer will start moving again if enemy moves away.  should be larger than stop distance


	public float shotInterval;
	float maxShotInterval;

	public float myCurrentAngle;

	public float aimSpeed;

	IAstarAI ai;




	// Use this for initialization
	void Start () {
		maxShotInterval = shotInterval;
		ai = GetComponent<IAstarAI> ();

	}
	
	// Update is called once per frame
	void Update () {

		Sight ();

        if (aimTarget != null && aimTarget.GetComponent<enemyHealthManager>() && aimTarget.GetComponent<enemyHealthManager>().enemyHealth <= 0)
        {
            EndTarget();
        }

		//Debug.Log (gameObject.GetComponent<undeadMain>().travelling);


		shotInterval -= Time.deltaTime;

		if (aimTarget != null) {
		//	Debug.Log (Mathf.Abs (transform.position.x - aimTarget.transform.position.x));
			if (enemyInSight && Mathf.Abs (transform.position.x - aimTarget.transform.position.x) <= stopMovingDistance || Mathf.Abs (transform.position.y - aimTarget.transform.position.y) <= stopMovingDistance) {
				halt = true;
			}

			if (halt && Mathf.Abs (transform.position.x - aimTarget.transform.position.x) >= startMovingDistance || Mathf.Abs (transform.position.y - aimTarget.transform.position.y) >= startMovingDistance || !enemyInSight) {
				
				halt = false;
			}
		} else {
			halt = false;
		}

		if (halt) {
			gameObject.GetComponent<undeadMain> ().travelling = false;
		}

		if (gameObject.GetComponent<undeadMain>().travelling == false) {

			if (aimTarget != null) {

				Vector2 aimDirection = aimTarget.transform.position - transform.position;
				float aimAngle = (Mathf.Atan2 (-aimDirection.y, -aimDirection.x) * Mathf.Rad2Deg) - 90;
				//Quaternion aimRotation = Quaternion.AngleAxis (aimAngle, Vector3.forward);
				//transform.rotation = Quaternion.Slerp (transform.rotation, aimRotation, aimSpeed * Time.deltaTime);
				myCurrentAngle = Mathf.LerpAngle (myCurrentAngle, aimAngle, aimSpeed * Time.deltaTime);

			}

			if (enemyInSight) {
				if (shotInterval <= 0) {
					GameObject myArrow = Instantiate (arrow, transform.position, gameObject.transform.rotation);
					myArrow.GetComponent<friendlyArrow> ().papa = gameObject;
					shotInterval = maxShotInterval;
				}
			}
			
			//Sight ();
		} else {
			myCurrentAngle = 0;
		}

		if (gameObject.GetComponent<undeadMain> ().targetingMode && halt) {
			ai.isStopped = true;
		} else if (!gameObject.GetComponent<undeadMain> ().targetingMode || !halt) {
			ai.isStopped = false;
		}

		gameObject.transform.localEulerAngles = new Vector3(0, 0, myCurrentAngle);
	}

    public void EndTarget()
    {
        aimTarget = null;
        gameObject.GetComponent<undeadMain>().target = null;
        ai.destination = transform.position;
    }

	private void Sight (){

		enemyInSight = false;

		int RaysToShoot = 360;

		float angle = 0;

		GameObject newTarget = null;
		float nearestDistance = 1000000000000f;
		for (int i=0; i<RaysToShoot; i++) {
			float x = Mathf.Sin (angle);
			float y = Mathf.Cos (angle);
			angle += 2 * Mathf.PI / RaysToShoot;

			Vector2 dir = new Vector2 (x,y);
			RaycastHit2D hit;
			//Debug.DrawRay (transform.position, (Vector2)dir, Color.red);
			hit = Physics2D.Raycast ((Vector2)transform.position, dir, sightRange, layerMask);
			//if (Physics.Raycast (transform.position, dir, out hit, 3f, layerMask)) {
			if (hit.collider != null) {

				if (hit.collider.gameObject.CompareTag("enemy")) {
					if (aimTarget == null) {
						aimTarget = hit.collider.gameObject;
						if (shotInterval < .5f) {
							shotInterval = .5f;
						}
					} else if (aimTarget == hit.collider.gameObject) {
						if (!enemyInSight) {
							enemyInSight = true;
						}
					}

						
					//animator.SetLayerWeight(1, 1);

					if (hit.distance < nearestDistance) {
						nearestDistance = hit.distance;
						newTarget = hit.collider.gameObject;
					}
				}
			}
			//}
		}

	}
}
