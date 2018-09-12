using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public enum EnemyType { Small, Fast, Heavy, Armored };
    public EnemyType tankType;

    public int tankSpeed;
    public int bulletSpeed;

    public int health;
    public int bulletsFiredLimit;

    public float primaryDirection;
    public float secondaryDirection;
    public float awayDirection;

    public float minMoveChangeDelay;
    public float maxMoveChangeDelay;

    public float minFireDelay;
    public float maxFireDelay;

    private void Awake()
    {
        if (tankType == EnemyType.Small)
        {
            tankSpeed = 500;
            bulletSpeed = 1200;

            health = 1;
            bulletsFiredLimit = 1;

            primaryDirection = .5f;
            secondaryDirection = .3f;
            awayDirection = .1f;

            minMoveChangeDelay = 3f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1.5f;
            maxFireDelay = 5f;
        }
        else if (tankType == EnemyType.Fast)
        {
            tankSpeed = 850;
            bulletSpeed = 1200;

            health = 1;
            bulletsFiredLimit = 1;

            primaryDirection = .35f;
            secondaryDirection = .4f;
            awayDirection = .15f;

            minMoveChangeDelay = 1f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1.5f;
            maxFireDelay = 5f;
        }
        else if (tankType == EnemyType.Heavy)
        {
            tankSpeed = 500;
            bulletSpeed = 1800;

            health = 1;
            bulletsFiredLimit = 2;

            primaryDirection = .5f;
            secondaryDirection = .3f;
            awayDirection = .1f;

            minMoveChangeDelay = 3f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1.5f;
            maxFireDelay = 3.5f;
        }
        else if (tankType == EnemyType.Armored)
        {
            tankSpeed = 400;
            bulletSpeed = 1800;

            health = 4;
            bulletsFiredLimit = 2;

            primaryDirection = .5f;
            secondaryDirection = .3f;
            awayDirection = .1f;

            minMoveChangeDelay = 5f;
            maxMoveChangeDelay = 6f;

            minFireDelay = 3f;
            maxFireDelay = 5f;
        }
    }
}
