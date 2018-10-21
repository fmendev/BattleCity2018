using System;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : MonoBehaviour
{
    public float shellSpeed;
    public int shellAmmo = 3;
    public GameObject shellPrefab;

    private ShellDisplay shellDisplay;
    private List<GameObject> shellPool;
    private int shellPoolCount = 20;

    private AudioSource sfxSource;
    public AudioClip shootingSFX;

    private void Awake()
    {
        shellPool = new List<GameObject>();
        for (int i = 0; i < shellPoolCount; i++)
        {
            shellPool.Add(Instantiate(shellPrefab));
            shellPool[i].SetActive(false);
        }

        sfxSource = gameObject.GetComponent<AudioSource>();

        shellDisplay = GameObject.FindGameObjectWithTag("ShellDisplay").GetComponent<ShellDisplay>();
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
        for (int i = 0; i < shellPool.Count; i++)
        {
            if (shellPool[i].gameObject.activeSelf == false && shellAmmo > 0)
            {
                shellPool[i].gameObject.SetActive(true);
                shellPool[i].transform.position = gameObject.transform.position + PositionBulletInBarrel(gameObject.transform.localRotation.eulerAngles.z);
                shellPool[i].transform.rotation = gameObject.transform.rotation;
                shellPool[i].GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * shellSpeed * Time.fixedDeltaTime;
                shellPool[i].GetComponent<BulletCollisions>().firedByPlayer = true;
                shellPool[i].GetComponent<BulletCollisions>().shooter = gameObject;

                shellDisplay.ReloadShell(shellAmmo);
                shellAmmo--;

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
}
