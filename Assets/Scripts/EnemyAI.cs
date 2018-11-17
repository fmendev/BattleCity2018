using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyAI : MonoBehaviour
{
    public GameObject shellPrefab;
    public bool GUIon = true;

    private int enemySpeed;
    private int shellSpeed;
    private int health;
    private float primaryDirection;
    private float secondaryDirection;
    private float awayDirection;
    private float minMoveChangeDelay;
    private float maxMoveChangeDelay;
    private float minFireDelay;
    private float maxFireDelay;
    public Transform currentTarget;

    private Rigidbody2D rb2d;
    private Dictionary<int, float> pNextDirection;
    private Vector3 previousDirection, moveDirection = Vector3.up;
    private List<Vector3> barrierDirection = new List<Vector3>();

    private GameObject brick;
    private GameObject water;
    private GameObject concrete;
    private GameObject ice;
    private GameObject ground;
    private GameObject eagle;

    //Fire function variables
    private int ammoPoolCount = 20;
    private GameObject bigAmmoPool;
    private List<GameObject> ammoPool = new List<GameObject>();

    private void OnGUI()
    {
        int offsetX = 0;
        int offsetY = 150;

        if (GUIon)
        {
            string barrier0 = "null";
            string barrier1 = "null";

            if (barrierDirection.Count >= 1)
            {
                if (barrierDirection[0].x == 1) barrier0 = "Left";
                else if (barrierDirection[0].x == -1) barrier0 = "Right";
                else if (barrierDirection[0].y == 1) barrier0 = "Down";
                else if (barrierDirection[0].y == -1) barrier0 = "Up";
                else barrier0 = "??";
            }
            else barrier0 = "null";

            if (barrierDirection.Count >= 2)
            {
                if (barrierDirection[1].x == 1) barrier1 = "Left";
                else if (barrierDirection[1].x == -1) barrier1 = "Right";
                else if (barrierDirection[1].y == 1) barrier1 = "Down";
                else if (barrierDirection[1].y == -1) barrier1 = "Up";
                else barrier1 = "??";
            }
            else barrier1 = "null";

            GUI.Label(new Rect(10 + offsetX, 20 + offsetY, 200, 20), "pRight " + pNextDirection[0].ToString());
            GUI.Label(new Rect(10 + offsetX, 30 + offsetY, 200, 20), "pUp " + pNextDirection[1].ToString());
            GUI.Label(new Rect(10 + offsetX, 40 + offsetY, 200, 20), "pLeft " + pNextDirection[2].ToString());
            GUI.Label(new Rect(10 + offsetX, 50 + offsetY, 200, 20), "pDown " + pNextDirection[3].ToString());
            GUI.Label(new Rect(10 + offsetX, 60 + offsetY, 200, 20), "Dir Prev" + previousDirection.ToString());
            GUI.Label(new Rect(10 + offsetX, 70 + offsetY, 200, 20), "Dir Cur" + moveDirection.ToString());
            GUI.Label(new Rect(10 + offsetX, 80 + offsetY, 200, 20), "pSUM " + pNextDirection.Values.Sum().ToString());
            GUI.Label(new Rect(10 + offsetX, 90 + offsetY, 200, 20), "Target Cur: " + currentTarget.ToString());
            GUI.Label(new Rect(10 + offsetX, 100 + offsetY, 200, 20), "Barrier[0]: " + barrier0);
            GUI.Label(new Rect(10 + offsetX, 110 + offsetY, 200, 20), "Barrier[1]: " + barrier1);

            if (pNextDirection.Values.Sum() > 1)
                UnityEngine.Debug.Log("Probabilities greater than 1");
        }
    }

    private void Awake()
    {
        //Initialize dictionary with probability of next random move distribution
        pNextDirection = new Dictionary<int, float>()
        {
            {0, 0f}, //right
            {1, 0f}, //up
            {2, 0f}, //left
            {3, 1f}  //down
        };

        rb2d = GetComponent<Rigidbody2D>();
        bigAmmoPool = GameObject.FindGameObjectWithTag("AmmoPool");

        brick = GameObject.FindGameObjectWithTag("Brick");
        water = GameObject.FindGameObjectWithTag("Water");
        concrete = GameObject.FindGameObjectWithTag("Concrete");
        ice = GameObject.FindGameObjectWithTag("Ice");
        ground = GameObject.FindGameObjectWithTag("Ground");
        eagle = GameObject.FindGameObjectWithTag("Eagle");

        InitializeAmmoPool();
    }

    private void Start()
    {
        //Initialize enemy tank properties
        EnemyProperties properties = GetComponent<EnemyProperties>();

        enemySpeed = properties.tankSpeed;
        shellSpeed = properties.shellSpeed;

        health = properties.armor;

        primaryDirection = properties.primaryDirection;
        secondaryDirection = properties.secondaryDirection;
        awayDirection = properties.awayDirection;

        minMoveChangeDelay = properties.minMoveChangeDelay;
        maxMoveChangeDelay = properties.maxMoveChangeDelay;

        minFireDelay = properties.minFireDelay;
        maxFireDelay = properties.maxFireDelay;

        currentTarget = RefreshTarget();

        WeightedRandomDirection();

        Invoke("Fire", UnityEngine.Random.Range(minFireDelay, maxFireDelay));
    }

    private void FixedUpdate()
    {
        rb2d.velocity = moveDirection * enemySpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //barrierDirection keeps a list of last two barriers encountered

        //Colliders to turn away from
        if (collision.gameObject == brick || collision.gameObject == water || collision.gameObject == concrete || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boundary"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            if (barrierDirection.Count == 2)
                barrierDirection.Remove(barrierDirection.First());

            barrierDirection.Add(contacts[0].normal);
            WeightedRandomDirection();
        }

        //Colliders to turn to and shoot
        else if (collision.gameObject.CompareTag("Player") || collision.gameObject == eagle)
        {
            CancelInvoke("WeightedRandomDirection");

            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            if (!contacts.All(c => Math.Round(c.normal.x) == Math.Round(contacts[0].normal.x))
                && !contacts.All(c => Math.Round(c.normal.y) == Math.Round(contacts[0].normal.y)))
            {
                throw new Exception("Multiple normal to contacts values from enemy-player collision");
            }

            moveDirection = contacts[0].normal * -1;

            gameObject.transform.rotation = SetRotation(moveDirection);

            CancelInvoke("Fire");
            Invoke("Fire", 0f);

            Invoke("WeightedRandomDirection", UnityEngine.Random.Range(minMoveChangeDelay, maxMoveChangeDelay));
        }
    }

    private void WeightedRandomDirection()
    {
        CancelInvoke("WeightedRandomDirection");

        //Get probability distribution for the next direction
        pNextDirection = CalculateDirectionProbability();

        //If 
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

    private Dictionary<int, float> CalculateDirectionProbability()
    {
        currentTarget = RefreshTarget();
        Vector3 distance = currentTarget.position - transform.position;

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

        //Set probability of most recent barriers hit to zero
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

    private Transform RefreshTarget()
    {
        EnemyProperties properties = GetComponent<EnemyProperties>();

        if (properties.preferredTarget == Target.Eagle)
        {
            currentTarget = eagle.transform;
        }
        else if (properties.preferredTarget == Target.Player)
        {
            //If target is refreshed while player is not in the scene, eagle will be the target until refresh is called again
            if (GameObject.FindGameObjectWithTag("Player").transform != null)
            {
                currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                currentTarget = eagle.transform;
            }
        }
        else
        {
            throw new Exception("Preferred target undefined");
        }

        return currentTarget;
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

    private void Fire()
    {
        int shellsFired = 0;

        for (int i = 0; i < ammoPool.Count; i++)
        {
            if (ammoPool[i].gameObject.activeSelf == true)
            {
                shellsFired++;
            }
            else if (ammoPool[i].gameObject.activeSelf == false)
            {
                ammoPool[i].gameObject.SetActive(true);
                ammoPool[i].transform.position = gameObject.transform.position + PositionProjectileInBarrel(gameObject.transform.localRotation.eulerAngles.z);
                ammoPool[i].transform.rotation = gameObject.transform.rotation;
                ammoPool[i].GetComponent<Rigidbody2D>().velocity = gameObject.transform.right * shellSpeed * Time.fixedDeltaTime;
                ammoPool[i].GetComponent<BulletCollisions>().firedByPlayer = false;
                ammoPool[i].GetComponent<BulletCollisions>().shooter = gameObject;
                break;
            }
        }

        Invoke("Fire", UnityEngine.Random.Range(minFireDelay, maxFireDelay));
    }

    private Vector3 PositionProjectileInBarrel(float rotation)
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

    private void InitializeAmmoPool()
    {
        for (int i = 0; i < ammoPoolCount; i++)
        {
            ammoPool.Add(Instantiate(shellPrefab, bigAmmoPool.transform));
            ammoPool[i].SetActive(false);
        }
    }
}