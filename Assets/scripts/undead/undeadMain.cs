using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class undeadMain : MonoBehaviour {

	[HideInInspector]
	public GameObject manager; 

	//[HideInInspector]
	public bool isSelected;

	private SpriteRenderer spriteRend;
	//public Sprite undeadUnselected;
	//public Sprite undeadSelected;

	GameObject closestEnemy = null;
	bool isFighting;

	float attackTimer = 0;
	public float attackInterval;

	public int attackDamage;

	new Vector3 targetPoint;

	[HideInInspector]
	public bool travelling;

	public bool projectileSkeleton;

	public GameObject[] hitNumbers;

	IAstarAI ai;

	Rigidbody2D pushBox;

	Text healthText;

	[HideInInspector]
	public GameObject necromancer;

	public int defenseDistance;// distance at which this unit will defend the king

	// stuff for chasing enemies
	[HideInInspector]
	public GameObject target;
	[HideInInspector]
	public bool targetingMode = false;

	//[HideInInspector]
	public Vector2 direction;

	private Animator animator;

	public bool debug;

    public bool startSelected;

    float zOffset;

    float baseZPos;


	// Use this for initialization
	void Start () {
        baseZPos = transform.position.z;
       /* zOffset = transform.position.z + Random.Range(0.0000001f, 0.01f);
        transform.position = new Vector3(transform.position.x, transform.position.y, zOffset);
        */

		animator = GetComponentInChildren<Animator> ();

		

		ai = GetComponent<IAstarAI> ();
		spriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();
		healthText = GetComponentInChildren<Text> ();

		necromancer = GameObject.Find ("necromancer");
		manager = GameObject.Find ("manager");
		manager.GetComponent<controller> ().selectableObjects.Add (this.gameObject);
		pushBox = GetComponent<Rigidbody2D> ();
     
        

        // ---------- THIS SELECTS THE SKELETON WHEN IT SPAWNS ---------------
        if (startSelected)
        {
            target = necromancer;
            targetingMode = true;
            isSelected = true;
            manager.GetComponent<controller>().selectedSkeletons.Add(gameObject);
            ClickedOn();
        }
        //------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update () {

        //transform.position = new Vector3(transform.position.x, transform.position.y, (baseZPos + (transform.position.y / 1000)));


        if (debug) {
            if (closestEnemy != null)
            {
                Debug.Log(Vector3.Distance(transform.position, closestEnemy.transform.position));
            }
		}



		AnimateMovement (direction);

		if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		} 


		FindClosestEnemy ();

       /* if (necromancer != null)
        {
            if (isSelected || necromancer.GetComponent<magic>().healActive == true)
            {
                healthText.text = "" + gameObject.GetComponent<undeadHealthManager>().unitHealth + "/" + gameObject.GetComponent<undeadHealthManager>().maxHealth;
            }
            else
            {
                healthText.text = "";
            }
        }*/
		/*
		if (gameObject.GetComponent<undeadMain> ().isSelected == false && ai.reachedEndOfPath) {
			pushBox.constraints = RigidbodyConstraints2D.FreezeAll;
		} else {
			pushBox.constraints = RigidbodyConstraints2D.FreezeRotation;
		}*/


		if (closestEnemy != null) {
			
			//if (Vector3.Distance(transform.position, ai.destination) < .2 && Vector3.Distance(transform.position, closestEnemy.transform.position) > .2) {

				//Physics.IgnoreLayerCollision (gameObject.layer, 10, false);
			//}

			if (Vector3.Distance (transform.position, closestEnemy.transform.position) < 1) {
				isFighting = true;
			} else {
				isFighting = false;
			}
		} else {
			isFighting = false;
		}

		if (isFighting) {
			direction = closestEnemy.transform.position - transform.position;
			 if (attackTimer <= 0) {
				Attack ();
				attackTimer = attackInterval;
			}
		}
		if (ai.reachedEndOfPath) {
			if (!isFighting && isSelected && direction.y > 0) {
				direction = new Vector2 (0, -1);
			}
			if (!ai.pathPending) {
				travelling = false;
			}
			if (pushBox.isKinematic == true) {
				pushBox.isKinematic = false;
			}
			//ai.destination = null;
			//ai.isStopped = true;
		} else {
			direction = ai.steeringTarget - transform.position;
		}

		//Debug.Log (unitHealth);

		if (targetingMode) {
			if (target == null) {
				targetingMode = false;
			} else if (target != null) {
				if (ai.destination != target.transform.position) {
					SetTarget ();
				}
			}
			//Debug.Log ("targeting");

		}


	}

	void FindClosestEnemy (){
		float distanceToClosestEnemy = Mathf.Infinity;
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag ("enemy");
		if (allEnemies.Length > 0) {
			foreach (GameObject currentEnemy in allEnemies) {
				float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
				if (distanceToEnemy < distanceToClosestEnemy) {
					distanceToClosestEnemy = distanceToEnemy;
					closestEnemy = currentEnemy;
				}
			}
		} else {
			closestEnemy = null;
		}
		//Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
	}

	void Attack(){
        animator.SetTrigger("attack");
        //int attackDamage = Random.Range (0, 4);
        //Debug.Log (attackDamage);
        closestEnemy.GetComponent<enemyHealthManager> ().enemyHealth -= attackDamage;
		//if (closestEnemy
		if (closestEnemy.GetComponent<archerMain> ()) {
			closestEnemy.GetComponent<archerMain> ().foe = gameObject;
		}

        if (closestEnemy.GetComponent<guardPatrol>())
        {
            closestEnemy.GetComponent<guardPatrol>().foe = gameObject;
        }
        //Instantiate (hitNumbers [attackDamage], closestEnemy.transform.position, Quaternion.identity);
    }
	

	public void ClickedOn (){
		if (isSelected) {
			//spriteRend.sprite = undeadSelected;
			animator.SetLayerWeight(1, 1);
		} else if (!isSelected) {
			//spriteRend.sprite = undeadUnselected;
			animator.SetLayerWeight(1, 0);
		}
		//isSelected = !isSelected;
	}

    public void UnselectMe()
    {
        isSelected = false;
        manager.GetComponent<controller>().selectedSkeletons.Remove(gameObject);
        ClickedOn();
    }

	public void SetDestination(){
		
		pushBox.isKinematic = true;
		var mousePoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePoint.z = transform.position.z;
		ai.destination = mousePoint;

		ai.SearchPath ();

		travelling = true;
	}

	public void SetTarget(){
		if (!gameObject.GetComponent<skeletonArcher>() || target == necromancer) {
			
			pushBox.isKinematic = true;
			var targetLocation = target.transform.position;
			targetLocation.z = transform.position.z;
			ai.destination = targetLocation;
			ai.SearchPath ();
			travelling = true;
		} else if (gameObject.GetComponent<skeletonArcher>() && target != necromancer){
			if (gameObject.GetComponent<skeletonArcher>()){
				gameObject.GetComponent<skeletonArcher> ().aimTarget = target;
			if (gameObject.GetComponent<skeletonArcher> ().halt) {
					travelling = false;
				}
			}
		}
	}

	public void AnimateMovement (Vector2 direction) {
		animator.SetFloat ("x", direction.x);
		animator.SetFloat ("y", direction.y);

        if (travelling)
        {
            animator.SetBool("moving", true);
        } else {
            animator.SetBool("moving", false);
        }

		if (gameObject.GetComponent<skeletonArcher> ()) {
			if (gameObject.GetComponent<skeletonArcher> ().enemyInSight && !travelling) {
				animator.SetBool ("aiming", true);
			} else {
				animator.SetBool ("aiming", false);
			}
		}
	}

    //---- THIS IS THE FINAL STEP IN THE BODYGUARD SYSTEM.  THIS IS TRIGGERED FROM CONTROLLER -----
	public void DefendYourKing(GameObject kingsFoe){
		if (!travelling && Mathf.Abs(transform.position.x - necromancer.transform.position.x) <= defenseDistance && Mathf.Abs(transform.position.y - necromancer.transform.position.y) <= defenseDistance) {
			targetingMode = true;
			target = kingsFoe;

            if (gameObject.GetComponent<skeletonArcher>())
            {
                gameObject.GetComponent<skeletonArcher>().aimTarget = kingsFoe;
            }

			SetTarget ();
		}
	}
		

}
