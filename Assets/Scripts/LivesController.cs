using UnityEngine;
using UnityEngine.UI;

public class LivesController : MonoBehaviour {

    private static LivesController singletonInstance;

    public GameObject playerObject;
    public GameObject livesDisplay;

    private int currentLives = 3;
    private int livesCap = 5;

    void Awake()
    {
        InitializeSingleton();
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
        singletonInstance.UpdateLivesDisplay();

        if (singletonInstance.currentLives > 0)
        {
            PlayerSpawner.SpawnPlayer();
        }
        else if (singletonInstance.currentLives == 0)
        {
            LevelEndManager.ShowLevelEndScreen(Outcome.Defeat);
            SoundManager.FadeOutMusic(3f);
        }
    }
}
