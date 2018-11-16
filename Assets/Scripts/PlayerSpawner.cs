using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner singletonInstance;

    private void Awake()
    {
        InitializeSingleton();   
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
        GameObject player = Instantiate(TankFactory.GetPlayer());
        player.transform.position = gameObject.transform.position;
        GetComponent<Animator>().SetBool("isSpawning", false);
        PlayerController.RefreshPlayerController();
    }
}
