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
    [SerializeField]
    private float playerRespawnDelay;

    private int livesCap = 5;

    private void Awake()
    {
        InitializeSingleton();

        currentLives = 3;
        playerRespawnDelay = 1f;

        UpdateLivesDisplay();
    }

    private void Start()
    {
        PlayerSpawner.SpawnPlayer();
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

    public static int GetCurrentLives()
    {
        return singletonInstance.currentLives;
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
            PlayerSpawner.SpawnPlayer();
            ArmorController.IncreaseArmor();
        }
        else if (singletonInstance.currentLives == 0)
        {
            Animator gameOverAnim = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Animator>();
            gameOverAnim.SetBool("isEagleDestroyed", true);
        }
    }
}
