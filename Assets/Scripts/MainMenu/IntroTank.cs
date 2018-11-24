using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroTank : MonoBehaviour
{
    public Texture2D introTank;
    public GameObject flashPanel;
    public GameObject mainMenuPanel;
    public GameObject background;
    public GameObject title;

    private Color color;
    private Color menuOrange = new Color(255, 199, 0, 1);
    private float alphaDelta = .005f;
    private float rgbDelta = .25f;
    private bool flashing = false;
    private bool optionsFadingIn = false;

    private bool introEscaped = false;

    private void Awake()
    {
        gameObject.GetComponent<RawImage>().enabled = false;

        for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
        {
            mainMenuPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    void Start ()
    {
        SoundManager.PlaySfx(SFX.IntroTankRolling);

        color = new Color(0, 0, 0, 255);
        background.GetComponent<RawImage>().color = color;

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

        SoundManager.PlaySfx(SFX.IntroTankFiring);
        title.gameObject.SetActive(true);

        flashPanel.SetActive(true);
        flashing = true;
        flashPanel.GetComponent<RawImage>().color = color;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!introEscaped)
            {
                StopAllCoroutines();
                SoundManager.Stop();
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                flashPanel.SetActive(false);
                flashing = false;
                optionsFadingIn = false;

                title.gameObject.SetActive(true);
                background.GetComponent<RawImage>().color = Color.white;

                for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
                {
                    mainMenuPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color = menuOrange;
                }

                SoundManager.PlayMusic(Music.TwinCannons);
                introEscaped = true;
            }
        }

        if (flashing)
        {
            float alpha = flashPanel.GetComponent<RawImage>().color.a;
            Color flashColor = new Color(255, 255, 255, alpha - alphaDelta);
            flashPanel.GetComponent<RawImage>().color = flashColor;

            float rgb = background.GetComponent<RawImage>().color.r;
            Color backgroundColor = new Color(rgb + rgbDelta, rgb + rgbDelta, rgb + rgbDelta, 255);
            background.GetComponent<RawImage>().color = backgroundColor;

            if (flashColor.a <= 0)
            {
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

                if (color.a >= 1)
                {
                    optionsFadingIn = false;
                    SoundManager.PlayMusic(Music.TwinCannons);

                    mainMenuPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void DestroyIntroTank()
    {
        Destroy(gameObject);
    }
}
