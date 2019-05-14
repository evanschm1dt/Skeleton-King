using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	public Camera mainCam;

	public float movementSpeed;

    public float lerpSpeed;

	float mouseMovementSpeed = 30f;

	public float sizeChanger = 5;
    float zoomSpeed = .1f;

    public bool extraControls;

	 Vector3 camPos;

     Vector3 lastPos;

     Vector3 nextPos;

	[HideInInspector]
	public bool attached;

	[HideInInspector]
	public GameObject manager;

	//[HideInInspector]
	public GameObject necromancer;

    public bool noNecromancer;

	[HideInInspector]
	public bool gameOver = false;

	// Use this for initialization
	void Start () {
        if (!noNecromancer)
        {
            necromancer = GameObject.Find("necromancer");
        }
		manager = GameObject.Find("manager");

		camPos = transform.position;
		camPos.x = necromancer.transform.position.x;
		camPos.y = necromancer.transform.position.y;
		transform.position = camPos;
		//transform.SetParent (necromancer.transform);
		attached = true;
	}

	// Update is called once per frame
	void Update () {
        if (!gameOver && necromancer != null)
        {
            lastPos = transform.position;

            nextPos.x = necromancer.transform.position.x;
            nextPos.y = necromancer.transform.position.y;
            nextPos.z = transform.position.z;
        }

        //---Middle mouse drag----------
        /*if (Input.GetMouseButton (2)) {
			var pos = transform.position;
			pos.x -= Input.GetAxis ("Mouse X") * mouseMovementSpeed * Time.deltaTime;
			pos.y -= Input.GetAxis ("Mouse Y") * mouseMovementSpeed * Time.deltaTime;
			transform.position = pos;
		}*/
        /*
		mainCam.orthographicSize = sizeChanger;

		//Debug.Log (mainCam.orthographicSize);
		Mathf.Clamp (sizeChanger, 5, 10);

		if (Input.GetKey (KeyCode.Q)) {
			sizeChanger += .1f;
		}
		if (Input.GetKey (KeyCode.E)) {
			sizeChanger -= .1f;
		}

		*/
        if (extraControls)
        {
            mainCam.orthographicSize = sizeChanger;

            if (Input.GetKey(KeyCode.K))
            {
                sizeChanger += zoomSpeed;
            }
            if (Input.GetKey(KeyCode.L))
            {
                sizeChanger -= zoomSpeed;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                zoomSpeed += .01f;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                zoomSpeed -= .01f;
            }
            if (Input.GetKey(KeyCode.J))
            {
                sizeChanger = 5;
            }
        }


        if (!noNecromancer)
        {
            if (necromancer != null && necromancer.GetComponent<necromancerMain>().scrying)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
                }

                if (attached)
                {
                    unParent();
                }
            }
            else
            {
                transform.position = Vector3.Lerp(lastPos, nextPos, lerpSpeed * Time.deltaTime);

                if (!attached && !gameOver)
                {
                    //reParent ();
                }
            }
        }
	}

	public void unParent (){
		transform.parent = null;
		attached = false;
	}

	public void reParent () {
		camPos.x = necromancer.transform.position.x;
		camPos.y = necromancer.transform.position.y;
		transform.position = camPos;
		transform.SetParent (necromancer.transform);
		attached = true;
	}
}
