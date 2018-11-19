using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager singletonInstance;

    public GameObject enemyTankParentObject;
    public GameObject logicController;

    private GameObject ammoPool;
    private bool isFrozen = false;

    private List<Vector2> projectileVelocitiesAtFreeze;
    private List<Vector2> tankVelocitiesAtFreeze;

    private void Awake()
    {
        InitializeSingleton();

        projectileVelocitiesAtFreeze = new List<Vector2>();
        tankVelocitiesAtFreeze = new List<Vector2>();
    }

    private void Start()
    {
        ammoPool = GameObject.FindGameObjectWithTag("AmmoPool");
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void FreezeDynamicObjects()
    {
        Debug.Log("Freeze called");
        singletonInstance.StartCoroutine("BeginFreezingDynamicObjects");
        singletonInstance.isFrozen = true;
    }

    private IEnumerator BeginFreezingDynamicObjects()
    {
        //Pause enemy spawning
        EnemySpawnerController.PauseEnemySpawning();

        //Freeze bullets
        for (int i = 0; i < ammoPool.transform.childCount; i++)
        {
            if (ammoPool.transform.GetChild(i).gameObject.activeSelf)
            {
                Vector2 velocity = ammoPool.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity;
                projectileVelocitiesAtFreeze.Add(velocity);

                ammoPool.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }

        //Deactivate player controller
        logicController.GetComponent<PlayerController>().enabled = false;
        logicController.GetComponent<WeaponsController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;

        //Deactivate enemy AI
        do
        {
            tankVelocitiesAtFreeze.Clear();
            for (int i = 0; i < enemyTankParentObject.transform.childCount; i++)
            {
                enemyTankParentObject.transform.GetChild(i).GetComponent<EnemyAI>().PauseAI();
                enemyTankParentObject.transform.GetChild(i).GetComponent<Animator>().enabled = false;
                enemyTankParentObject.transform.GetChild(i).GetComponent<EnemyAI>().enabled = false;

                Vector2 velocity = enemyTankParentObject.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity;
                tankVelocitiesAtFreeze.Add(velocity);
                enemyTankParentObject.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            yield return new WaitForFixedUpdate();

        } while (isFrozen);
    }

    public static void UnfreezeDynamicObjects()
    {
        Debug.Log("Unfreeze called");

        singletonInstance.isFrozen = false;

        //Resume enemy spawning
        EnemySpawnerController.ResumeEnemySpawning();
        //Unfreeze bullets
        for (int i = 0; i < singletonInstance.ammoPool.transform.childCount; i++)
        {
            if (singletonInstance.ammoPool.transform.GetChild(i).gameObject.activeSelf)
            {
                singletonInstance.ammoPool.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = singletonInstance.projectileVelocitiesAtFreeze[i];
            }
        }

        //Reactivate player controller
        singletonInstance.logicController.GetComponent<PlayerController>().enabled = true;
        singletonInstance.logicController.GetComponent<WeaponsController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = true;


        //Reactivate enemy AI
        for (int i = 0; i < singletonInstance.enemyTankParentObject.transform.childCount; i++)
        {
            singletonInstance.enemyTankParentObject.transform.GetChild(i).GetComponent<EnemyAI>().enabled = true;
            singletonInstance.enemyTankParentObject.transform.GetChild(i).GetComponent<Animator>().enabled = true;
            singletonInstance.enemyTankParentObject.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = singletonInstance.tankVelocitiesAtFreeze[i];
            singletonInstance.enemyTankParentObject.transform.GetChild(i).GetComponent<EnemyAI>().ResumeAI();
        }
    }
}
