using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyType { Small, Fast, Heavy, Armored };
public enum Target { Player, Eagle };

public class EnemyProperties : MonoBehaviour
{
    public EnemyType enemyType;

    public bool hasPU = false;
    public bool alreadySpawnedPU = false;
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
            tankSpeed = 600;
            shellSpeed = 1300;

            armor = 1;

            primaryDirection = .5f;
            secondaryDirection = .5f;
            awayDirection = .000001f;

            minMoveChangeDelay = 3f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 3f;
            maxFireDelay = 5f;

            preferredTarget = Target.Player;
        }
        else if (enemyType == EnemyType.Fast)
        {
            tankSpeed = 1000;
            shellSpeed = 1300;

            armor = 1;

            primaryDirection = .65f;
            secondaryDirection = .35f;
            awayDirection = .00001f;

            minMoveChangeDelay = 1f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1f;
            maxFireDelay = 5f;

            preferredTarget = Target.Eagle;
        }
        else if (enemyType == EnemyType.Heavy)
        {
            tankSpeed = 600;
            shellSpeed = 2000;

            armor = 1;

            primaryDirection = .8f;
            secondaryDirection = .2f;
            awayDirection = .0000001f;

            minMoveChangeDelay = 3f;
            maxMoveChangeDelay = 5f;

            minFireDelay = 1f;
            maxFireDelay = 3.5f;

            preferredTarget = Target.Player;
        }
        else if (enemyType == EnemyType.Armored)
        {
            tankSpeed = 400;
            shellSpeed = 2200;

            armor = 4;

            primaryDirection = .8f;
            secondaryDirection = .2f;
            awayDirection = .000001f;

            minMoveChangeDelay = 5f;
            maxMoveChangeDelay = 6f;

            minFireDelay = 3f;
            maxFireDelay = 5f;

            preferredTarget = Target.Player;
        }

        //PU and respective animation
        indexTankWithPU = new List<int>() { 4, 8, 10, 13, 16, 17, 18 };
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
