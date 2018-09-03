using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletCollide : MonoBehaviour
{
    public bool firedByPlayer = false;

    private GameObject brickGameObject;
    private GameObject enemyTank;
    private GameObject player;
    private ContactPoint2D[] contacts;
    private Tilemap tilemap;
    private Vector3 hitPosition;
    private Vector3Int tileHit, adjacentTileHit;

    void Start()
    {
        brickGameObject = GameObject.FindGameObjectWithTag("Brick");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if bullet collides with a brick tile, destroy tile
        if (collision.gameObject == brickGameObject)
        {
            tilemap = collision.gameObject.GetComponent<Tilemap>();
            contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            hitPosition = Vector3.zero;

            //Debug.Log("Collision has " + collision.contactCount + " contacts");
            foreach (ContactPoint2D hit in contacts)
            {
                //Debug.Log("Hit normal: x:" + hit.normal.x + " y:" + hit.normal.y);
                //Debug.Log("Hit point converted: " + hit.point.x + "," + hit.point.y);

                hitPosition.x = hit.point.x - (.2f * hit.normal.x);
                hitPosition.y = hit.point.y - (.2f * hit.normal.y);

                tileHit = tilemap.WorldToCell(GetContact(hit));
                adjacentTileHit = tilemap.WorldToCell(GetAdjacentContact(hit));

                tilemap.SetTile(tileHit, null);
                tilemap.SetTile(adjacentTileHit, null);

                //Debug.Log("Hit point converted: " + hitPosition.x + "," + hitPosition.y);
                //Debug.Log("Cell: " + tilemap.WorldToCell(hitPosition));
            }
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Enemy" && firedByPlayer)
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
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
