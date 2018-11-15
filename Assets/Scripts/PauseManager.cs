using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager singletonInstance;

    private GameObject ammoPool;
    private GameObject player;

    private List<Vector2> velocitiesAtFreeze;

	private void Awake ()
    {
        InitializeSingleton();

        velocitiesAtFreeze = new List<Vector2>();
    }

    private void Start()
    {
        ammoPool = GameObject.FindGameObjectWithTag("AmmoPool");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    public static void FreezeGame()
    {
        //No other function other than freezing some elements while tooltips show

        //Freeze bullets
        for (int i = 0; i < singletonInstance.ammoPool.transform.childCount; i++)
        {
            if (singletonInstance.ammoPool.transform.GetChild(i).gameObject.activeSelf)
            {
                Vector2 velocity = singletonInstance.ammoPool.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity;
                singletonInstance.velocitiesAtFreeze.Add(velocity);

                singletonInstance.ammoPool.transform.GetChild(i).GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }

        //Deactivate player controller
        singletonInstance.player.GetComponent<PlayerController>().enabled = false;

        //Deactivate enemy AI

    }
}
