using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectileCollisions : MonoBehaviour
{
    public bool firedByPlayer = false;
    public GameObject shooter;
    public GameObject explosion;

    private GameObject logicController;
    private GameObject brick;
    private GameObject concrete;

    void Start()
    {
        logicController = GameObject.FindGameObjectWithTag("LogicController");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            Vector3Int tileHit, adjacentTileHit;
            Vector3 normal = Vector3.zero, point;

            //Get contacts
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            //Check that the normal vector for all contact points is the same
            if (contacts.All(c => Math.Round(c.normal.x) == Math.Round(contacts[0].normal.x))
                && contacts.All(c => Math.Round(c.normal.y) == Math.Round(contacts[0].normal.y)))
            {
                normal = new Vector3(contacts[0].normal.x, contacts[0].normal.y);
            }
            //if contacts have different normals, default to opposite direction player is facing, eg.: (-1,0) when player is facing right
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                float direction = player.transform.localRotation.eulerAngles.z;
                switch ((int)direction)
                {
                    case 0:
                        normal = new Vector3(-1, 0, 0);
                        break;
                    case 90:
                    case -270:
                        normal = new Vector3(0, 1, 0);
                        break;
                    case 180:
                    case -180:
                        normal = new Vector3(1, 0, 0);
                        break;
                    case -90:
                    case 270:
                        normal = new Vector3(0, -1, 0);
                        break;
                    default:
                        throw new Exception();
                }
            }

            float correction = .5f;

            point = new Vector3(contacts.Average(c => c.point.x - (correction * normal.x)),
                               (contacts.Average(c => c.point.y - (correction * normal.y))));

            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
            tileHit = tilemap.WorldToCell(point);
            adjacentTileHit = tilemap.WorldToCell(GetAdjacentTile(normal, tileHit));

            tilemap.SetTile(tileHit, null);
            tilemap.SetTile(adjacentTileHit, null);
            if (firedByPlayer)
            {
                SoundManager.PlaySfx(SFX.ProjectileHitsBrick);
            }
        }

        //Enemy is hit by player's bullet
        else if (collision.gameObject.CompareTag("Enemy") && firedByPlayer && collision.gameObject != shooter)
        {
            int health = collision.gameObject.GetComponent<EnemyProperties>().armor;
            health--;

            if (health == 0)
            {
                if (collision.gameObject.GetComponent<EnemyProperties>().hasPU && !collision.gameObject.GetComponent<EnemyProperties>().alreadySpawnedPU)
                {
                    PowerUpController.SpawnPowerup();
                    collision.gameObject.GetComponent<EnemyProperties>().alreadySpawnedPU = true;
                }

                explosion.transform.localPosition = collision.gameObject.transform.localPosition;

                Instantiate(explosion);
                if (collision.gameObject.GetComponent<EnemyProperties>().enemyType == EnemyType.Armored)
                {
                    SoundManager.PlaySfx(SFX.ExplosionEnemyArmored);
                }
                else
                {
                    SoundManager.PlaySfx(SFX.ExplosionEnemyRegular);
                }
                Destroy(collision.gameObject);
                LevelEndManager.IncreaseKillCount();
            }
            else
            {
                if (collision.gameObject.GetComponent<EnemyProperties>().enemyType == EnemyType.Armored)
                {
                    SoundManager.PlaySfx(SFX.DamageEnemy);
                }
                collision.gameObject.GetComponent<EnemyProperties>().armor = health;
            }
        }
        //Player is hit by a bullet that is not his own
        else if (collision.gameObject.CompareTag("Player") && collision.gameObject != shooter)
        {
            Animator playerAnim = collision.gameObject.GetComponent<Animator>();
            ArmorController.TakeDamage();

            if (ArmorController.GetArmor() == 0)
            {
                explosion.transform.localPosition = collision.gameObject.transform.localPosition;

                Instantiate(explosion);
                LivesController.DecreaseLives();
                SoundManager.PlaySfx(SFX.ExplosionPlayer);
                collision.gameObject.SetActive(false);
            }
        }
        //Bullet hits eagle
        else if (collision.gameObject.CompareTag("Eagle"))
        {
            Animator eagleAnim = GameObject.FindGameObjectWithTag("Eagle").GetComponent<Animator>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            eagleAnim.SetBool("isEagleDestroyed", true);

            SoundManager.Stop();
            SoundManager.PlaySfx(SFX.ExplosionEagle);
            LevelEndManager.ShowLevelEndScreen(Outcome.Defeat);

            logicController.GetComponent<PlayerController>().enabled = false;
            logicController.GetComponent<WeaponsController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
        }
        else if ((collision.gameObject.CompareTag("Concrete") || collision.gameObject.CompareTag("Boundary")) && firedByPlayer)
        {
            SoundManager.PlaySfx(SFX.ProjectileHitsWall);
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
