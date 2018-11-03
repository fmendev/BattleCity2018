using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroTank : MonoBehaviour {

    public Texture2D introTank;
    public GameObject flashPanel;
    public GameObject mainMenuPanel;

    private Color color;
    private bool flashing = false;
    private bool optionsFadingIn = false;

    void Start ()
    {
        gameObject.GetComponent<RawImage>().enabled = false;

        for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
        {
            color = new Color(255, 255, 255, 0);
            mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color = color;
        }

        color = new Color(255, 255, 255, 1);
        StartCoroutine("IntroAnimation");
    }

    private IEnumerator IntroAnimation()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.GetComponent<RawImage>().enabled = true;

        flashPanel.SetActive(true);
        flashing = true;
        flashPanel.GetComponent<RawImage>().color = color;
    }

    void Update()
    {
        if (flashing)
        {
            float alpha = flashPanel.GetComponent<RawImage>().color.a;
            color = new Color(255, 255, 255, alpha - .01f);
            flashPanel.GetComponent<RawImage>().color = color;

            if (color.a <= 0)
            {
                color = new Color(255, 255, 255, 1);
                flashPanel.GetComponent<RawImage>().color = color;
                flashing = false;
                flashPanel.SetActive(false);
                optionsFadingIn = true;
            }
        }

        if (optionsFadingIn)
        {
            for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
            {
                float alpha = mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color.a;
                color = new Color(255, 255, 255, alpha + .01f);
                mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color = color;

                if (color.a >= 255)
                {
                    color = new Color(255, 255, 255, 1);
                    mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color = color;
                    optionsFadingIn = false;
                }
            }
        }
    }
}
