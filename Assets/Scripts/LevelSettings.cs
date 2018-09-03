using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelSettings : MonoBehaviour
{
    public List<GameObject> enemyTanks;

    private Vector3 disabledTankPosition = new Vector3(-60, 20, 0);
    private Vector3 offset = new Vector3(0, -5, 0);

    private void Awake()
    {
        for (int i = 0; i < enemyTanks.Count; i++)
        {
            enemyTanks[i].transform.position = disabledTankPosition + offset;
            enemyTanks[i].gameObject.SetActive(false);
        }
    }
}