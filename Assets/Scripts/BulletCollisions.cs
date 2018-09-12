using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletCollisions : MonoBehaviour
{
    public bool firedByPlayer = false;

    private float dyingAnimationDuration = .25f;
    private GameObject brick;
    private GameObject water;
    private GameObject enemyTank;
    private GameObject player;
    private ContactPoint2D[] contacts;
    private Tilemap tilemap;
    private Vector3 hitPosition;
    private Vector3Int tileHit, adjacentTileHit;

    void Start()
    {
        brick = GameObject.FindGameObjectWithTag("Brick");
        
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == brick)
        {
            tilemap = collision.gameObject.GetComponent<Tilemap>();
            contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            hitPosition = Vector3.zero;

            foreach (ContactPoint2D hit in contacts)
            {
                hitPosition.x = hit.point.x - (.2f * hit.normal.x);
                hitPosition.y = hit.point.y - (.2f * hit.normal.y);

                tileHit = tilemap.WorldToCell(GetContact(hit));
                adjacentTileHit = tilemap.WorldToCell(GetAdjacentContact(hit));

                tilemap.SetTile(tileHit, null);
                tilemap.SetTile(adjacentTileHit, null);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy") && firedByPlayer)
        {
            int health = collision.gameObject.GetComponent<EnemyProperties>().health;
            health--;

            if (health == 0)
            {
                //trigger explosion animation
                Destroy(collision.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyProperties>().health = health;
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Animator playerAnim = collision.gameObject.GetComponent<Animator>();
            int health = collision.gameObject.GetComponent<PlayerController>().health;
            health--;

            if (health == 0)
            {
                playerAnim.SetBool("isDying", true);
                yield return new WaitForSeconds(dyingAnimationDuration);
                Destroy(collision.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().health = health;
            }
        }
        else if (collision.gameObject.tag == "Eagle")
        {
            Debug.Log("Game Over!");
            Animator gameOverAnim = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Animator>();
            Animator eagleAnim = GameObject.FindGameObjectWithTag("Eagle").GetComponent<Animator>();

            gameOverAnim.SetBool("isEagleDestroyed", true);
            eagleAnim.SetBool("isEagleDestroyed", true);
        }

        gameObject.SetActive(false);
    }

    private Vector3 GetContact(ContactPoint2D contact)
    {
        float correction = 0.2f;
        float x = contact.point.x - (correction * contact.normal.x);
        float y = contact.point.y - (correction * contact.normal.y);
        return new Vector3(x, y, 0);
    }

    private Vector3 GetAdjacentContact(ContactPoint2D contact)
    {
        float correction = 0.2f;
        int x = (int)Math.Round(contact.point.x, 0, MidpointRounding.AwayFromZero);
        int y = (int)Math.Round(contact.point.y, 0, MidpointRounding.AwayFromZero);

        if (contact.normal.x == 1 || contact.normal.x == -1)
        {
            if (contact.point.y > y)
                return new Vector3(contact.point.x, y - correction, 0);
            else
                return new Vector3(contact.point.x, y + correction, 0);
        }
        else if (contact.normal.y == 1 || contact.normal.y == -1)
        {
            if (contact.point.x > x)
                return new Vector3(x - correction, contact.point.y, 0);
            else
                return new Vector3(x + correction, contact.point.y, 0);
        }
        else
        {
            throw new Exception("contact.normal values != 1");
        }
    }
}
