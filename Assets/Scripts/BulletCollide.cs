using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletCollide : MonoBehaviour
{
    public bool firedByPlayer = false;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
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
        else if(collision.gameObject.tag == "Enemy" && firedByPlayer)
        {
            collision.gameObject.SetActive(false);
            
        }
        else if(collision.gameObject.tag == "Eagle")
        {
            Debug.Log("Game Over!");
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
