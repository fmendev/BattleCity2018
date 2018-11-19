using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager singletonInstance;

    public GameObject enemyTankParentObject;

    private List<EnemyType> enemyTankList;
    private string customTankOrder;

    private Vector3 initialPlayerOrientation;

    private void Awake()
    {
        InitializeSingleton();

        customTankOrder = "000212111333";
        enemyTankList = GenerateEnemyTankList();
    }

    private void Start ()
    {
        StartCoroutine(PlayScriptedTooltipAnimations());

        initialPlayerOrientation = Vector3.up;
	}

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    private IEnumerator PlayScriptedTooltipAnimations()
    {
        yield return new WaitForSeconds(2f);
        TooltipController.PlayTooltipAnimation(Tooltip.Eagle, false);
        PauseManager.FreezeDynamicObjects();
        EnemySpawnerController.SetSpawnFrequency(.8f);
        EnemySpawnerController.SetSpawnStartTime(0f);

        //Freeze as soon as spawned has been called on the 3rd enemy
        yield return new WaitWhile(() => EnemySpawnerController.GetNumberEnemiesSpawned() != 3);
        PauseManager.FreezeDynamicObjects();

        //Show crosshairs once all three enemies are spawned and on scene
        yield return new WaitWhile(() => enemyTankParentObject.transform.childCount != 3);
        TooltipController.PlayTooltipAnimation(Tooltip.FirstWave, false);
        EnemySpawnerController.SetSpawnFrequency(4f);
        EnemySpawnerController.SetSpawnStartTime(1f);

        yield return new WaitWhile(() => EnemySpawnerController.GetNumberEnemiesSpawned() != 4);
        TooltipController.PlayTooltipAnimation(Tooltip.Ammo, true);
        yield return new WaitForSeconds(.5f);
        TooltipController.PlayTooltipAnimation(Tooltip.HealthBar, true);
        yield return new WaitForSeconds(.5f);
        TooltipController.PlayTooltipAnimation(Tooltip.Lives, true);
    }

    public static List<EnemyType> GetEnemyTankList()
    {
        return singletonInstance.enemyTankList;
    }

    public static Vector3 GetInitialPlayerOrientation()
    {
        return singletonInstance.initialPlayerOrientation;
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
