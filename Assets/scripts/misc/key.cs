using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{

    public GameObject door;

    public AudioClip pickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("skeleton") && other.gameObject.name == "necromancer")
        {
            door.GetComponent<keyManager>().keysToFind -= 1;
            door.GetComponent<keyManager>().keysFound += 1;
            door.GetComponent<keyManager>().RewriteText();
            other.GetComponentInChildren<AudioSource>().PlayOneShot(pickup);
            Destroy(gameObject);
        }
    }
}
