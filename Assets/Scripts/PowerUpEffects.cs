using UnityEngine;

public class PowerUpEffects : MonoBehaviour
{
    public PowerUpType puType;
    public float timer = 10f;

    private GameObject player;
    private Animator anim;

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

        Destroy(gameObject, timer);
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

        Destroy(gameObject);
    }
}
