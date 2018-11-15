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
    
    public static GameObject GetPlayer()
    {
        return singletonInstance.player;
    }

    public static GameObject GetEnemySmall()
    {
        return singletonInstance.enemySmall;
    }

    public static GameObject GetEnemyHeavy()
    {
        return singletonInstance.enemyHeavy;
    }

    public static GameObject GetEnemyFast()
    {
        return singletonInstance.enemyFast;
    }

    public static GameObject GetEnemyArmored()
    {
        return singletonInstance.enemyArmored;
    }
}
