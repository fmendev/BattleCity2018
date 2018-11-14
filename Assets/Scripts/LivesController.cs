using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesController : MonoBehaviour {

    private static LivesController singletonInstance;

    public GameObject playerPrefab;
    public GameObject livesDisplay;

    [SerializeField]
    private int currentLives;

    private int livesCap = 5;

    private void Awake()
    {
        InitializeSingleton();

        currentLives = 3;

        UpdateLivesDisplay();
    }

    private void UpdateLivesDisplay()
    {
        for (int i = 0; i < currentLives; i++)
        {
            livesDisplay.transform.GetChild(i).GetComponent<RawImage>().color = Color.white;
        }

        for (int i = currentLives; i < livesCap; i++)
        {
            livesDisplay.transform.GetChild(i).GetComponent<RawImage>().color = Color.clear;
        }
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void IncreaseLives()
    {
        if (singletonInstance.currentLives < singletonInstance.livesCap)
        {
            singletonInstance.currentLives++;
            singletonInstance.UpdateLivesDisplay();
        }
    }

    public static void DecreaseLives()
    {
        singletonInstance.currentLives--;

        if (singletonInstance.currentLives > 0)
        {
            singletonInstance.UpdateLivesDisplay();
            //Instantiate()
        }
    }
}
