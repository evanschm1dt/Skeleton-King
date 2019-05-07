using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossArenaFlameWalls : MonoBehaviour
{

    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss != null && boss.GetComponentInChildren<fireKnight>())
        {
            if (boss.GetComponentInChildren<fireKnight>().fighting && gameObject.GetComponent<flameWall>().active == false)
            {
                
                gameObject.GetComponent<flameWall>().active = true;
            }
        }

        if (boss == null)
        {
            gameObject.GetComponent<flameWall>().active = false;
        }
    }
}
