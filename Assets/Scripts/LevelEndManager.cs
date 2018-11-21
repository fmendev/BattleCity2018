using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Outcome { Victory, Defeat };

public class LevelEndManager : MonoBehaviour
{
    private static LevelEndManager singletonInstance;

    public GameObject levelEndScreen;

    private GameObject outcome;
    private GameObject killValueText;
    private GameObject timeValueText;
    private GameObject livesValueText;
    private GameObject fadeInPanel;

    private DateTime timeStart;
    private DateTime timeFinished;
    private TimeSpan timeElapsed;

    private int kills = 0;
    private int livesLeft;

    float alphaDelta = .005f;

    private void Awake()
    {
        InitializeSingleton();
        timeStart = DateTime.Now;

        outcome = levelEndScreen.transform.GetChild(0).gameObject;
        killValueText = levelEndScreen.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        timeValueText = levelEndScreen.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        livesValueText = levelEndScreen.transform.GetChild(1).GetChild(1).GetChild(2).gameObject;
        fadeInPanel = levelEndScreen.transform.GetChild(3).gameObject;
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
        PauseManager.FreezeDynamicObjects();

        singletonInstance.timeFinished = DateTime.Now;
        singletonInstance.timeElapsed = singletonInstance.timeFinished.Subtract(singletonInstance.timeStart);
        string time = singletonInstance.timeElapsed.ToString();

        string outcome = null;
        if (o == Outcome.Victory) outcome = "Victory";
        else if (o == Outcome.Defeat) outcome = "Defeat";

        singletonInstance.outcome.GetComponent<TextMeshProUGUI>().text = outcome;
        singletonInstance.killValueText.GetComponent<TextMeshProUGUI>().text = singletonInstance.kills.ToString();
        singletonInstance.timeValueText.GetComponent<TextMeshProUGUI>().text = time;
        singletonInstance.livesValueText.GetComponent<TextMeshProUGUI>().text = LivesController.GetCurrentLives().ToString();

        singletonInstance.StartCoroutine("BeginLevelEndAnimation");
    }

    private IEnumerator BeginLevelEndAnimation()
    {
        yield return new WaitForSeconds(1f);

        fadeInPanel.gameObject.SetActive(true);
        levelEndScreen.gameObject.SetActive(true);
        Color color = fadeInPanel.GetComponent<Image>().color;

        while (color.a <= 1)
        {
            float alpha = fadeInPanel.GetComponent<Image>().color.a;
            Color newColor = new Color(0, 0, 0, alpha - alphaDelta);
            fadeInPanel.GetComponent<Image>().color = newColor;
            yield return new WaitForEndOfFrame();
        }
    }

    public static void IncreaseKillCount()
    {
        singletonInstance.kills++;

        if (singletonInstance.kills == LevelManager.GetEnemyTankList().Count)
        {
            ShowLevelEndScreen(Outcome.Victory);
        }
    }

}
