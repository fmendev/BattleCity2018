using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Outcome { Victory, Defeat };

public class LevelEndManager : MonoBehaviour
{
    private static LevelEndManager singletonInstance;

    public GameObject levelEndScreen;
    public GameObject logicController;

    private GameObject outcome;
    private GameObject killValueText;
    private GameObject timeValueText;
    private GameObject livesValueText;
    private GameObject options;
    private GameObject optionText;
    private GameObject fadeInPanel;

    private DateTime timeStart;
    private DateTime timeFinished;
    private TimeSpan timeElapsed;

    private int kills = 0;
    private int livesLeft;

    private bool levelWon;

    float alphaDelta = .005f;

    private void Awake()
    {
        InitializeSingleton();
        timeStart = DateTime.Now;

        outcome = levelEndScreen.transform.GetChild(0).gameObject;
        killValueText = levelEndScreen.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        timeValueText = levelEndScreen.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        livesValueText = levelEndScreen.transform.GetChild(1).GetChild(1).GetChild(2).gameObject;
        options = levelEndScreen.transform.GetChild(2).gameObject;
        optionText = levelEndScreen.transform.GetChild(2).GetChild(1).GetChild(0).gameObject;
        fadeInPanel = levelEndScreen.transform.GetChild(3).gameObject;

        fadeInPanel.GetComponent<Image>().color = Color.black;
    }

    private void Start()
    {
        levelEndScreen.gameObject.SetActive(false);
        fadeInPanel.gameObject.SetActive(false);
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void ShowLevelEndScreen(Outcome o)
    {
        singletonInstance.timeFinished = DateTime.Now;
        singletonInstance.timeElapsed = singletonInstance.timeFinished.Subtract(singletonInstance.timeStart);
        string time = singletonInstance.timeElapsed.ToString();

        string outcome = null;
        string option = null;

        if (o == Outcome.Victory)
        {
            outcome = "Victory";
            option = "Continue";
            singletonInstance.levelWon = true;
        }

        else if (o == Outcome.Defeat)
        {
            outcome = "Defeat";
            option = "Replay";
            singletonInstance.levelWon = false;
        }

        singletonInstance.outcome.GetComponent<TextMeshProUGUI>().text = outcome;
        singletonInstance.killValueText.GetComponent<TextMeshProUGUI>().text = singletonInstance.kills.ToString();
        singletonInstance.timeValueText.GetComponent<TextMeshProUGUI>().text = time;
        singletonInstance.livesValueText.GetComponent<TextMeshProUGUI>().text = LivesController.GetCurrentLives().ToString();
        singletonInstance.optionText.GetComponent<Text>().text = option;

        singletonInstance.StartCoroutine("BeginLevelEndAnimation");
    }

    public static void ContinueReplay()
    {
        singletonInstance.StartCoroutine("ContinueReplayLevelEndAnimation");
    }

    public static void IncreaseKillCount()
    {
        singletonInstance.kills++;

        if (singletonInstance.kills == LevelManager.GetEnemyTankList().Count)
        {
            ShowLevelEndScreen(Outcome.Victory);
            SoundManager.FadeOutMusic(1.5f);
            singletonInstance.logicController.GetComponent<PlayerController>().enabled = false;
            singletonInstance.logicController.GetComponent<WeaponsController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
        }
    }

    private IEnumerator ContinueReplayLevelEndAnimation()
    {
        fadeInPanel.gameObject.SetActive(true);
        options.transform.GetChild(0).GetComponent<Button>().interactable = false;
        options.transform.GetChild(1).GetComponent<Button>().interactable = false;

        Color newColor = fadeInPanel.GetComponent<Image>().color;

        while (newColor.a < 1)
        {
            float alpha = fadeInPanel.GetComponent<Image>().color.a;
            newColor = new Color(0, 0, 0, alpha + alphaDelta);
            fadeInPanel.GetComponent<Image>().color = newColor;
            yield return new WaitForEndOfFrame();
        }

            if (singletonInstance.levelWon)
            {
                if (LevelManager.GetCurrentLevel() == 2) //or whatever the final level is
                {
                    //End game screen!!!
                }
                else
                {
                    SceneManager.LoadScene(LevelManager.GetCurrentLevel() + 1);
                }
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
    }

    private IEnumerator BeginLevelEndAnimation()
    {
        bool isMusicOn = false; //so that it calls music only once

        yield return new WaitForSeconds(1.5f);

        fadeInPanel.gameObject.SetActive(true);
        levelEndScreen.gameObject.SetActive(true);
        Color color = fadeInPanel.GetComponent<Image>().color;

        while (color.a >= 0)
        {
            if (color.a >.8f && !isMusicOn)
            {
                if (levelWon)
                {
                    SoundManager.PlaySfx(SFX.Victory);
                }
                else
                {
                    SoundManager.FadeInMusic(Music.Defeat, 3);
                }

                isMusicOn = true;
            }
            float alpha = fadeInPanel.GetComponent<Image>().color.a;
            color = new Color(0, 0, 0, alpha - alphaDelta);
            fadeInPanel.GetComponent<Image>().color = color;
            yield return new WaitForEndOfFrame();
        }

        fadeInPanel.gameObject.SetActive(false);
    }
}
