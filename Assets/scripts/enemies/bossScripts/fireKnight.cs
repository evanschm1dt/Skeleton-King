using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class fireKnight : MonoBehaviour
{

    [HideInInspector]
    public Animator animator;

    public GameObject bossPortrait;

    public float burstTimerMin;
    public float burstTimerMax;
    [HideInInspector]
    public float burstTimer;

    public int burstDamage;

    public int burstParticles;

    ParticleSystem particles;

    public ParticleSystem sprayParticleSystem;
    public int sprayParticles;

    bool isBursting;
    bool isSpraying;

    float sprayTimer;
    public float sprayTimerMax;

    public float sprayRotationSpeed;

    public float specialAttackDistance;

    [HideInInspector]
    public bool fighting;

    IAstarAI guardAI;

    float myCurrentAngle = 0;

    bool buffed = false;

    Rigidbody2D rb;



    // Start is called before the first frame update
    void Start()
    {


        sprayTimer = sprayTimerMax;

        burstTimer = .5f;

        particles = gameObject.GetComponent<ParticleSystem>();

        animator = GetComponent<Animator>();

        guardAI = GetComponentInParent<IAstarAI>();

        //bossPortrait.SetActive(false);

        gameObject.GetComponentInParent<guardPatrol>().canMove = false;
        guardAI.isStopped = true;

        rb = GetComponentInParent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (gameObject.GetComponentInParent<enemyHealthManager>().enemyHealth < (gameObject.GetComponentInParent<enemyHealthManager>().enemyHealthMax - (gameObject.GetComponentInParent<enemyHealthManager>().enemyHealthMax * .33)) && !buffed)
        {
            buffed = true;
            sprayTimerMax *= 2;
        }
            //Debug.Log((gameObject.GetComponentInParent<enemyHealthManager>().enemyHealthMax - (gameObject.GetComponentInParent<enemyHealthManager>().enemyHealthMax * .33)));
            //AssessEnemies();
            if (fighting && gameObject.GetComponentInParent<guardPatrol>().foe == null)
        {
            gameObject.GetComponentInParent<guardPatrol>().foe = gameObject.GetComponentInParent<guardPatrol>().necromancer;
        }

        gameObject.transform.parent.localEulerAngles = new Vector3(0, 0, myCurrentAngle);

        if (gameObject.GetComponentInParent<guardPatrol>().foe != null && !fighting)
        {
            
            animator.SetTrigger("awaken");
        }
       if (burstTimer > 0)
        {
            if (fighting)
            {
                burstTimer -= Time.deltaTime;
            }
        }

        if (burstTimer <= 0 && !isBursting && !isSpraying)
        {
            // ------- THE KNIGHT ASSESSES HIS SURROUNDINGS BEFORE CHOOSING HIS NEXT MOVE -----------------------
            int enemiesNearMe = AssessEnemies();

            if (enemiesNearMe >= 2)
            {
                if (!buffed)
                {
                    animator.SetTrigger("burst");
                }else
                {
                    animator.SetTrigger("doubleBurst");
                }
                isBursting = true;
            } else if (enemiesNearMe < 2 && gameObject.GetComponentInParent<guardPatrol>().foeInSight)
            {
                animator.SetBool("spraying", true);
                isSpraying = true;

                Vector2 aimDirection = gameObject.GetComponentInParent<guardPatrol>().foe.transform.position - transform.position;
                float aimAngle = (Mathf.Atan2(-aimDirection.y, -aimDirection.x) * Mathf.Rad2Deg) - 90;
                myCurrentAngle = aimAngle;
                gameObject.transform.parent.localEulerAngles = new Vector3(0, 0, myCurrentAngle);
            }
        }

        if (isBursting)
        {
            gameObject.GetComponentInParent<guardPatrol>().attackTimer = 1;
            gameObject.GetComponentInParent<guardPatrol>().canMove = false;
           guardAI.isStopped = true;
        }

        if (isSpraying)
        {
            sprayParticleSystem.Emit(sprayParticles);

            gameObject.GetComponentInParent<guardPatrol>().attackTimer = 1;
            gameObject.GetComponentInParent<guardPatrol>().canMove = false;
            guardAI.isStopped = true;

            // -----------ROTATION--------
            if (gameObject.GetComponentInParent<guardPatrol>().foe != null)
            {
                Vector2 aimDirection = gameObject.GetComponentInParent<guardPatrol>().foe.transform.position - transform.position;
                float aimAngle = (Mathf.Atan2(-aimDirection.y, -aimDirection.x) * Mathf.Rad2Deg) - 90;
                myCurrentAngle = Mathf.LerpAngle(myCurrentAngle, aimAngle, sprayRotationSpeed * Time.deltaTime);
            }
            //--------------------------

            if (sprayTimer > 0)
            {
                sprayTimer -= Time.deltaTime;
            }
            if (sprayTimer <= 0)
            {
                animator.SetBool("spraying", false);
                gameObject.GetComponentInParent<guardPatrol>().canMove = true;
                guardAI.isStopped = false;
                sprayTimer = sprayTimerMax;
                burstTimer = Random.Range(burstTimerMin, burstTimerMax);
                isSpraying = false;
            }
        }else if (!isSpraying)
        {
            myCurrentAngle = 0;
        }

        if (gameObject.GetComponentInParent<guardPatrol>().necromancer == null && fighting)
        {
            fighting = false;
        }

    }

    public void Burst()
    {
        particles.Emit(burstParticles);
        
    }

    public void BurstEnd()
    {
        burstTimer = Random.Range(burstTimerMin, burstTimerMax);
        isBursting = false;
        gameObject.GetComponentInParent<guardPatrol>().canMove = true;
        guardAI.isStopped = false;
    }

    public void Awaken()
    {
        fighting = true;
        //bossPortrait.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponentInParent<enemyHealthManager>().SetHealthActive();
        gameObject.GetComponentInParent<guardPatrol>().canMove = true;
        guardAI.isStopped = false;
    }

    int AssessEnemies()
    {
        int nearbyEnemies = 0;
        GameObject[] skeletons = GameObject.FindGameObjectsWithTag("skeleton");

        foreach (GameObject skeleton in skeletons)
        {
           
            if (Vector2.Distance(transform.position, skeleton.transform.position) < specialAttackDistance)
            {
                nearbyEnemies += 1;
                if (skeleton.name == "necromancer")
                {
                    nearbyEnemies += 5;
                }
            }
        }
        //Debug.Log(nearbyEnemies);
            
        return nearbyEnemies;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("skeleton"))
        {
            other.GetComponent<undeadHealthManager>().DamageMe(burstDamage);
        }

        if (other.gameObject.CompareTag("enemy"))
        {
            other.GetComponent<enemyHealthManager>().DamageMe(burstDamage);
        }
    }

    }
