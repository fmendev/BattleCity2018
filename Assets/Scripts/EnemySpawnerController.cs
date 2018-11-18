using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemySpawnerController : MonoBehaviour
{
    private static EnemySpawnerController singletonInstance;

    private int enemiesSpawned;
    private int currentSpawnPoint;
    private int currentEnemySpawning;
    private float spawningStartTime;
    private float spawningFrequencyRate;
    private List<EnemyType> tankSpawningOrder;

    private void Awake()
    {
        InitializeSingleton();

        enemiesSpawned = 0;
        currentSpawnPoint = 0;
        currentEnemySpawning = 0;
        spawningStartTime = 2.1f;
        spawningFrequencyRate = 4f;
    }

    private void Start()
    {
        tankSpawningOrder = LevelManager.GetEnemyTankList();

        InvokeRepeating("SpawnEnemy", spawningStartTime, spawningFrequencyRate);
    }

    private void SpawnEnemy()
    {
        enemiesSpawned++;
        Debug.Log("Invoke Spawn called");
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
            Debug.Log("Reached end of enemy list. Invoke canceled");
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

    public static float GetSpawnStartTime()
    {
        return singletonInstance.spawningStartTime;
    }

    public static void SetSpawnStartTime(float startTime)
    {
        singletonInstance.spawningStartTime = startTime;
    }

    public static int GetNumberEnemiesSpawned()
    {
        return singletonInstance.enemiesSpawned;
    }

    public static void PauseEnemySpawning()
    {
        singletonInstance.CancelInvoke();
    }

    public static void ResumeEnemySpawning()
    {
        if (singletonInstance.currentEnemySpawning < singletonInstance.tankSpawningOrder.Count - 1)
        {
            singletonInstance.InvokeRepeating("SpawnEnemy", singletonInstance.spawningStartTime, singletonInstance.spawningFrequencyRate);
        }
        
    }

    //public static void IncreaseEnemiesSpawnedCounter()
    //{
    //    singletonInstance.enemiesSpawned++;
    //}
}
