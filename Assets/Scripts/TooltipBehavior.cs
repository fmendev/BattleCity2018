using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipBehavior : MonoBehaviour
{
    public GameObject flashPanel;
    public bool selfDestroys = false;

    private float tooltipCloseDelay = 2f;

    //Typewrite effect
    private int currentCharIndex = 0;
    private float typewriterSpeed = .02f;

    //Flash efect
    private float alphaDeltaFlash = .04f;
    private bool flashing = false;

    //Blinking effect
    private Color color;
    private float alphaDeltaBlinking = .02f;
    private bool continueBlinking = false;
    private bool fadingIn = false;

	private void Awake ()
    {
        flashing = true;

        color = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color;
        color.a = 0f;
    }

    private void Start()
    {
        StartCoroutine(AnimateMainText());
    }

    private void Update ()
    {
        if (flashing)
        {
            float alpha = flashPanel.GetComponent<RawImage>().color.a;
            Color flashColor = new Color(255, 255, 255, alpha - alphaDeltaFlash);
            flashPanel.GetComponent<RawImage>().color = flashColor;

            if (flashColor.a <= 0)
            {
                flashColor = new Color(255, 255, 255, 1);
                flashPanel.GetComponent<RawImage>().color = flashColor;

                flashing = false;
                flashPanel.SetActive(false);
                continueBlinking = true;
            }
        }

        if (continueBlinking)
        {
            float alpha = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color.a;
            Color blinkingColor = color;

            if (fadingIn)
            {
                blinkingColor.a = alpha + alphaDeltaBlinking;
            }
            else
            {
                blinkingColor.a = alpha - alphaDeltaBlinking;
            }

            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = blinkingColor;

            if (blinkingColor.a >= 1)
            {
                 fadingIn = false;
            }
            else if (blinkingColor.a <= 0)
            {
                fadingIn = true;
            }
        }

        if (!selfDestroys)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                PauseManager.UnfreezeDynamicObjects();
                Destroy(gameObject);
            }
        }
        else
        {
            StartCoroutine("CloseTooltip");
        }
    }

    private IEnumerator CloseTooltip()
    {
        yield return new WaitForSeconds(tooltipCloseDelay);
        Destroy(gameObject);
    }

    private IEnumerator AnimateMainText()
    {
        string[] message = new string[] { transform.GetChild(0).GetComponent<TextMeshProUGUI>().text };
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        for (int i = 0; i < (message[currentCharIndex].Length + 1); i++)
        {
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message[currentCharIndex].Substring(0, i);
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
}
