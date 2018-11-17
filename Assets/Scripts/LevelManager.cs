using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager singletonInstance;

    private List<EnemyType> enemyTankList;
    private string customTankOrder;

    private void Awake()
    {
        InitializeSingleton();

        customTankOrder = "0";// 00212111333";
        enemyTankList = GenerateEnemyTankList();
    }

    private void Start ()
    {
        StartCoroutine(PlayEagleTooltipAnimation());
	}

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    private IEnumerator PlayEagleTooltipAnimation()
    {
        //This wrapper function is just to add a delay before the first tooltip shows up
        yield return new WaitForSeconds(5f);
        TooltipController.PlayTooltipAnimation(Tooltip.Eagle);
        //PauseManager.FreezeGame();
    }
    
    public static List<EnemyType> GetEnemyTankList()
    {
        return singletonInstance.enemyTankList;
    }

    private List<EnemyType> GenerateEnemyTankList()
    {
        List<EnemyType> list = new List<EnemyType>();

        foreach (var c in customTankOrder)
        {
            if (c == '0') list.Add(EnemyType.Small);
            else if (c == '1') list.Add(EnemyType.Heavy);
            else if (c == '2') list.Add(EnemyType.Fast);
            else if (c == '3') list.Add(EnemyType.Armored);
            else
                throw new Exception("Invalid custom tank order string");
        }

        return list;
    }
}
