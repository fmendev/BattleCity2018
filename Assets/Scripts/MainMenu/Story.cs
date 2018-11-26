using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    public GameObject frame;
    public GameObject mainMenuPanel;
    public GameObject title;
    public GameObject background;

    private GameObject storyPanel;
    private bool storyPanelFadingIn = false;
    private bool storyPanelFadingOut = false;
    private float alphaDeltaStoryPanel = .5f;

    private bool mainMenuFadingOut = false;
    private float alphaDeltaMainMenu = .25f;

    private GameObject storyTextPanel;
    private List<string> storyText;
    private float alphaDeltaStoryText = .75f;

    private bool cutsceneEscaped = false;
    private bool escapeCutsceneEnabled = false;

    void Start ()
    {
        storyPanel = frame.gameObject.transform.GetChild(0).gameObject;
        storyPanel.GetComponent<Image>().color = new Color(255, 255, 255, 0);

        storyTextPanel = frame.gameObject.transform.GetChild(5).gameObject;
        frame.gameObject.SetActive(false);

        storyText = new List<string>()
                        { "Brother",
                          "Our land, home of the Condor, is under attack by evil forces",
                          "You are the Libero, the last defender",
                          "Take arms! Protect your people, and save the land of the Condor!"
        };
    }
	
	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!cutsceneEscaped && escapeCutsceneEnabled)
            {
                StopAllCoroutines();
                SoundManager.Stop();
                storyPanelFadingOut = true;

                mainMenuFadingOut = false;
                storyPanelFadingIn = false;
                cutsceneEscaped = true;
            }
        }

        if (mainMenuFadingOut)
        {
            for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
            {
                float alpha = mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color.a;
                Color optionsColor = mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color;

                optionsColor.a = alpha - Time.deltaTime * alphaDeltaMainMenu;

                mainMenuPanel.transform.GetChild(i).GetComponentInChildren<Text>().color = optionsColor;
                title.GetComponent<RawImage>().color = new Color(255, 255, 255, alpha - .05f);
                background.GetComponent<RawImage>().color = new Color(255, 255, 255, alpha - Time.deltaTime * alphaDeltaMainMenu);

                if (optionsColor.a <= 0)
                {
                    mainMenuFadingOut = false;
                    storyPanelFadingIn = true;
                    frame.gameObject.SetActive(true);
                    storyTextPanel.SetActive(false);
                    SoundManager.PlayMusic(Music.HowlingWind);
                }
            }
        }

        if (storyPanelFadingIn)
        {
            float alpha = storyPanel.GetComponent<Image>().color.a;
            Color storyPanelColor = new Color(255, 255, 255, alpha + Time.deltaTime * alphaDeltaStoryPanel);
            storyPanel.GetComponent<Image>().color = storyPanelColor;

            if (storyPanelColor.a >= 1)
            {
                storyPanelColor = new Color(255, 255, 255, 1);
                storyPanel.GetComponent<Image>().color = storyPanelColor;
                storyPanelFadingIn = false;

                storyTextPanel.SetActive(true);
                storyTextPanel.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0);
                StartCoroutine(BeginTextAnimation());
            }
        }

        if (storyPanelFadingOut)
        {
            float alpha = storyPanel.GetComponent<Image>().color.a;
            Color storyPanelColor = new Color(255, 255, 255, alpha - Time.deltaTime * alphaDeltaStoryPanel);
            storyPanel.GetComponent<Image>().color = storyPanelColor;

            if (storyPanelColor.a <= 0)
            {
                storyPanelColor = new Color(255, 255, 255, 0);
                storyPanel.GetComponent<Image>().color = storyPanelColor;
                storyPanelFadingOut = false;

                SceneManager.LoadScene(1);
            }
        }
    }

    public void BeginStoryAnimation()
    {
        escapeCutsceneEnabled = true;
        SoundManager.FadeOutMusic(3f);
        mainMenuFadingOut = true;
    }

    private IEnumerator BeginTextAnimation()
    {
        foreach (var line in storyText)
        {
            storyTextPanel.GetComponent<TextMeshProUGUI>().text = line;
            Color storyTextColor = storyTextPanel.GetComponent<TextMeshProUGUI>().color;

            while (storyTextColor.a <= 1)
            {
                float alphaText = storyTextPanel.GetComponent<TextMeshProUGUI>().color.a;
                storyTextColor = new Color(255, 255, 255, alphaText + Time.deltaTime * alphaDeltaStoryText);
                storyTextPanel.GetComponent<TextMeshProUGUI>().color = storyTextColor;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (storyTextColor.a >= 1)
            {
                yield return new WaitForSeconds(2.5f);
                storyTextColor = new Color(255, 255, 255, 0);
                storyTextPanel.GetComponent<TextMeshProUGUI>().color = storyTextColor;
            }
        }

        storyPanel.GetComponent<Animator>().SetBool("isScrolling", true);
        yield return new WaitForSeconds(15f);
        storyPanelFadingOut = true;
    }
}
