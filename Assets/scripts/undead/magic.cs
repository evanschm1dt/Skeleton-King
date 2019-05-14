using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class magic : MonoBehaviour {

	GameObject manager;

	public int mana;

	public Text manaCount;

	public GameObject manaWarning;
	float manaWarningTimer = 1.5f;
	//bool manaWarningVisible = false;

	public GameObject magicMouseSprite;
	GameObject magicMouse;

    public LayerMask layerMask;

	public GameObject[] undeadList;

	int currentSpellCost;

	[HideInInspector]
	public bool spellActive = false;

	[HideInInspector]
	public bool raiseDeadActive = false;
	public int raiseDeadCost;
	public KeyCode raiseDeadKey;
	public float raiseDeadRange;

	[HideInInspector]
	public bool consumeCorpseActive = false;
	public KeyCode consumeCorpseKey;
	public float consumeCorpseRange;

	[HideInInspector]
	public bool healActive = false;
	public int healCost;
	public KeyCode healKey;
	public GameObject[] healNumbers;
	public float healRange;

    [HideInInspector]
    public bool sacrificeActive = false;
    public KeyCode sacrificeKey;
    public float sacrificeRange;

    bool canBeEnded = false;

	bool cannotCast;

	public GameObject purpleSmoke;

    ParticleSystem particles;

    public int raiseDeadParticles;

    public float raiseDeadCooldownMax;
    float raiseDeadCooldown;

    [HideInInspector]
    public bool canRaise;

    public Slider raiseDeadMeter;

    public float consumeHealthRegen;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("manager");
		manaCount.text = "Mana: " + mana;
		manaWarning.SetActive (false);

        particles = gameObject.GetComponent<ParticleSystem>();

        raiseDeadCooldown = 0;

        raiseDeadMeter.value = CalculateRaise();
	}
	
	// Update is called once per frame
	void Update () {

        if (!canRaise)
        {
            raiseDeadMeter.value = CalculateRaise();
            raiseDeadCooldown -= Time.deltaTime;
            if (raiseDeadCooldown <= 0)
            {
                raiseDeadCooldown = raiseDeadCooldownMax;
                canRaise = true;
                
            }
        }

		if (spellActive) {
			manager.GetComponent<controller> ().isCasting = true;

			if (magicMouse.GetComponent<uiPosition> ().tooFar == true) {
				cannotCast = true;
			} else {
				cannotCast = false;
			}

		} else {
			manager.GetComponent<controller> ().isCasting = false;
		}

		if (mana < currentSpellCost) {
			cannotCast = true;
		}

		//---------------- HOTKEYS -------------------------------------

		if (Input.GetKeyDown (raiseDeadKey) && canRaise) {
			//RaiseDead ();
            particles.Emit(raiseDeadParticles);
            gameObject.GetComponentInChildren<audioPlayer>().PlayRaiseDead();
            canRaise = false;
        }

		if (Input.GetKeyDown (consumeCorpseKey) && !canBeEnded) {
			ConsumeCorpse ();
		}

		if (Input.GetKeyDown (healKey)) {
			Heal ();
		}

        if (Input.GetKeyDown(sacrificeKey))
        {
            Sacrifice();
        }

        //-------------------------------------------------------------

        if (manaWarning.activeSelf) {
			manaWarningTimer -= Time.deltaTime;
			if (manaWarningTimer <= 0) {
				manaWarningTimer = 1.5f;
				manaWarning.SetActive (false);
			}
		}

	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ BOOK OF MAGIC, PT. 1 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

		//=============================== RAISE DEAD =============================================

		if (raiseDeadActive) {
			if (Input.GetMouseButtonDown (0)) {
				Collider2D clickedCollider = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition), layerMask);

				if (clickedCollider != null) {

					if (clickedCollider.gameObject.CompareTag( "corpse" ) && !cannotCast && mana >= raiseDeadCost && clickedCollider.GetComponent<corpse>().used == false) {
                        //Instantiate (clickedCollider.GetComponent<corpse>().unitType, new Vector3 (clickedCollider.gameObject.transform.position.x, clickedCollider.gameObject.transform.position.y, 1), Quaternion.identity);
                        //Destroy (clickedCollider.gameObject);
                        clickedCollider.GetComponent<corpse>().Reanimate();

						mana -= raiseDeadCost;
						manaCount.text = "Mana: " + mana;
						//SpellEnd ();
					} else if (mana < raiseDeadCost) {
						manaWarning.SetActive (true);
					}
				}
			}

			if (Input.GetMouseButtonDown (1)) {
				SpellEnd ();
			}
		}

		//============================== CONSUME CORPSE ===========================================

		if (consumeCorpseActive) {
			if (Input.GetMouseButtonDown (0)) {
				Collider2D clickedCollider = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition), layerMask);

				if (clickedCollider != null) {

					if (clickedCollider.gameObject.CompareTag ("corpse") && !cannotCast && clickedCollider.GetComponent<corpse>().used == false) {
						//mana += 50;
                        //Instantiate(purpleSmoke, clickedCollider.gameObject.transform.position, Quaternion.identity);
                        //Destroy (clickedCollider.gameObject);
                        clickedCollider.GetComponent<corpse>().Consume();
                        gameObject.GetComponent<undeadHealthManager>().unitHealth += consumeHealthRegen;
                       // manaCount.text = "Mana: " + mana;
						//SpellEnd ();
					}
				}
			}

            if (Input.GetKeyUp(consumeCorpseKey))
            {
                canBeEnded = true;
            }

            if (canBeEnded)
            {
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(consumeCorpseKey))
                {
                    SpellEnd();
                }
            }
		}

		//================================== HEAL ===================================================

		if (healActive) {
			if (Input.GetMouseButtonDown (0)) {
				Collider2D clickedCollider = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition), layerMask);

				if (clickedCollider != null) {

					if (clickedCollider.gameObject.CompareTag ("skeleton") && !cannotCast && mana >= healCost) {
						if (clickedCollider.GetComponent<undeadHealthManager> ().unitHealth < clickedCollider.GetComponent<undeadHealthManager> ().maxHealth) {
							clickedCollider.GetComponent<undeadHealthManager> ().unitHealth += 4;
							Instantiate (healNumbers [1], clickedCollider.transform.position, Quaternion.identity);
							mana -= healCost;
							manaCount.text = "Mana: " + mana;
						} else {
							Instantiate (healNumbers[0], clickedCollider.transform.position, Quaternion.identity);
						}
					} else if (mana < healCost) {
						manaWarning.SetActive (true);
					}
				}
			}

			if (Input.GetMouseButtonDown (1)) {
				SpellEnd ();
			}
		}

        //================================= SACRIFICE ===============================================

        if (sacrificeActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D clickedCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), layerMask);

                if (clickedCollider != null)
                {
                    if (clickedCollider.gameObject.CompareTag("enemy") && !cannotCast && clickedCollider.gameObject.GetComponent<guardPatrol>() != null && clickedCollider.gameObject.GetComponent<guardPatrol>().weak)
                    {
                        Debug.Log("zap");
                        gameObject.GetComponent<undeadHealthManager>().unitHealth = 1;
                        clickedCollider.GetComponent<guardPatrol>().Instakill();
                        
                        SpellEnd();
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                SpellEnd();
            }
        }

	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
	}

	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ BOOK OF MAGIC, PT. 2 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

	public void RaiseDead () {
		SpellEnd ();
		if (mana >= raiseDeadCost && !spellActive) {
			raiseDeadActive = true;
			spellActive = true;
			currentSpellCost = raiseDeadCost;
			var magicCirclePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			magicCirclePos.z = 1;
			magicMouse = Instantiate (magicMouseSprite, magicCirclePos, Quaternion.identity);
			magicMouse.GetComponent<uiPosition> ().spellRange = raiseDeadRange;
		}

		if (mana < raiseDeadCost) {
			manaWarning.SetActive (true);
		}
	}

	public void ConsumeCorpse () {
		SpellEnd ();
		if (!spellActive) {
			consumeCorpseActive = true;
			spellActive = true;
			currentSpellCost = 0;
			var magicCirclePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			magicCirclePos.z = 1;
			magicMouse = Instantiate (magicMouseSprite, magicCirclePos, Quaternion.identity);
			magicMouse.GetComponent<uiPosition> ().spellRange = consumeCorpseRange;
		}
	}

	public void Heal () {
		SpellEnd ();
		if (mana >= healCost && !spellActive) {
			healActive = true;
			spellActive = true;
			currentSpellCost = healCost;
			var magicCirclePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			magicCirclePos.z = 1;
			magicMouse = Instantiate (magicMouseSprite, magicCirclePos, Quaternion.identity);
			magicMouse.GetComponent<uiPosition> ().spellRange = healRange;
		}

		if (mana < healCost) {
			manaWarning.SetActive (true);
		}
	}

    public void Sacrifice()
    {
        Debug.Log("pow");
        SpellEnd();
        if (gameObject.GetComponent<undeadHealthManager>().unitHealth > 1)
        {
            sacrificeActive = true;
            spellActive = true;
            currentSpellCost = 0;

            var magicCirclePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            magicCirclePos.z = 1;
            magicMouse = Instantiate(magicMouseSprite, magicCirclePos, Quaternion.identity);
            magicMouse.GetComponent<uiPosition>().spellRange = sacrificeRange;
        }

    }

	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

	public void SpellEnd () {
		spellActive = false;

		//----- ADD ALL SPELLS HERE ------------
		raiseDeadActive = false;
		consumeCorpseActive = false;
		healActive = false;
        sacrificeActive = false;
		//--------------------------------------

		manaCount.text = "Mana: " + mana;
		Destroy (magicMouse);
        canBeEnded = false;
	}

    float CalculateRaise()
    {
        return (raiseDeadCooldown / raiseDeadCooldownMax);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("corpseParticleCollider"))
        {
            other.GetComponentInParent<corpse>().Reanimate();
        }

    }

    }
