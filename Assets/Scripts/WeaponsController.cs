using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    private static WeaponsController singletonInstance;

    public GameObject shellPrefab;

    [SerializeField]
    private float shellSpeed = 1000;
    [SerializeField]
    private int currentMaxAmmo = 3;
    [SerializeField]
    private int currentShellAmmo = 3;

    private int ammoPoolCount = 20;
    private List<int> isLoaded = new List<int>();
    private List<GameObject> ammoPool = new List<GameObject>();

    private ShellDisplay shellDisplay;
    private GameObject bigAmmoPool;


    private void Awake()
    {
        InitializeSingleton();

        bigAmmoPool = GameObject.FindGameObjectWithTag("AmmoPool");

        InitializeAmmoPool();
        InitializeLoadingStatus();

        shellDisplay = GameObject.FindGameObjectWithTag("ShellDisplay").GetComponent<ShellDisplay>();
    }

    private void InitializeLoadingStatus()
    {
        for (int i = 0; i < currentMaxAmmo; i++)
        {
            isLoaded.Add(1);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        for (int i = 0; i < ammoPool.Count; i++)
        {
            if (ammoPool[i].gameObject.activeSelf == false && currentShellAmmo > 0)
            {
                ammoPool[i].gameObject.SetActive(true);
                ammoPool[i].transform.position = PlayerController.GetPlayerTransform().position+ PositionProjectileInBarrel(PlayerController.GetPlayerTransform().eulerAngles.z);
                ammoPool[i].transform.rotation = PlayerController.GetPlayerTransform().rotation;
                ammoPool[i].GetComponent<Rigidbody2D>().velocity = PlayerController.GetPlayerTransform().right * shellSpeed * Time.fixedDeltaTime;
                ammoPool[i].GetComponent<BulletCollisions>().firedByPlayer = true;
                ammoPool[i].GetComponent<BulletCollisions>().shooter = gameObject;

                int shellFiredIndex = isLoaded.FindLastIndex(s => s == 1);
                isLoaded[shellFiredIndex] = 0;

                currentShellAmmo--;
                shellDisplay.ReloadShell(shellFiredIndex);

                SoundManager.PlaySfx(SoundManager.GetSFX(SFX.playerFire));
                break;
            }
        }
    }

    private Vector3 PositionProjectileInBarrel(float rotation)
    {
        float distanceToBarrelTip = 1.7f;
        switch ((int)rotation)
        {
            case 0:
                return new Vector3(distanceToBarrelTip, 0, 0);
            case 90:
            case -270:
                return new Vector3(0, distanceToBarrelTip, 0);
            case 180:
            case -180:
                return new Vector3(-distanceToBarrelTip, 0, 0);
            case -90:
            case 270:
                return new Vector3(0, -distanceToBarrelTip, 0);
            default:
                throw new Exception();
        }
    }

    private void InitializeSingleton()
    {
        singletonInstance = this;
    }

    private void InitializeAmmoPool()
    {
        for (int i = 0; i < ammoPoolCount; i++)
        {
            ammoPool.Add(Instantiate(shellPrefab, bigAmmoPool.transform));
            ammoPool[i].SetActive(false);
        }
    }

    public static int GetCurrentAmmo()
    {
        return singletonInstance.currentShellAmmo;
    }

    public static void SetCurrentAmmo(int amount)
    {
        singletonInstance.currentShellAmmo = amount;
    }

    public static int GetCurrentMaxAmmo()
    {
        return singletonInstance.currentMaxAmmo;
    }

    public static void IncreaseMaxAmmo()
    {
        singletonInstance.isLoaded.Add(1);
        singletonInstance.currentShellAmmo++;
        singletonInstance.currentMaxAmmo++;
    }

    public static void SetShellAsLoaded(int shellIndex)
    {
        singletonInstance.isLoaded[shellIndex] = 1;
    }

    public static float GetShellSpeed()
    {
        return singletonInstance.shellSpeed;
    }

    public static void SetShellSpeed(float speed)
    {
        singletonInstance.shellSpeed = speed;
    }
}
