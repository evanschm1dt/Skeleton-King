using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corpse : MonoBehaviour {

	public bool presetPosition;

	public GameObject unitType;

    private Animator animator;

    GameObject manager;

    [HideInInspector]
    public bool used;

    CircleCollider2D collider;

    ParticleSystem particles;

    public bool canConsume;

    public int minParticles;
    public int maxParticles;
 
    public int particleCount;

    public bool createParticles;

    public bool randomParticles;

    public bool unselect;

    float zOffset;

    //GameObject bloodMaker;

	// Use this for initialization
	void Start () {

        zOffset = Random.Range(0.00001f, 0.001f);

        transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z + zOffset));

        collider = gameObject.GetComponent<CircleCollider2D>();

        manager = GameObject.Find("manager");

        //bloodMaker = GameObject.Find("bloodMaker");

        animator = GetComponentInChildren<Animator>();

        particles = gameObject.GetComponent<ParticleSystem>();

        transform.position = new Vector3(transform.position.x, transform.position.y, 0.9f);

		if (!presetPosition) {
			transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
           // bloodMaker.GetComponent<bloodMaker>().MoveToCorpse(transform.position);
		}

        if (manager.GetComponent<controller>().bossIsDead)
        {
            createParticles = false;
            Consume();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reanimate()
    {
        animator.SetTrigger("reanimate");
        used = true;
        collider.enabled = false;
    }

    public void Consume()
    {
        if (canConsume)
        {
            animator.SetTrigger("consume");
            used = true;
        }
    }

    public void EmitParticles()
    {
        if (createParticles)
        {
            if (!randomParticles)
            {
                particles.Emit(particleCount);
            } else {
                particles.Emit(Random.Range(minParticles, maxParticles));
            }
        }
    }

    public void ReanimateComplete()
    {
        GameObject mySkeleton = Instantiate(unitType, new Vector3(transform.position.x, transform.position.y, .1f), Quaternion.identity);
        if (unselect)
        {
            mySkeleton.GetComponent<undeadMain>().startSelected = false;
        }
        Destroy(gameObject);
    }

    public void ConsumeComplete()
    {
        Destroy(gameObject, 5f);
    }
}
