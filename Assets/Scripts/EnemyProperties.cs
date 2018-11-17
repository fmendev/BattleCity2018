using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyType { Small, Fast, Heavy, Armored };
public enum Target { Player, Eagle };

public class EnemyProperties : MonoBehaviour
{
    public EnemyType enemyType;

    public bool hasPU = false;
    public bool spawnedPU = false;
    private List<int> indexTankWithPU;

    private Animator anim;

    public int tankSpeed;
    public int shellSpeed;

    public int armor;

    public float primaryDirection;
    public float secondaryDirection;
    public float awayDirection;

    public float minMoveChangeDelay;
    public float maxMoveChangeDelay;

    public float minFireDelay;
    public float maxFireDelay;

    public Target preferredTarget;

    private void Awake()
    {
        if (enemyType == EnemyType.Small)
        {
            tankSpeed = 500;
            shellSpeed = 1200;

            armor = 1;

            primaryDirection = .45f;
            secondaryDirection = .45f;
            awayDirection = .05f;

            minMoveChangeDelay = 3f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 3f;
            maxFireDelay = 5f;

            preferredTarget = Target.Player;
        }
        else if (enemyType == EnemyType.Fast)
        {
            tankSpeed = 850;
            shellSpeed = 1200;

            armor = 1;

            primaryDirection = .45f;
            secondaryDirection = .45f;
            awayDirection = .05f;

            minMoveChangeDelay = 1f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1f;
            maxFireDelay = 5f;

            preferredTarget = Target.Eagle;
        }
        else if (enemyType == EnemyType.Heavy)
        {
            tankSpeed = 500;
            shellSpeed = 1800;

            armor = 1;

            primaryDirection = .75f;
            secondaryDirection = .2f;
            awayDirection = .025f;

            minMoveChangeDelay = 3f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1f;
            maxFireDelay = 3.5f;

            preferredTarget = Target.Player;
        }
        else if (enemyType == EnemyType.Armored)
        {
            tankSpeed = 400;
            shellSpeed = 1800;

            armor = 4;

            primaryDirection = .75f;
            secondaryDirection = .2f;
            awayDirection = .025f;

            minMoveChangeDelay = 5f;
            maxMoveChangeDelay = 6f;

            minFireDelay = 3f;
            maxFireDelay = 5f;

            preferredTarget = Target.Player;
        }

        //PU and respective animation
        //indexTankWithPU = new List<int>() { 2, 6, 12, 18 };
        indexTankWithPU = new List<int>() { 0, 1, 2, 3 };
        int siblingIndex = transform.GetSiblingIndex();

        if (indexTankWithPU.Any(i => i == siblingIndex))
            hasPU = true;
        else
            hasPU = false;

        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (hasPU)
        {
            anim.SetBool("isBlinking", true);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
    }
}
