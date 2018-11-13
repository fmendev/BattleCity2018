using UnityEngine;

public class PowerUpEffects : MonoBehaviour
{
    public PowerUpType puType;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (puType == PowerUpType.Ammo)
        {
            GetComponent<SpriteRenderer>().sprite = PowerUpController.GetAmmoPUSprite();
        }

        else if (puType == PowerUpType.BulletSpeed)
        {
            GetComponent<SpriteRenderer>().sprite = PowerUpController.GetBulletSpeedPUSprite();
        }

        else if (puType == PowerUpType.Money)
        {
            GetComponent<SpriteRenderer>().sprite = PowerUpController.GetMoneyPUSprite();
        }

        else if (puType == PowerUpType.TankSpeed)
        {
            GetComponent<SpriteRenderer>().sprite = PowerUpController.GetTankSpeedPUSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            if (puType == PowerUpType.Ammo)
            {
                TankGun.IncreaseMaxAmmo();
                ShellDisplay.UpdateAmmoDisplay();
            }

            else if (puType == PowerUpType.BulletSpeed)
            {
                float speed = TankGun.GetShellSpeed();
                TankGun.SetShellSpeed(speed * 1.15f);
            }

            else if (puType == PowerUpType.Money)
            {
            }

            else if (puType == PowerUpType.TankSpeed)
            {
                float speed = player.GetComponent<PlayerController>().playerSpeed;
                player.GetComponent<PlayerController>().playerSpeed = speed * 1.15f;
            }
        }
    }
}
