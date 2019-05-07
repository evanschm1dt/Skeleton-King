using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class textManager : MonoBehaviour
{
    public Image fadeToBlack;
    public float fadeTime;

    public float textColorMin;
    public float textColorMax;

    public float flickerSpeedMin;
    public float flickerSpeedMax;
    float flickerSpeed;

    float textColorGoal;
    float textColor;

    int textIndex = 0;

    float textAppearTimer;
    public float textAppearTimerBase;

    public float endTimer;
    float nextSceneTimer;

    bool goingDown = true;

    public TextMeshProUGUI[] texts;

    public System.String levelName;

    // Start is called before the first frame update


    void Start()
    {
        textColorGoal = Random.Range(textColorMin, textColorMax);
        flickerSpeed = Random.Range(flickerSpeedMin, flickerSpeedMax);
        textColor = 1;

        textAppearTimer = textAppearTimerBase;

        nextSceneTimer = fadeTime + 2.5f;

        fadeToBlack.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(fadeToBlack.color.a);
        if (endTimer > 0)
        {
            fadeToBlack.CrossFadeAlpha(0, 0, false);
        }

        

        textAppearTimer -= Time.deltaTime;

        if (textAppearTimer <= 0 && textIndex < texts.Length)
        {
            texts[textIndex].gameObject.SetActive(true);
            textAppearTimer = textAppearTimerBase;
            textIndex += 1;
        }

        if (textIndex >= texts.Length)
        {
            if (endTimer > 0)
            {
                endTimer -= Time.deltaTime;
                
            }
            if (endTimer <= 0)
            {
                
                fadeToBlack.CrossFadeAlpha(1, fadeTime, false);
                nextSceneTimer -= Time.deltaTime;

                if (nextSceneTimer <= 0)
                {
                    SceneManager.LoadScene(levelName);
                }
            }
        }

        if (goingDown)
        {
            textColor -= flickerSpeed * Time.deltaTime;
            if (textColor <= textColorGoal)
            {
                goingDown = false;
                textColorGoal = Random.Range(textColorMin, textColorMax);
                flickerSpeed = Random.Range(flickerSpeedMin, flickerSpeedMax);
            }
        }
        else
        {
            textColor += flickerSpeed * Time.deltaTime;
            if (textColor >= 1)
            {
                goingDown = true;
                flickerSpeed = Random.Range(flickerSpeedMin, flickerSpeedMax);
            }
        }

        textColor = Mathf.Clamp(textColor, 0, 1);

        foreach (TextMeshProUGUI text in texts)
        {
            text.color = new Color32(((byte) (textColor * 255)), (byte)(textColor * 255), (byte)(textColor * 255), 255);
        }
    }
}
