using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyTankParentObject;
    private EnemyType enemyTypeSpawning;

    public void SpawnEnemy(EnemyType type)
    {
        Debug.Log("Spawn animation started");
        enemyTypeSpawning = type; 
        GetComponent<Animator>().SetBool("isSpawning", true);
        //After spawn animation completes, it calls ActivatePlayer via event. This brings the player on scene and resets the trigger
    }

    private void ActivateEnemy()
    {
        GameObject enemy = Instantiate(TankFactory.GetEnemyTank(enemyTypeSpawning), enemyTankParentObject.transform);

        //EnemySpawnerController.IncreaseEnemiesSpawnedCounter();
        enemy.transform.position = gameObject.transform.position;
        GetComponent<Animator>().SetBool("isSpawning", false);
        Debug.Log("Enemy tank created");
    }
}
