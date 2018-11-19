using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyTankParentObject;
    private EnemyType enemyTypeSpawning;
    private DateTime a = DateTime.Now;
    private DateTime c = DateTime.Now;

    public void SpawnEnemy(EnemyType type)
    {
        a = System.DateTime.Now;
        //Debug.Log("Spawn animation started at " + a.ToString("s.ffff"));
        enemyTypeSpawning = type; 
        GetComponent<Animator>().SetBool("isSpawning", true);
        //After spawn animation completes, it calls ActivatePlayer via event. This brings the player on scene and resets the trigger
    }

    private void ActivateEnemy()
    {
        GameObject enemy = Instantiate(TankFactory.GetEnemyTank(enemyTypeSpawning), enemyTankParentObject.transform);
        c = System.DateTime.Now;
        //Debug.Log("Enemy created at " + c.ToString("m.s.ff"));

        enemy.transform.position = gameObject.transform.position;
        GetComponent<Animator>().SetBool("isSpawning", false);
    }
}
