using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner singletonInstance;

    public GameObject playerTank;

    private void Awake()
    {
        InitializeSingleton();   
    }

    private void Start()
    {
        playerTank.gameObject.SetActive(false);
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void SpawnPlayer()
    {
        singletonInstance.GetComponent<Animator>().SetBool("isSpawning", true);
        //After spawn animation completes, it calls ActivatePlayer via event. This brings the player on scene and resets the trigger
    }

    private void ActivatePlayer()
    {
        playerTank.SetActive(true);
        playerTank.transform.position = transform.position;
        GetComponent<Animator>().SetBool("isSpawning", false);
    }
}
