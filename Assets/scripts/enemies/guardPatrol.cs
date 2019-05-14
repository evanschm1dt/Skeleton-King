using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class guardPatrol : MonoBehaviour {

	public bool canPatrol;

    Rigidbody2D rb;

    [HideInInspector]
    public bool canMove = true;

	[HideInInspector]
	public bool patrolMode;
	[HideInInspector]
	public bool chaseMode;
	[HideInInspector]
	public bool fightMode;

    public bool weak;
    public GameObject mySkele;

    //int fightDistance = 1;
    public float fightDistance;

	public float attackInterval;
    [HideInInspector]
	public float attackTimer = 0;

	public bool patrolWaiting;
	public float totalWaitTime;

	// LIMITED TIME CHASE STUFF -------------
	public float chaseTimerBase;
	[HideInInspector]
	public float chaseTimer;
	[HideInInspector]
	public bool foeInSight;
	public float chaseSightRange;
	public float chaseEndWaitTimer;
	[HideInInspector]
	public bool waitingAfterChase = false;
	//---------------------------------------

	public float baseSpeed;
	public float chaseSpeed;

	[HideInInspector]
	public GameObject foe = null;
	new Vector3 currentFoePos;
	new Vector3 recordedFoePos;

	new Vector3 startPos;

	public int startingWaypoint;

	bool hasNewPath = false;

	[SerializeField]
	List<GameObject> patrolPoints;

	int currentPatrolIndex;
	bool travelling;
	bool waiting = false;
	float waitTimer;

	public float guardSight;

	private SpriteRenderer spriteRend;

	public GameObject corpse;

	public GameObject[] hitNumbers;

	public LayerMask layerMask;
	IAstarAI guardAI;

	public bool checkPatrol;

	public int attackDamage;

	[HideInInspector]
	public GameObject manager; 

	[HideInInspector]
	public GameObject necromancer;

	//[HideInInspector]
	public Vector2 direction;

	[HideInInspector]
	public Animator animator;

    public bool boss;


	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;

		animator = GetComponentInChildren<Animator> ();

        guardAI = GetComponent<IAstarAI>();

        if (canPatrol) {
			patrolMode = true;
		} else
        {
            guardAI.isStopped = true;
        }

		chaseTimer = chaseTimerBase;

		chaseEndWaitTimer = Random.Range (0.5f, 4f);

		//Random.seed = (int)System.DateTime.Now.Ticks;

		spriteRend = gameObject.GetComponentInChildren<SpriteRenderer>();

		manager = GameObject.Find ("manager");

		necromancer = GameObject.Find ("necromancer");

		

		if (patrolMode) {
			currentPatrolIndex = startingWaypoint;
			SetDestination ();
		}
	}
	
	// Update is called once per frame
	void Update () {

        if (necromancer == null)
        {
            guardAI.isStopped = true;
        }

		AnimateMovement (direction);

		if (!canPatrol) {
			if (patrolMode) {
				patrolMode = false;
			}
		}

		if (checkPatrol) {
            Debug.Log(guardAI.isStopped);
		}

		if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		}

	//--------------------PATROL MODE---------------------------------
		if (patrolMode) {

			if (!guardAI.reachedEndOfPath) {
				hasNewPath = false;
			}

			if (guardAI.reachedEndOfPath && !hasNewPath) {
			
				//travelling = false;


				if (patrolWaiting) {
					waiting = true;

				} else { 
					ChangePatrolPoint ();
					SetDestination ();
					hasNewPath = true;
				}
			}

			if (gameObject.GetComponent<AIPath> ().maxSpeed != baseSpeed) {
				gameObject.GetComponent<AIPath> ().maxSpeed = baseSpeed;
			}

			if (waiting) {
				waitTimer += Time.deltaTime;
				if (waitTimer >= totalWaitTime) {
					waiting = false;
					waitTimer = 0f;
					ChangePatrolPoint ();
					SetDestination ();
					hasNewPath = true;
				}
			}
		}
	//------------------------------------------------------------
	//--------------------CHASE MODE--------------------------------
		if (chaseMode) {

            if (guardAI.isStopped && canMove)
            {
                guardAI.isStopped = false;
            }

			chasingSight ();

			if (foeInSight) {
				if (chaseTimer < chaseTimerBase) {
					chaseTimer = chaseTimerBase;
					chaseEndWaitTimer = Random.Range (0f, 4f);
				}
			} else if (!foeInSight) {
				chaseTimer -= Time.deltaTime;
			}

			if (chaseTimer <= 0) {

				foe = null;

				//waitingAfterChase = true;

				endChase ();

				chaseMode = false;

			}

			if (gameObject.GetComponent<AIPath> ().maxSpeed != chaseSpeed) {
				gameObject.GetComponent<AIPath> ().maxSpeed = chaseSpeed;
			}

			


			if (foe != null) {
				currentFoePos = foe.transform.position;

				if (Vector3.Distance (transform.position, foe.transform.position) < fightDistance){
					guardAI.isStopped = true;
					fightMode = true;
					chaseMode = false;
				}

                if (!hasNewPath || foe.transform.position != recordedFoePos)
                {
                    SetDestination();
                    hasNewPath = true;
                }
            }

            

        }

		//---------------------------------------------------------------------
		//--------------------------WAITING AFTER A CHASE----------------------
		if (waitingAfterChase) {

			guardAI.isStopped = true;

			chasingSight ();

			chaseEndWaitTimer -= Time.deltaTime;

			if (chaseEndWaitTimer <= 0) {
				endChase ();
			}
		}
		//----------------------------------------------------------------------
		//-------------------------FIGHT MODE-----------------------------------

		if (fightMode) {



            if (!guardAI.isStopped)
            {
                guardAI.isStopped = true;
            }

            if (foe != null) {
				direction = foe.transform.position - transform.position;
				if (Vector3.Distance (transform.position, foe.transform.position) > fightDistance) {
                    if (canMove)
                    {
                        guardAI.isStopped = false;
                    }
					fightMode = false;
					chaseMode = true;
				}
				if (attackTimer <= 0) {
					Attack ();
					attackTimer = attackInterval;
				}
			}

		} else {
			direction = guardAI.steeringTarget - transform.position;
		}

		//----------------------------------------------------------------------

		if (!fightMode && !chaseMode) {
			Sight (); // THIS IS WHERE THE GUARD LOOKS AROUND
		}

		if (foe != null && !boss) {
			if (foe.GetComponent<undeadHealthManager> ().unitHealth <= 0) {
			//	hasNewPath = false;
				guardAI.isStopped = false;
				if (canPatrol) {
					patrolMode = true;
				}
				fightMode = false;
				chaseMode = false;
			
				animator.SetLayerWeight(1, 0);
				ChangePatrolPoint ();
				SetDestination ();
				foe = null;
			}
		} else if (foe == null && !boss){
			if (canPatrol) {
				patrolMode = true;
			}
			chaseMode = false;
			fightMode = false;
			/*if (guardAI.isStopped) {
				guardAI.isStopped = false;
			}*/

		}

        guardAI.SearchPath();
    }

	private void SetDestination () {
		
		if (patrolMode) {
			Vector3 targetVector = patrolPoints [currentPatrolIndex].transform.position;
			targetVector.z = transform.position.z;
			guardAI.destination = targetVector;
			guardAI.SearchPath ();
			travelling = true;

		} else if (chaseMode) {
            if (foe != null)
            {
                Vector3 targetVector = foe.transform.position;
                targetVector.z = transform.position.z;
                recordedFoePos = targetVector;
                guardAI.destination = targetVector;
                guardAI.SearchPath();
                travelling = true;
            }
		}
	}

	private void ChangePatrolPoint () {
		
		currentPatrolIndex++;
		if (currentPatrolIndex >= patrolPoints.Count) {
			currentPatrolIndex = 0;
		}
	}

	private void Attack (){

        animator.SetTrigger("attack");

		//int attackDamage = Random.Range (0, 4);
		//Debug.Log (attackDamage);
		foe.GetComponent<undeadHealthManager> ().unitHealth -= attackDamage;
		//Instantiate (hitNumbers [attackDamage], foe.transform.position, Quaternion.identity);

        // ---- THIS TRIGGERS THE BODYGUARD SYSTEM. -----

		if (foe == necromancer) {
			manager.GetComponent<controller> ().alertUndead (gameObject);
		}
	}

	private void Sight (){

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
			hit = Physics2D.Raycast ((Vector2)transform.position, dir, guardSight, layerMask);
			//if (Physics.Raycast (transform.position, dir, out hit, 3f, layerMask)) {
				if (hit.collider != null) {
				

				
				if (hit.collider.gameObject.CompareTag("skeleton")) {
					foe = hit.collider.gameObject;
						patrolMode = false;
						chaseMode = true;
						animator.SetLayerWeight(1, 1);
						//here is how to do your cool stuff ;)
						//Debug.Log("hit " + hit.collider.name);
						if (hit.distance < nearestDistance) {
							nearestDistance = hit.distance;
							newTarget = hit.collider.gameObject;
						}

                        if (gameObject.GetComponent<keyGuard>())
                           {
                             gameObject.GetComponent<keyGuard>().CryForHelp();
                           }
					}
				}
			//}
		}
			
	}

	private void chasingSight (){

		int RaysToShoot = 100;

		float angle = 0;

		GameObject newTarget = null;
		float nearestDistance = 1000000000000f;
		foeInSight = false;
		for (int i = 0; i < RaysToShoot; i++) {
			float x = Mathf.Sin (angle);
			float y = Mathf.Cos (angle);
			angle += 2 * Mathf.PI / RaysToShoot;

			Vector2 dir = new Vector2 (x, y);
			RaycastHit2D hit;
			//Debug.DrawRay (transform.position, (Vector2)dir, Color.red);
			hit = Physics2D.Raycast ((Vector2)transform.position, dir, chaseSightRange, layerMask);
			//if (Physics.Raycast (transform.position, dir, out hit, 3f, layerMask)) {
			if (hit.collider != null) {


				if (hit.collider.gameObject == foe) {
					//Debug.Log ("I see you");
					if (!foeInSight) {
						foeInSight = true;
					}
				}
			}
			//}
		}
	}

	private void endChase () {
		if (canPatrol) {
			patrolMode = true;
		} else {
			guardAI.destination = startPos;
            patrolMode = false;
		}

        if (guardAI.isStopped)
        {
            guardAI.isStopped = false;
        }

        waitingAfterChase = false;
		chaseEndWaitTimer = Random.Range (0.5f, 4f);

        chaseMode = false;
        fightMode = false;

		animator.SetLayerWeight(1, 0);
		ChangePatrolPoint ();
		SetDestination ();
	}

	public void AnimateMovement (Vector2 direction) {
		if (animator == null)
			return;
		animator.SetFloat ("x", direction.x);
		animator.SetFloat ("y", direction.y);

        if (guardAI.isStopped)
        {
            animator.SetBool("moving", false);
        } else
        {
            animator.SetBool("moving", true);
        }
	}

    public void Instakill()
    {
        GameObject newMe = Instantiate(mySkele, transform.position, Quaternion.identity);
        newMe.GetComponent<undeadMain>().direction = gameObject.GetComponent<guardPatrol>().direction;
       // newMe.GetComponent<undeadMain>().AnimateMovement(direction);
        Destroy(gameObject);
    }
}
