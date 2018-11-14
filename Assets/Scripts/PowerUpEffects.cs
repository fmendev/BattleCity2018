﻿using UnityEngine;

public class PowerUpEffects : MonoBehaviour
{
    public PowerUpType puType;
    public float timer = 15f;

    private GameObject player;
    private Animator anim;

    [SerializeField]
    private float shellSpeedCap = 2500;
    [SerializeField]
    private float tankSpeedCap = 1050;
    [SerializeField]
    private int armorCap = 4;
    [SerializeField]
    private int ammoCap = 7;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        if (puType == PowerUpType.Ammo)
        {
            anim.SetBool("isAmmo", true);
        }

        else if (puType == PowerUpType.BulletSpeed)
        {
            anim.SetBool("isBulletSpeed", true);
        }

        else if (puType == PowerUpType.Money)
        {
            anim.SetBool("isMoney", true);
        }

        else if (puType == PowerUpType.TankSpeed)
        {
            anim.SetBool("isTankSpeed", true);
        }

        else if (puType == PowerUpType.Armor)
        {
            anim.SetBool("isArmor", true);
        }

        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            if (puType == PowerUpType.Ammo)
            {
                if (TankGun.GetCurrentMaxAmmo() < ammoCap)
                {
                    TankGun.IncreaseMaxAmmo();
                    ShellDisplay.UpdateAmmoDisplay();
                }
            }

            else if (puType == PowerUpType.BulletSpeed)
            {
                if (TankGun.GetShellSpeed() < shellSpeedCap)
                {
                    float shellSpeed = TankGun.GetShellSpeed();
                    TankGun.SetShellSpeed(shellSpeed * 1.15f);
                }
            }

            else if (puType == PowerUpType.Money)
            {
                TowerController.AddMoney(200);
            }

            else if (puType == PowerUpType.TankSpeed)
            {
                float tankSpeed = player.GetComponent<PlayerController>().playerSpeed;

                if (tankSpeed < tankSpeedCap)
                {
                    player.GetComponent<PlayerController>().playerSpeed = tankSpeed * 1.15f;
                }
            }

            else if (puType == PowerUpType.Armor)
            {
                int currentMaxArmor = ArmorController.GetMaxArmor();
                string param = player.GetComponent<PlayerController>().currentSizeParameter;

                player.GetComponent<Animator>().SetBool(param, false);

                if (currentMaxArmor < armorCap)
                {
                    if (currentMaxArmor == 1)
                    {
                        player.GetComponent<PlayerController>().currentSizeParameter = "isMoving_S1";
                    }
                    else if (currentMaxArmor == 2)
                    {
                        player.GetComponent<PlayerController>().currentSizeParameter = "isMoving_S2";
                    }
                    else if (currentMaxArmor == 3)
                    {
                        player.GetComponent<PlayerController>().currentSizeParameter = "isMoving_S3";
                    }

                    //Increase max armor and set it to the new max
                    ArmorController.IncreaseMaxArmor();
                    ArmorController.SetArmor(ArmorController.GetMaxArmor());
                }
                else
                {
                    //if already at max, just refill armor
                    ArmorController.SetArmor(ArmorController.GetMaxArmor());
                }
            }

            Destroy(gameObject);
        }
    }
}
