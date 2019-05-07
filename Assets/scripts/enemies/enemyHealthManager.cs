using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthManager : MonoBehaviour {

	public float enemyHealth;
    [HideInInspector]
    public float enemyHealthMax;

	public GameObject corpse;

	[HideInInspector]
	public bool isDead = false;

    public bool boss;

    public bool flameResistant;

    public bool consumeImmune;

    public bool unselect;

    public float damageTimerBase;
    float damageTimer;

    public Slider healthBar;


    

	// Use this for initialization
	void Start () {
        damageTimer = damageTimerBase;

        enemyHealthMax = enemyHealth;

        healthBar.value = CalculateHealth();

        healthBar.gameObject.SetActive(false);
        
    }
	
	// Update is called once per frame
	void Update () {

        if (healthBar.gameObject.activeInHierarchy)
        {
            healthBar.value = CalculateHealth();
        }


        if (!boss)
        {
            if (enemyHealth < enemyHealthMax && !healthBar.gameObject.activeInHierarchy)
            {
                SetHealthActive();
            }
        }

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }

        if (isDead) {
			GameObject myCorpse = Instantiate (corpse, transform.position, transform.rotation);
            healthBar.gameObject.SetActive(false);
            if (consumeImmune)
            {
                myCorpse.GetComponent<corpse>().canConsume = false;
            }

            if (unselect)
            {
                myCorpse.GetComponent<corpse>().unselect = true;
            }
            Destroy (gameObject);
		}

		if (enemyHealth <= 0) {
			isDead = true;
		}


	}

    float CalculateHealth()
    {
        return enemyHealth / enemyHealthMax;
    }

    public void SetHealthActive()
    {
        healthBar.gameObject.SetActive(true);
    }

    public void DamageMe(int dmg)
    {
        if (damageTimer <= 0)
        {
            enemyHealth -= dmg;
            damageTimer = damageTimerBase;
        }
    }
}
