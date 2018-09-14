using System;
using System.Collections;
using System.Collections.Generic;
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

            foreach (ContactPoint2D contact in contacts)
            {
                tileHit = tilemap.WorldToCell(GetContact(contact));
                adjacentTileHit = tilemap.WorldToCell(GetAdjacentTile(contact, tileHit));

                //Debug.Log("hit point: " + hit.point);
                //Debug.Log("t: " + tileHit);
                //Debug.Log("a: " + adjacentTileHit);
                //Debug.Log("Tile: " + tilemap.GetTile(tileHit));
                //Debug.Log("Adjacent: " + tilemap.GetTile(adjacentTileHit));
                tilemap.SetTile(tileHit, null);
                tilemap.SetTile(adjacentTileHit, null);
            }
            //Debug.Log("next bullet...");
        }
        else if (collision.gameObject.CompareTag("Enemy") && firedByPlayer)
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
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            gameOverAnim.SetBool("isEagleDestroyed", true);
            eagleAnim.SetBool("isEagleDestroyed", true);

            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerController>().enabled = false;
            }
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

    private Vector3Int GetAdjacentTile(ContactPoint2D contact, Vector3Int tileHit)
    {
        int normal_x = (int)Math.Round(contact.normal.x, 0);
        int normal_y = (int)Math.Round(contact.normal.y, 0);

        Vector3Int adjacentTile = new Vector3Int();

        if (normal_x == 1 || normal_x == -1)
        {
            Vector3Int tileAbove = new Vector3Int(tileHit.x, tileHit.y + 1, tileHit.z);
            Vector3Int tileBelow = new Vector3Int(tileHit.x, tileHit.y - 1, tileHit.z);

            if (DestructibleTileMapping.tiles_y[tileHit] == DestructibleTileMapping.tiles_y[tileAbove])
            {
                adjacentTile = tileAbove;
            }
            else if (DestructibleTileMapping.tiles_y[tileHit] == DestructibleTileMapping.tiles_y[tileBelow])
            {
                adjacentTile = tileBelow;
            }
        }
        else if (normal_y == 1 || normal_y == -1)
        {
            Vector3Int tileLeft = new Vector3Int(tileHit.x - 1, tileHit.y, tileHit.z);
            Vector3Int tileRight = new Vector3Int(tileHit.x + 1, tileHit.y, tileHit.z);

            if (DestructibleTileMapping.tiles_y[tileHit] == DestructibleTileMapping.tiles_y[tileLeft])
            {
                adjacentTile = tileLeft;
            }
            else if (DestructibleTileMapping.tiles_y[tileHit] == DestructibleTileMapping.tiles_y[tileRight])
            {
                adjacentTile = tileRight;
            }
        }

        return adjacentTile;
    }
}
