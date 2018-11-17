using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFactory : MonoBehaviour
{
    private static TankFactory singletonInstance;

    public GameObject player;
    public GameObject enemySmall;
    public GameObject enemyHeavy;
    public GameObject enemyFast;
    public GameObject enemyArmored;

    private void Start ()
    {
        InitializeSingleton();
	}

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }
    
    public static GameObject GetPlayerTank()
    {
        return singletonInstance.player;
    }

    public static GameObject GetEnemyTank(EnemyType type)
    {
        if (type == EnemyType.Small)
        {
            return singletonInstance.enemySmall;
        }
        else if (type == EnemyType.Heavy)
        {
            return singletonInstance.enemyHeavy;
        }
        else if (type == EnemyType.Fast)
        {
            return singletonInstance.enemyFast;
        }
        else if (type == EnemyType.Armored)
        {
            return singletonInstance.enemyArmored;
        }
        else
        {
            throw new Exception("Enemy type not defined");
        }
    }
}
