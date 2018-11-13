using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TankGun : MonoBehaviour
{
    private static TankGun singletonInstance;

    public GameObject shellPrefab;

    [SerializeField]
    private float shellSpeed = 1000;
    [SerializeField]
    private int currentMaxAmmo = 3;
    [SerializeField]
    private int currentShellAmmo = 3;

    private int shellPoolCount = 20;
    private List<int> isLoaded = new List<int>();
    private List<GameObject> playerShellAmmoPool = new List<GameObject>();
    private ShellDisplay shellDisplay;

    private AudioSource sfxSource;
    public AudioClip shootingSFX;

    private void Awake()
    {
        InitializeSingleton();
        InitializeShellAmmoPool();
        InitializeLoadingStatus();

        sfxSource = gameObject.GetComponent<AudioSource>();

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
        for (int i = 0; i < playerShellAmmoPool.Count; i++)
        {
            if (playerShellAmmoPool[i].gameObject.activeSelf == false && currentShellAmmo > 0)
            {
                playerShellAmmoPool[i].gameObject.SetActive(true);
                playerShellAmmoPool[i].transform.position = gameObject.transform.position + PositionBulletInBarrel(gameObject.transform.localRotation.eulerAngles.z);
                playerShellAmmoPool[i].transform.rotation = gameObject.transform.rotation;
                playerShellAmmoPool[i].GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * shellSpeed * Time.fixedDeltaTime;
                playerShellAmmoPool[i].GetComponent<BulletCollisions>().firedByPlayer = true;
                playerShellAmmoPool[i].GetComponent<BulletCollisions>().shooter = gameObject;

                int shellFiredIndex = isLoaded.FindLastIndex(s => s == 1);
                isLoaded[shellFiredIndex] = 0;

                currentShellAmmo--;
                shellDisplay.ReloadShell(shellFiredIndex);

                sfxSource.clip = shootingSFX;
                sfxSource.Play();
                break;
            }
        }
    }

    private Vector3 PositionBulletInBarrel(float rotation)
    {
        float distanceToBarrelTip = 1.4f;
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

    private void InitializeShellAmmoPool()
    {
        for (int i = 0; i < shellPoolCount; i++)
        {
            playerShellAmmoPool.Add(Instantiate(shellPrefab));
            playerShellAmmoPool[i].SetActive(false);
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
