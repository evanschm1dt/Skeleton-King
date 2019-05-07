using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controller : MonoBehaviour {

	[HideInInspector]
	public List <GameObject> selectedSkeletons;

	[HideInInspector]
	public List<GameObject> selectableObjects; 

	[HideInInspector]
	public GameObject boss;

	[HideInInspector]
	public bool isCasting = false;

	private Vector3 mousePos1;
	private Vector3 mousePos2;

	public GameObject destinationMarker;
	public GameObject targetMarker;

	[HideInInspector]
	public bool bossIsDead;

	[HideInInspector]
	public bool youWin;

    public Image fadeToBlack;
    [HideInInspector]
    public bool fadeOut;
    public float fadeInTime;
    public float fadeOutTime;

    public GameObject deathText;
	public GameObject winText;

    public LayerMask layerMask;

    [HideInInspector]
    public bool paused;

    public GameObject pauseMenu;

	//[HideInInspector]
	//public GameObject targetCorpse;

	void Awake() {
		selectedSkeletons = new List<GameObject>();
		selectableObjects = new List<GameObject> ();

        fadeToBlack.gameObject.SetActive(true);
    }
	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("necromancer");
        paused = false;

        //fadeToBlack = GameObject.Find("fadeToBlack").GetComponent<Image>();
        

        Application.targetFrameRate = 60;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }

		/*if (boss.GetComponent<undeadHealthManager> ().unitHealth <= 0) {
			bossIsDead = true;
		}*/

        if (!fadeOut)
        {
            fadeToBlack.CrossFadeAlpha(0, fadeInTime, false);
        } else
        {
            fadeToBlack.CrossFadeAlpha(1, fadeOutTime, false);
        }

		if (bossIsDead) {
			deathText.SetActive (true);
		}

		if (youWin) {
			winText.SetActive (true);
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Scene loadedLevel = SceneManager.GetActiveScene();
			SceneManager.LoadScene (loadedLevel.buildIndex);
		}

		//------------------------------- LEFT CLICK STUFF -----------------------------------------

		if (Input.GetMouseButtonDown (0)) {
			
			if (!isCasting) {
				mousePos1 = Camera.main.ScreenToViewportPoint (Input.mousePosition);

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (!Input.GetKey (KeyCode.LeftControl)) {
					EmptySkeleList ();
				}

				//if (Physics.Raycast (ray, out hit)) {
				Collider2D clickedCollider = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition), layerMask);

				if (clickedCollider != null) {

					if (clickedCollider.gameObject.CompareTag("skeleton") && clickedCollider.gameObject != boss) {
					
						undeadMain skeleScript = clickedCollider.GetComponent<undeadMain> ();
						//Debug.Log ("click!");
						//skeleScript.ClickedOn ();
						if (Input.GetKey (KeyCode.LeftControl)) {
							if (skeleScript.isSelected == false) {
								selectedSkeletons.Add (clickedCollider.gameObject);
								skeleScript.isSelected = true;
								skeleScript.ClickedOn ();
							} else {
								selectedSkeletons.Remove (clickedCollider.gameObject);
								skeleScript.isSelected = false;
								skeleScript.ClickedOn ();
							}
						} else {
							/*if (selectedSkeletons.Count > 0) {
							foreach (GameObject obj in selectedSkeletons) {
								obj.GetComponent<undeadMain> ().isSelected = false;
								obj.GetComponent<undeadMain> ().ClickedOn ();
							}
							selectedSkeletons.Clear ();
						}*/
							EmptySkeleList ();
							selectedSkeletons.Add (clickedCollider.gameObject);
							skeleScript.isSelected = true;
							skeleScript.ClickedOn ();
						}
					} else if (clickedCollider.gameObject.tag != "skeleton") {
						EmptySkeleList ();
					}
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {

			if (!isCasting) {
				mousePos2 = Camera.main.ScreenToViewportPoint (Input.mousePosition);

				if (mousePos1 != mousePos2) {
					SelectObjects ();
				}
			}
		}

		//---------------------------------- RIGHT CLICK STUFF -------------------------------------------------

		if (Input.GetMouseButtonDown (1)) {

			if (!isCasting) {
				
				if (selectedSkeletons.Count > 0) {
					
					var destinationSpawn = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					destinationSpawn.z = -5;

					/*foreach (GameObject obj in selectedSkeletons) {
						obj.GetComponent<undeadMain> ().SetDestination ();
					}*/

					foreach (GameObject undead in selectedSkeletons) {
                        if (undead != null)
                        {
                            undead.GetComponent<undeadMain>().target = null;
                            undead.GetComponent<undeadMain>().targetingMode = false;
                        }
					}

					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;

					Collider2D clickedCollider = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition), layerMask);

					if (clickedCollider != null) {

						if (clickedCollider.gameObject.CompareTag ("corpse")) {
							selectedSkeletons [0].GetComponent<corpseInteraction> ().targetCorpse = clickedCollider.gameObject;
							destinationSpawn.y += .5f;
							GameObject targetArrow = Instantiate (targetMarker, destinationSpawn, Quaternion.identity);
							targetArrow.transform.SetParent (clickedCollider.transform);
							//Debug.Log ("a corpse!");
						} else if (clickedCollider.gameObject.CompareTag ("enemy")) {
							foreach (GameObject undead in selectedSkeletons) {
                                if (undead != null)
                                {
                                    undead.GetComponent<undeadMain>().target = clickedCollider.gameObject;
                                    undead.GetComponent<undeadMain>().targetingMode = true;
                                }
							}
							destinationSpawn.y += .5f;
							GameObject targetArrow = Instantiate (targetMarker, destinationSpawn, Quaternion.identity);
							targetArrow.transform.SetParent (clickedCollider.transform);
						} else if (clickedCollider.gameObject == boss) {
							foreach (GameObject undead in selectedSkeletons) {
                                if (undead != null)
                                {
                                    undead.GetComponent<undeadMain>().target = clickedCollider.gameObject;
                                    undead.GetComponent<undeadMain>().targetingMode = true;
                                    if (undead.GetComponent<corpseInteraction>().hasCorpse)
                                    {
                                        undead.GetComponent<corpseInteraction>().deliverCorpse = true;
                                    }
                                }
							}
							destinationSpawn.y += .5f;
							GameObject targetArrow = Instantiate (targetMarker, destinationSpawn, Quaternion.identity);
							targetArrow.transform.SetParent (clickedCollider.transform);
						} else {
							Instantiate (destinationMarker, destinationSpawn, Quaternion.identity);
							foreach (GameObject obj in selectedSkeletons) {
								obj.GetComponent<undeadMain> ().SetDestination ();
							}
						}
					} else {
						Instantiate (destinationMarker, destinationSpawn, Quaternion.identity);
						foreach (GameObject obj in selectedSkeletons) {
                            if (obj != null)
                            {
                                obj.GetComponent<undeadMain>().SetDestination();
                            }
						}
					}
				}
			}
				
		}
	}

    //---- THIS IS PART OF THE BODYGUARD SYSTEM.  THIS IS TRIGGERED FROM GUARDPATROL, WITH "AGGRESSOR" BEING THE GUARD IN QUESTION -----

	public void alertUndead (GameObject aggressor) {
		foreach (GameObject undead in selectableObjects) {
            if (undead != null)
            {
                undead.GetComponent<undeadMain>().DefendYourKing(aggressor);
            }
		}
	}

	void SelectObjects(){
		List<GameObject> remObjects = new List <GameObject> ();

		if (Input.GetKey (KeyCode.LeftControl)) {
			EmptySkeleList ();
		}

		Rect selectRect = new Rect (mousePos1.x, mousePos1.y, mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);

		foreach (GameObject selectObject in selectableObjects) {
			if (selectObject != null) {
				if (selectRect.Contains (Camera.main.WorldToViewportPoint (selectObject.transform.position), true)) {
					selectedSkeletons.Add (selectObject);
					selectObject.GetComponent<undeadMain> ().isSelected = true;
					selectObject.GetComponent<undeadMain> ().ClickedOn ();
				}
			} else {
				remObjects.Add (selectObject);
			}
		}

		if (remObjects.Count > 0) {
			foreach (GameObject rem in remObjects) {
				selectableObjects.Remove (rem);
			}
			remObjects.Clear ();
		}
	}

    public void ReturnToMain()
    {
        SceneManager.LoadScene("titleScreen");
    }

	public void EmptySkeleList (){
		//Debug.Log ("blep");
		if (selectedSkeletons.Count > 0) {
			foreach (GameObject obj in selectedSkeletons) {
                if (obj != null)
                {
                    obj.GetComponent<undeadMain>().isSelected = false;
                    obj.GetComponent<undeadMain>().ClickedOn();
                }
			}
			selectedSkeletons.Clear ();
		}
	}

}
