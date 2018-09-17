using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletCollisions : MonoBehaviour
{
    public bool firedByPlayer = false;
    public GameObject shooter;

    private float dyingAnimationDuration = .25f;
    private GameObject brick;
    private GameObject water;
    private GameObject enemyTank;
    private GameObject player;
    private ContactPoint2D[] contacts;
    private Vector3 hitPosition;

    void Start()
    {
        brick = GameObject.FindGameObjectWithTag("Brick");
        
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == brick)
        {
            Vector3Int tileHit, adjacentTileHit;
            Vector3 normal, point;

            //Get contacts
            contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            //Check that the normal vector for all contact points is the same
            if (contacts.All(c => Math.Round(c.normal.x) == Math.Round(contacts[0].normal.x))
                && contacts.All(c => Math.Round(c.normal.y) == Math.Round(contacts[0].normal.y)))
            {
                normal = new Vector3(contacts[0].normal.x, contacts[0].normal.y);
            }
            else
            {
                foreach (var contact in contacts)
                {
                    Debug.Log(contact.normal);
                }
                throw new Exception("Vector normal to collision not same for all contacts");
            }

            float correction = .5f;
            point = new Vector3(contacts.Average(c => c.point.x - (correction * normal.x)), (contacts.Average(c => c.point.y - ((correction * normal.y)))));

            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
            tileHit = tilemap.WorldToCell(point);
            adjacentTileHit = tilemap.WorldToCell(GetAdjacentTile(normal, tileHit));

            tilemap.SetTile(tileHit, null);
            tilemap.SetTile(adjacentTileHit, null);
        }
        else if (collision.gameObject.CompareTag("Enemy") && firedByPlayer && collision.gameObject != shooter)
        {
            Animator enemyAnim = collision.gameObject.GetComponent<Animator>();
            int health = collision.gameObject.GetComponent<EnemyProperties>().health;
            health--;

            if (health == 0)
            {
                enemyAnim.SetBool("isDying", true);
                collision.gameObject.GetComponent<EnemyProperties>().tankSpeed = 0;
                yield return new WaitForSeconds(dyingAnimationDuration);
                enemyAnim.SetBool("isDying", false);
                Destroy(collision.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyProperties>().health = health;
            }
        }
        else if (collision.gameObject.CompareTag("Player") && collision.gameObject != shooter)
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
            Animator gameOverAnim = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Animator>();
            Animator eagleAnim = GameObject.FindGameObjectWithTag("Eagle").GetComponent<Animator>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            gameOverAnim.SetBool("isEagleDestroyed", true);
            eagleAnim.SetBool("isEagleDestroyed", true);

            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerController>().enabled = false;
            }
        }

        if (collision.gameObject != shooter)
        {
            gameObject.SetActive(false);
        }
    }

    private Vector3Int GetAdjacentTile(Vector3 normal, Vector3Int tileHit)
    {
        int normal_x = (int)Math.Round(normal.x, 0);
        int normal_y = (int)Math.Round(normal.y, 0);

        Vector3Int adjacentTile = new Vector3Int(999,999,999);

        if (normal_x == 1 || normal_x == -1)
        {
            Vector3Int tileAbove = new Vector3Int(tileHit.x, tileHit.y + 1, tileHit.z);
            Vector3Int tileBelow = new Vector3Int(tileHit.x, tileHit.y - 1, tileHit.z);

            bool above = DestructibleTileMapping.tiles_y.ContainsKey(tileAbove);
            bool below = DestructibleTileMapping.tiles_y.ContainsKey(tileBelow);

            if (above)
            {
                if (DestructibleTileMapping.tiles_y[tileHit] == DestructibleTileMapping.tiles_y[tileAbove])
                {
                    adjacentTile = tileAbove;
                }
            }

            if (below)
            {
                if (DestructibleTileMapping.tiles_y[tileHit] == DestructibleTileMapping.tiles_y[tileBelow])
                {
                    adjacentTile = tileBelow;
                }
            }
        }
        else if (normal_y == 1 || normal_y == -1)
        {
            Vector3Int tileLeft = new Vector3Int(tileHit.x - 1, tileHit.y, tileHit.z);
            Vector3Int tileRight = new Vector3Int(tileHit.x + 1, tileHit.y, tileHit.z);

            bool left = DestructibleTileMapping.tiles_x.ContainsKey(tileLeft);
            bool right = DestructibleTileMapping.tiles_x.ContainsKey(tileRight);

            if (left)
            {
                if (DestructibleTileMapping.tiles_x[tileHit] == DestructibleTileMapping.tiles_x[tileLeft])
                {
                    adjacentTile = tileLeft;
                }
            }
            if (right)
            {
                if (DestructibleTileMapping.tiles_x[tileHit] == DestructibleTileMapping.tiles_x[tileRight])
                {
                    adjacentTile = tileRight;
                }
            }
        }

        return adjacentTile;
    }
}
