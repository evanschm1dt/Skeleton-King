using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class necromancerMain : MonoBehaviour {

	//GameObject myManager;

	new Vector3 myPosition;

	public GameObject projectile;

	[HideInInspector]
	public GameObject manager; 

	bool canFire = true;

    bool hasAccelerated;

	[Range(0, 2)]
	public float projectileCooldownBase;
	float projectileCooldown;

	[Range (1f, 5f)]
	public float walkSpeed;
    [Range(6f, 20f)]
    public float accel;

    float currentSpeed = 0;

	[HideInInspector]
	public Rigidbody2D rb;

	[HideInInspector]
	public Vector2 direction;
	[HideInInspector]
	public Vector2 currentfacingDirection;

	[HideInInspector]
	public bool scrying = false;

	//public Text healthText;



    [HideInInspector]
	public Animator animator;


	// Use this for initialization
	void Start () {
		//myManager = GameObject.Find ("manager");

		myPosition = transform.position;

		rb = GetComponent<Rigidbody2D>();

		currentfacingDirection = new Vector2 (0, -1);

		projectileCooldown = projectileCooldownBase;

		//healthText.text = "King's Health: " + gameObject.GetComponent<undeadHealthManager> ().unitHealth;

		animator = GetComponentInChildren<Animator> ();

		manager = GameObject.Find ("manager");

        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
       

        currentSpeed = Mathf.Clamp(currentSpeed, 0, walkSpeed);

		//healthText.text = "King's Health: " + gameObject.GetComponent<undeadHealthManager> ().unitHealth + "/" + gameObject.GetComponent<undeadHealthManager> ().maxHealth;
		/*if (gameObject.GetComponent<undeadHealthManager> ().unitHealth <= 0) {
			myManager.GetComponent<controller> ().bossIsDead = true;
		}*/

		if (Input.GetKey (KeyCode.LeftShift) && !scrying) {
			scrying = true;
		} else if (!Input.GetKey (KeyCode.LeftShift) && scrying) {
			scrying = false;
		}

        if (scrying)
        {
            animator.SetBool("scrying", true);
        }
        else
        {
            animator.SetBool("scrying", false);
        }

        if (!scrying && !manager.GetComponent<controller>().paused /*&& !manager.GetComponent<controller>().fadeOut*/) {
			if (canFire) {
				MovementInput ();
                //transform.Translate (direction * walkSpeed * Time.deltaTime);
            
				rb.MovePosition (rb.position + direction * walkSpeed * Time.deltaTime);
				AnimateMovement (direction);
			}

			if (direction != Vector2.zero && direction != currentfacingDirection) {
				currentfacingDirection = direction;
			}

			if (Input.GetKeyDown (KeyCode.Space) && canFire) {
				GameObject myShot = Instantiate (projectile, transform.position, Quaternion.identity);
				canFire = false;
				direction = (Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position);
				AnimateMovement (direction);
				//Debug.Log (direction);
			}
		}

		if (!canFire) {
			projectileCooldown -= Time.deltaTime;
			if (projectileCooldown <= 0) {
				canFire = true;
				projectileCooldown = projectileCooldownBase;
			}
		}

       /* if (manager.GetComponent<controller>().fadeOut)
        {
            direction = new Vector2(0, 0);
            AnimateMovement(direction);
        }*/


    }

	/*void FixedUpdate() {
		if (Input.GetKey (KeyCode.W)) {
			//myPosition.y += walkSpeed;
			rb.MovePosition(transform.position + Vector2.up * (walkSpeed));
		}
		if (Input.GetKey (KeyCode.S)) {
			//myPosition.y -= walkSpeed;
			rb.MovePosition(transform.position - Vector2.up * (walkSpeed ));
		}
		if (Input.GetKey (KeyCode.A)) {
			//myPosition.x -= walkSpeed;
			rb.MovePosition(transform.position + Vector2.left * (walkSpeed));
		}
		if (Input.GetKey (KeyCode.D)) {
			//myPosition.x += walkSpeed;
			rb.MovePosition(transform.position + Vector2.right * (walkSpeed));
		}
	}*/

	void MovementInput () {

		direction = Vector2.zero;

		if (Input.GetKey (KeyCode.W)) {
			direction += Vector2.up;
            if (currentSpeed < walkSpeed && !hasAccelerated)
            {
                currentSpeed += accel * Time.deltaTime;
                hasAccelerated = true;
            }
		}
		if (Input.GetKey (KeyCode.S)) {
			direction += Vector2.down;
            if (currentSpeed < walkSpeed && !hasAccelerated)
            {
                currentSpeed += accel * Time.deltaTime;
                hasAccelerated = true;
            }
        }
		if (Input.GetKey (KeyCode.A)) {
			direction += Vector2.left;
            if (currentSpeed < walkSpeed && !hasAccelerated)
            {
                currentSpeed += accel * Time.deltaTime;
                hasAccelerated = true;
            }
        }
		if (Input.GetKey (KeyCode.D)) {
			direction += Vector2.right;
            if (currentSpeed < walkSpeed && !hasAccelerated)
            {
                currentSpeed += accel * Time.deltaTime;
                hasAccelerated = true;
            }
        }

        hasAccelerated = false;
        if (direction == Vector2.zero)
        {
            currentSpeed = 0;
        }
	}

	public void AnimateMovement (Vector2 direction) {
		animator.SetFloat ("x", direction.x);
		animator.SetFloat ("y", direction.y);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)){
            animator.SetBool("moving", true);
        } else {
            animator.SetBool("moving", false);
        }

        if (!canFire)
        {
            animator.SetBool("attacking", true);
        } else {
            animator.SetBool("attacking", false);
        }

        if (scrying)
        {
            animator.SetBool("scrying", true);
        }
        else
        {
            animator.SetBool("scrying", false);
        }

    }
}
