using System;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject bulletPrefab;

    private List<GameObject> bullets;
    private int bulletsTotal = 20;
    private int bulletsFiredLimit = 3;

    private AudioSource SFXSource;
    public AudioClip ShootingSFX;

    private void Awake()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < bulletsTotal; i++)
        {
            bullets.Add(Instantiate(bulletPrefab));
            bullets[i].SetActive(false);
        }

        SFXSource = gameObject.GetComponent<AudioSource>();
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
        int bulletsFired = 0;

        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].gameObject.activeSelf == true)
            {
                bulletsFired++;
            }
            else if (bullets[i].gameObject.activeSelf == false && bulletsFired < bulletsFiredLimit)
            {
                bullets[i].gameObject.SetActive(true);
                bullets[i].transform.position = gameObject.transform.position + PositionBulletInBarrel(gameObject.transform.localRotation.eulerAngles.z);
                bullets[i].transform.rotation = gameObject.transform.rotation;
                bullets[i].GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * bulletSpeed * Time.fixedDeltaTime;
                bullets[i].GetComponent<BulletCollisions>().firedByPlayer = true;
                bullets[i].GetComponent<BulletCollisions>().shooter = gameObject;

                SFXSource.clip = ShootingSFX;
                SFXSource.Play();
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
