using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour {

	GameObject king;

	Vector2 projectileDirection;

	[Range (1f, 10f)]
	public float projectileSpeed;

	[Range (0f, 2f)]
	public float projectileLifetime;

	[Range (0f, 2f)]
	public float speedDecay;

	public int projectileDamage;

	public GameObject hitNumber;

    ParticleSystem particles;

    public int particleNumber;

    private Animator animator;

    // Use this for initialization
    void Start () {
		king = GameObject.Find ("necromancer");
		//projectileDirection = king.GetComponent<necromancerMain>().currentfacingDirection;
		//Mathf.Clamp (projectileDirection.x, -1, 1);
		//Mathf.Clamp (projectileDirection.y, -1, 1);

		projectileDirection = (Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position);
		projectileDirection.Normalize ();
        particles = GetComponentInChildren<ParticleSystem>();

        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate (projectileDirection * projectileSpeed * Time.deltaTime);

		projectileSpeed -= speedDecay * Time.deltaTime;

		projectileLifetime -= Time.deltaTime;
		if (projectileLifetime <= 0) {
            /*  particles.Emit(particleNumber);
              particles.Stop();
              particles.gameObject.transform.parent = null;
              Destroy(particles.gameObject, 1f);*/
            // Destroy (gameObject);
            animator.SetBool("ending", true);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("enemy")) {
			other.GetComponent<enemyHealthManager> ().enemyHealth -= 1;
			Instantiate (hitNumber, other.transform.position, Quaternion.identity);

            if (other.GetComponent<archerMain>())
            {
                if (other.GetComponent<archerMain>().foe == null)
                {
                    other.GetComponent<archerMain>().foe = king;
                }
            }

            if (other.GetComponent<guardPatrol>())
            {

                if (other.GetComponent<guardPatrol>().foe == null)
                {
                    other.GetComponent<guardPatrol>().foe = king;
                    other.GetComponent<guardPatrol>().patrolMode = false;
                    other.GetComponent<guardPatrol>().chaseMode = true;
                    other.GetComponent<guardPatrol>().animator.SetLayerWeight(1, 1);
                }
            }
        }
		if (other.gameObject.layer != 11) {
            /* particles.Emit(particleNumber);
             particles.Stop();
             particles.gameObject.transform.parent = null;
             Destroy(particles.gameObject, 1f);
             Destroy(gameObject);*/
            DestroyWithParticles();
		} 
	}

    void DestroyMe()
    {
        Destroy(gameObject);
    }

   public void DestroyWithParticles()
    {
        particles.Emit(particleNumber);
        particles.Stop();
        particles.gameObject.transform.parent = null;
        Destroy(particles.gameObject, 1f);
        Destroy(gameObject);
    }
}
