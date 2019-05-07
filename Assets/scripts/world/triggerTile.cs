using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class triggerTile : MonoBehaviour
{

    public GameObject papa;

    private SpriteRenderer spriteRend;
    public Sprite tileUp;
    public Sprite tileDown;

    public bool permaPress;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("skeleton") && papa != null)
        {

            spriteRend.sprite = tileDown;

            if (papa.CompareTag("door"))
            {
                foreach (Transform child in papa.transform)
                {
                    child.GetComponent<doorPart>().CreateReplacement();
                }

                Destroy(papa);
                papa = null;
                AstarPath.active.Scan();
                return;
            }

            if (papa.GetComponent<arrowWall>())
            {
                papa.GetComponent<arrowWall>().active = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("skeleton") && papa != null)
        {
            if (!permaPress)
            {
                spriteRend.sprite = tileUp;
            }

            if (papa.GetComponent<arrowWall>())
            {
                papa.GetComponent<arrowWall>().active = false;
            }
        }
    }
}
