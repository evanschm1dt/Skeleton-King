using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneLoader : MonoBehaviour {

    public bool mainMenu;

    public string[] levels;

    public Image fadeToBlack;

    public float fadeInTime;
    public float fadeOutTime;

    public float endTimer;

    bool ending;

    string levelToLoad;

	// Use this for initialization
    void Awake()
    {
        if (mainMenu)
        {
            fadeToBlack.gameObject.SetActive(true);
        }
    }

	void Start () {
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {

        if (mainMenu)
        {
            if (!ending)
            {
                fadeToBlack.CrossFadeAlpha(0, fadeInTime, false);
            } else
            {
                fadeToBlack.CrossFadeAlpha(1, fadeOutTime, false);
                endTimer -= Time.deltaTime;
                if (endTimer <= 0)
                {
                    SceneManager.LoadScene(levelToLoad);
                }
            }
        }
        
		if (Input.GetKeyDown(KeyCode.Escape) && mainMenu)
        {
            Application.Quit();
        }
	}

	public void LoadLevel0(){
        //SceneManager.LoadScene (levels[0]);
        levelToLoad = levels[0];
        ending = true;
	}

	public void LoadLevel1(){
        levelToLoad = levels[1];
        ending = true;
    }

    public void LoadLevel2()
    {
        levelToLoad = levels[2];
        ending = true;
    }

    public void LoadStartMenu(){
		SceneManager.LoadScene ("titleScreen");
	}

	public void RestartScene(){
		//SceneManager.LoadScene (0);
		Scene loadedLevel = SceneManager.GetActiveScene();
		SceneManager.LoadScene (loadedLevel.buildIndex);
	}
}
