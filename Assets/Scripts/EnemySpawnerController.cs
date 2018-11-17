using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemySpawnerController : MonoBehaviour
{
    private static EnemySpawnerController singletonInstance;

    //public List<GameObject> enemySpawnPoints;
    //public List<GameObject> enemyTanks;

    //private int spawned = 0;
    //private float enemyRespawnTime = 5f;
    //private float spawnAnimationDuration = 1f;
    //private List<int> spawnLocationOrder;
    //private Vector3 enemyDisabledTankPosition = new Vector3(-60, 20, 0);
    //private Vector3 playerDisabledTankPosition = new Vector3(-10, 20, 0);
    //private Vector3 offset = new Vector3(0, -5, 0);

    private int currentSpawnPoint;
    private int currentEnemySpawning;
    private float spawningStartTime;
    private float spawningFrequencyRate;
    private List<EnemyType> tankSpawningOrder;

    private void Awake()
    {
        InitializeSingleton();

        currentSpawnPoint = 0;
        currentEnemySpawning = 0;
        spawningStartTime = 1f;
        spawningFrequencyRate = 4f;
    }

    private void Start()
    {
        tankSpawningOrder = LevelManager.GetEnemyTankList();

        InvokeRepeating("SpawnEnemy", spawningStartTime, spawningFrequencyRate);
    }

    private void SpawnEnemy()
    {
        transform.GetChild(currentSpawnPoint).GetComponent<EnemySpawner>().SpawnEnemy(tankSpawningOrder[currentEnemySpawning]);

        //Loop through available spawn points
        if (currentSpawnPoint >= transform.childCount - 1)
        {
            currentSpawnPoint = 0;
        }
        else
        {
            currentSpawnPoint++;
        }

        //Stop Invoke when no enemy tanks left to spawn
        if (currentEnemySpawning < tankSpawningOrder.Count - 1)
        {
            currentEnemySpawning++;
        }
        else
        {
            CancelInvoke("SpawnEnemy");
        }
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static float GetSpawnFrequency()
    {
        return singletonInstance.spawningFrequencyRate;
    }

    public static void SetSpawnFrequency(float frequency)
    {
        singletonInstance.spawningFrequencyRate = frequency;
    }
}
