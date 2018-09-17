using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyAI : MonoBehaviour
{
    public List<GameObject> targets;
    public GameObject bulletPrefab;

    private int enemySpeed;
    private int bulletSpeed;
    private int health;
    private int bulletsFiredLimit;
    private float primaryDirection;
    private float secondaryDirection;
    private float awayDirection;
    private float minMoveChangeDelay;
    private float maxMoveChangeDelay;
    private float minFireDelay;
    private float maxFireDelay;

    private int targetIndex = 0;
    private int bulletsTotal = 20;
    private Dictionary<int, float> pNextDirection;
    private List<GameObject> bullets;
    private List<Vector3> barrierDirection;
    private Rigidbody2D rb2d;
    private Vector3 distance, previousDirection, moveDirection = Vector3.up;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, transform.GetSiblingIndex() * 10, 200, 20), "velocity " + rb2d.velocity.ToString());
        //GUI.Label(new Rect(10, 50, 200, 20), "Right " + pNextDirection[0].ToString());
        //GUI.Label(new Rect(10, 60, 200, 20), "Up " + pNextDirection[1].ToString());
        //GUI.Label(new Rect(10, 70, 200, 20), "Left " + pNextDirection[2].ToString());
        //GUI.Label(new Rect(10, 80, 200, 20), "Down " + pNextDirection[3].ToString());
        //GUI.Label(new Rect(10, 90, 200, 20), "Previous " + previousDirection.ToString());
        //GUI.Label(new Rect(10, 100, 200, 20), "Current " + moveDirection.ToString());
        //GUI.Label(new Rect(10, 110, 200, 20), "Current " + pNextDirection.Values.Sum().ToString());
        //GUI.Label(new Rect(10, 130, 200, 20), "B_Count: " + barrierDirection.Count.ToString());
    }

    private void Awake()
    {
        //Initialize dictionary with probability of next random move distribution
        pNextDirection = new Dictionary<int, float>()
        {
            {0, .25f}, //right
            {1, .25f}, //up
            {2, .25f}, //left
            {3, .25f}  //down
        };

        barrierDirection = new List<Vector3>();

        rb2d = GetComponent<Rigidbody2D>();

        bullets = new List<GameObject>();
        for (int i = 0; i < bulletsTotal; i++)
        {
            bullets.Add(Instantiate(bulletPrefab));
            bullets[i].SetActive(false);
        }
    }

    private void Start()
    {
        //Initialize enemy tank properties
        EnemyProperties properties = GetComponent<EnemyProperties>();

        enemySpeed = properties.tankSpeed;
        bulletSpeed = properties.bulletSpeed;

        health = properties.health;
        bulletsFiredLimit = properties.bulletsFiredLimit;

        primaryDirection = properties.primaryDirection;
        secondaryDirection = properties.secondaryDirection;
        awayDirection = properties.awayDirection;

        minMoveChangeDelay = properties.minMoveChangeDelay;
        maxMoveChangeDelay = properties.maxMoveChangeDelay;

        minFireDelay = properties.minFireDelay;
        maxFireDelay = properties.maxFireDelay;

        //Make enemy target change for every new enemy spawned
        targetIndex = gameObject.transform.GetSiblingIndex() % targets.Count;

        WeightedRandomDirection();
        Invoke("Fire", UnityEngine.Random.Range(minFireDelay, maxFireDelay));
    }

    private void FixedUpdate()
    {
        rb2d.velocity = moveDirection * enemySpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        Vector3 hitPosition = Vector3.zero;
        collision.GetContacts(contacts);

        if(barrierDirection.Count == 2)
            barrierDirection.Remove(barrierDirection.First());

        barrierDirection.Add(contacts[0].normal);
        WeightedRandomDirection();
    }

    private void WeightedRandomDirection()
    {
        CancelInvoke("WeightedRandomDirection");

        pNextDirection = CalculateDirectionProbability();
        if (barrierDirection.Any())
        {
            pNextDirection = SetBarrierDirectionProbablity(barrierDirection);
        }

        previousDirection = moveDirection;
        while (previousDirection == moveDirection)
        {
            moveDirection = GetSelectedDirection();
        }

        gameObject.transform.rotation = SetRotation(moveDirection);

        //if WeightedRandomDirection not called by OnCollisionEnter, ignore recent barrier encounters
        StackTrace st = new StackTrace();
        if (st.FrameCount == 1)
           barrierDirection.Clear();

        Invoke("WeightedRandomDirection", UnityEngine.Random.Range(minMoveChangeDelay, maxMoveChangeDelay));
    }

    private Quaternion SetRotation(Vector3 direction)
    {
        if (direction.y != 0)
        {
            return Quaternion.Euler(new Vector3(0, 0, 90 * direction.y));
        }

        else if (direction.x != 0)
        {
            return Quaternion.Euler(new Vector3(0, 0, 180 * Math.Max(0, -direction.x)));
        }
        else
        {
            throw new Exception("Enemy tank assigned invalid direction");
        }
    }

    private Dictionary<int, float> CalculateDirectionProbability()
    {

        if (targets[targetIndex] == null)
        {
            targets.Remove(targets[targetIndex]);
            targetIndex = gameObject.transform.GetSiblingIndex() % targets.Count;
            distance = targets[targetIndex].transform.position - transform.position;
        }
        else
        {
            distance = targets[targetIndex].transform.position - transform.position;
        }
        if (Math.Abs(distance.x) >= Math.Abs(distance.y))
        {
            return new Dictionary<int, float>
            {
                {0, distance.x >= 0 ? primaryDirection : awayDirection },
                {1, distance.y >= 0 ? secondaryDirection : awayDirection },
                {2, distance.x < 0 ? primaryDirection : awayDirection },
                {3, distance.y < 0 ? secondaryDirection : awayDirection }
            };
        }
        else
        {
            return new Dictionary<int, float>
            {
                {0, distance.x >= 0 ? secondaryDirection : awayDirection},
                {1, distance.y >= 0 ? primaryDirection : awayDirection },
                {2, distance.x < 0 ? secondaryDirection : awayDirection },
                {3, distance.y < 0 ? primaryDirection : awayDirection }
            };
        }
    }

    private Dictionary<int, float> SetBarrierDirectionProbablity(List<Vector3> barrierDirection)
    {
        float difference = 0;
        float pRight = pNextDirection[0];
        float pUp = pNextDirection[1];
        float pLeft = pNextDirection[2];
        float pDown = pNextDirection[3];

        //Set probability of most recent barrier hit to zero
        for (int i = 0; i < barrierDirection.Count; i++)
        {
            pRight = barrierDirection[i].x == -1 ? 0 : pRight;
            pUp = barrierDirection[i].y == -1 ? 0 : pUp;
            pLeft = barrierDirection[i].x == 1 ? 0 : pLeft;
            pDown = barrierDirection[i].y == 1 ? 0 : pDown;
        }

        List<float> p = new List<float> { pRight, pUp, pLeft, pDown };

        //add what was substracted to the remaining directions so that cumulative probability == 1
        difference = 1 - p.Sum();

        for (int i = 0; i < p.Count; i++)
        {
            if (p[i] != 0)
                p[i] += difference / (p.Count - barrierDirection.Count);
        }

        return new Dictionary<int, float>
        {
            {0, p[0] },
            {1, p[1] },
            {2, p[2] },
            {3, p[3] }
        };
    }

    private Vector3 GetSelectedDirection()
    {
        int selectedDirection = 0;
        float roll = UnityEngine.Random.value;
        float cumulative = 0f;

        foreach (KeyValuePair<int, float> kv in pNextDirection)
        {
            cumulative += kv.Value;
            if (roll < cumulative)
            {
                selectedDirection = kv.Key;
                break;
            }
        }

        switch (selectedDirection)
        {
            case 0:
                return new Vector3(1, 0, 0);
            case 1:
                return new Vector3(0, 1, 0);
            case 2:
                return new Vector3(-1, 0, 0);
            case 3:
                return new Vector3(0, -1, 0);
            default:
                throw new Exception("Enemy selected direction invalid");
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
                bullets[i].GetComponent<BulletCollisions>().firedByPlayer = false;
                bullets[i].GetComponent<BulletCollisions>().shooter = gameObject;
                break;
            }
        }

        Invoke("Fire", UnityEngine.Random.Range(minFireDelay, maxFireDelay));
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