using System;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType { Ammo, TankSpeed, BulletSpeed, Money };

public class PowerUpController : MonoBehaviour
{
    private static PowerUpController singletonInstance;

    public GameObject powerUpTemplate;

    private GameObject bounds;
    private float minX, minY, maxX, maxY;

    private Sprite ammo;
    private Sprite tankSpeed;
    private Sprite bulletSpeed;
    private Sprite money;

    private void Awake()
    {
        InitializeSingleton();

        ammo = Resources.Load<Sprite>("Ammo_PU");
        tankSpeed = Resources.Load<Sprite>("TankSpeed_PU");
        bulletSpeed = Resources.Load<Sprite>("BulletSpeed_PU");
        money = Resources.Load<Sprite>("Money_PU");
    }

    private void Start()
    {
        bounds = GameObject.FindGameObjectWithTag("Bounds");
        minX = minY = maxX = maxY = 0;

        for (int i = 0; i < bounds.transform.childCount; i++)
        {
            GameObject boundary = bounds.transform.GetChild(i).gameObject;
            Vector3 position = transform.TransformPoint(boundary.transform.position);

            if (position.x < minX)
                minX = position.x;
            if (position.x > maxX)
                maxX = position.x;
            if (position.y < minY)
                minY = position.y;
            if (position.y > maxY)
                maxY = position.y;
        }
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }
    public static void SpawnPowerup()
    {
        singletonInstance.powerUpTemplate.transform.localPosition = GetRandomLocation();
        GetRandomPowerUp();

        Instantiate(singletonInstance.powerUpTemplate); 
    }

    private static void GetRandomPowerUp()
    {
        singletonInstance.powerUpTemplate.GetComponent<PowerUpEffects>().puType = GetRandomPUType();
    }

    private static Vector3 GetRandomLocation()
    {
        float offset = 2f;

        float x = UnityEngine.Random.Range(singletonInstance.minX + offset, singletonInstance.maxX - offset);
        float y = UnityEngine.Random.Range(singletonInstance.minY + offset, singletonInstance.maxY - offset);

        return new Vector3(x, y);
    }

    public static Sprite GetAmmoPUSprite()
    {
        return singletonInstance.ammo;
    }

    public static Sprite GetTankSpeedPUSprite()
    {
        return singletonInstance.tankSpeed;
    }

    public static Sprite GetBulletSpeedPUSprite()
    {
        return singletonInstance.bulletSpeed;
    }

    public static Sprite GetMoneyPUSprite()
    {
        return singletonInstance.money;
    }

    private static PowerUpType GetRandomPUType()
    {
        return (PowerUpType)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(PowerUpType)).Length));
    }
}
