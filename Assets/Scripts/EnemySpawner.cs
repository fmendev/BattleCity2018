﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner singletonInstance;

    public List<GameObject> enemySpawnPoints;
    public List<GameObject> enemyTanks;

    private int spawned = 0;
    private float enemyRespawnTime = 5f;
    private float spawnAnimationDuration = 1f;
    private List<int> spawnLocationOrder;
    private Vector3 enemyDisabledTankPosition = new Vector3(-60, 20, 0);
    private Vector3 playerDisabledTankPosition = new Vector3(-10, 20, 0);
    private Vector3 offset = new Vector3(0, -5, 0);
     
    private void Awake()
    {
        InitializeSingleton();

        for (int i = 0; i < enemyTanks.Count; i++)
        {
            enemyTanks[i].transform.position = playerDisabledTankPosition + offset;
            enemyTanks[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        spawnLocationOrder = new List<int>(GetSpawnLocationOrder());
        StartCoroutine("SpawnEnemy");
    }

    private void Update()
    {
        if (spawned == enemyTanks.Count)
        {
            StopCoroutine("SpawnEnemy");
        }
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < enemyTanks.Count; i++)
        {
            GameObject activeSpawnPoint = enemySpawnPoints[spawnLocationOrder[i]];
            Animator anim = activeSpawnPoint.GetComponent<Animator>();

            anim.SetBool("isSpawning", true);
            yield return new WaitForSeconds(spawnAnimationDuration);
            anim.SetBool("isSpawning", false);

            enemyTanks[i].transform.position = activeSpawnPoint.transform.position;
            enemyTanks[i].gameObject.SetActive(true);
            spawned++;
            yield return new WaitForSeconds(enemyRespawnTime);
        }
    }

    //private IEnumerator SpawnPlayer(int index)
    //{
    //    yield return new WaitWhile(() => playerTanks[index].gameObject.activeSelf == true);
    //    GameObject activeSpawnPoint = playerSpawnPoints[index];
    //    Animator anim = activeSpawnPoint.GetComponent<Animator>();

    //    anim.SetBool("isSpawning", true);
    //    yield return new WaitForSeconds(spawnAnimationDuration);
    //    anim.SetBool("isSpawning", false);

    //    playerTanks[index].SetActive(true);
    //    playerTanks[index].transform.position = playerSpawnPoints[index].transform.position;
    //}

    private IEnumerable<int> GetSpawnLocationOrder()
    {
        IList<int> list = new List<int>();
        int ratio = enemyTanks.Count / enemySpawnPoints.Count;
        int remainder = enemyTanks.Count % enemySpawnPoints.Count;

        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            for (int j = 0; j < ratio; j++)
            {
                list.Add(i);
            }
        }
        for (int i = 0; i < remainder; i++)
        {
            list.Add(UnityEngine.Random.Range(0, enemySpawnPoints.Count));
        }

        return list.OrderBy(item => (int)UnityEngine.Random.value);
    }
}