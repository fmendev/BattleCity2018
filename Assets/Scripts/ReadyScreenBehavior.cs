using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyScreenBehavior : MonoBehaviour
{
    //Typewrite effect
    private int currentCharIndex = 0;
    private float typewriterSpeed = 1f;

    //Fade out
    private bool fadingOut = false;
    private float alphaDeltaFadeOut = .04f;
    private Color textColor;

    //Blinking effect
    private float alphaDeltaBlinking = .02f;
    private bool continueBlinking = true;
    private bool blinkFadingIn = false;

    private bool isReady = false;

    void Awake()
    {
        GetComponent<Image>().enabled = true;
    }

    void Start()
    {
        GetComponent<Image>().enabled = true;
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + LevelManager.GetCurrentLevel().ToString();
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Ready";
        //gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "...";

        textColor = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;

        //StartCoroutine(AnimateReadyText());
        EnemySpawnerController.PauseEnemySpawning();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            fadingOut = true;
            isReady = true;
            LevelManager.SetPlayerReadyStatus(isReady);
            EnemySpawnerController.ResumeEnemySpawning();
        }

        if (fadingOut)
        {
            float alphaScreen = gameObject.GetComponent<Image>().color.a;
            float alphaText = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color.a;

            Color fadeOutScreenColor = new Color(0, 0, 0, alphaScreen - alphaDeltaFadeOut);

            textColor.a = alphaText - alphaDeltaFadeOut;

            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;
            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = textColor;
            gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = textColor;
            gameObject.GetComponent<Image>().color = fadeOutScreenColor;

            if (fadeOutScreenColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (continueBlinking)
        {
            float alpha = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color.a;
            Color blinkingColor = textColor;

            if (blinkFadingIn)
            {
                blinkingColor.a = alpha + alphaDeltaBlinking;
            }
            else
            {
                blinkingColor.a = alpha - alphaDeltaBlinking;
            }

            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = blinkingColor;
            gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = blinkingColor;

            if (blinkingColor.a >= 1)
            {
                blinkFadingIn = false;
            }
            else if (blinkingColor.a <= 0)
            {
                blinkFadingIn = true;
            }
        }
    }

    private IEnumerator AnimateReadyText()
    {
        while (!isReady)
        {
            string[] message = new string[] { "..." };
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

            for (int i = 0; i < (message[currentCharIndex].Length + 1); i++)
            {
                gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = message[currentCharIndex].Substring(0, i);
                yield return new WaitForSeconds(typewriterSpeed);
            }
        }
    }
}
