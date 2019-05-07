using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class undeadHealthManager : MonoBehaviour {

	public GameObject manager;

	public float unitHealth;

	public bool king;

	[HideInInspector]
	public float maxHealth;

    public float damageTimerBase;

    float damageTimer;

    public bool healthDrain;

    public float drainTimerBase;

    float drainTimer;

    public GameObject deathEffect;

    public Slider healthBar;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("manager");
		maxHealth = unitHealth;

        damageTimer = damageTimerBase;
        drainTimer = drainTimerBase;

        healthBar.value = CalculateHealth();

        if (!king)
        {
            healthBar.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (healthBar.gameObject.activeInHierarchy)
        {
            healthBar.value = CalculateHealth();
        }

        if (unitHealth < maxHealth && !healthBar.gameObject.activeInHierarchy)
        {
            healthBar.gameObject.SetActive(true);
        }

        if (healthDrain)
        {
            drainTimer -= Time.deltaTime;
            if (drainTimer <= 0)
            {
                unitHealth -= 1;
                drainTimer = drainTimerBase;
            }
        }

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }

		if (unitHealth <= 0) {
			

			if (king) {
				manager.GetComponent<controller> ().bossIsDead = true;
				//gameObject.GetComponentInChildren<cameraController> ().gameOver = true;
				//gameObject.GetComponentInChildren<cameraController> ().unParent ();
			}

            Instantiate(deathEffect, transform.position, Quaternion.identity);

            manager.GetComponent<controller>().selectedSkeletons.Remove(gameObject);
            manager.GetComponent<controller>().selectableObjects.Remove(gameObject);
            Destroy (gameObject);
		}

		if (unitHealth > maxHealth) {
			unitHealth = maxHealth;
		}
	}

    float CalculateHealth()
    {
        return unitHealth / maxHealth;
    }

    public void DamageMe (int dmg)
    {
        if  (damageTimer <= 0)
        {
            unitHealth -= dmg;
            damageTimer = damageTimerBase;
        }
    }
}
