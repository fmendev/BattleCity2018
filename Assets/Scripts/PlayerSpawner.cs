using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner singletonInstance;

    public GameObject playerObject;
    public GameObject logicController;

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
        singletonInstance.StartCoroutine("BeginSpawningAnimation");
    }

    private IEnumerator BeginSpawningAnimation()
    {
        yield return new WaitForSeconds(2f);
        singletonInstance.GetComponent<Animator>().SetBool("isSpawning", true);
        //After spawn animation completes, it calls ActivatePlayer via event. This brings the player on scene and resets the trigger
    }

    private void ActivatePlayer()
    {
        Debug.Log("Activating");

        playerObject.transform.position = gameObject.transform.position;
        logicController.GetComponent<PlayerController>().enabled = true;
        logicController.GetComponent<WeaponsController>().enabled = true;
        playerObject.GetComponent<Animator>().enabled = true;

        ArmorController.SetMaxArmor(1);
        ArmorController.SetArmor(1);

        GetComponent<Animator>().SetBool("isSpawning", false);
    }
}
