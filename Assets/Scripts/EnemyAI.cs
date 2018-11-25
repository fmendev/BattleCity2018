using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyAI : MonoBehaviour
{
    public GameObject shellPrefab;
    public bool enabledGUI = false;

    private int enemySpeed;
    private int shellSpeed;
    private float primaryDirection;
    private float secondaryDirection;
    private float awayDirection;
    private float minMoveChangeDelay;
    private float maxMoveChangeDelay;
    private float minFireDelay;
    private float maxFireDelay;
    public Transform currentTarget;

    private Rigidbody2D rb2d;
    private List<float> pNextDirection;
    private Vector3 moveDirection;
    private List<Vector3> barrierDirection = new List<Vector3>();

    private GameObject brick;
    private GameObject water;
    private GameObject concrete;
    private GameObject eagle;

    //Fire function variables
    private int ammoPoolCount = 4;
    private GameObject bigAmmoPool;
    private List<GameObject> ammoPool = new List<GameObject>();

    bool isFrozen = false;
    bool isTouchedByPlayer = false;

    //private void OnGUI()
    //{
    //    int offsetX = 0;
    //    int offsetY = 150;

    //    if (enabledGUI)
    //    {
    //        string barrier0 = "null";
    //        string barrier1 = "null";

    //        if (barrierDirection.Count >= 1)
    //        {
    //            if (barrierDirection[0].x == 1) barrier0 = "Left";
    //            else if (barrierDirection[0].x == -1) barrier0 = "Right";
    //            else if (barrierDirection[0].y == 1) barrier0 = "Down";
    //            else if (barrierDirection[0].y == -1) barrier0 = "Up";
    //            else barrier0 = "??";
    //        }
    //        else barrier0 = "null";

    //        if (barrierDirection.Count >= 2)
    //        {
    //            if (barrierDirection[1].x == 1) barrier1 = "Left";
    //            else if (barrierDirection[1].x == -1) barrier1 = "Right";
    //            else if (barrierDirection[1].y == 1) barrier1 = "Down";
    //            else if (barrierDirection[1].y == -1) barrier1 = "Up";
    //            else barrier1 = "??";
    //        }
    //        else barrier1 = "null";

    //        GUI.Label(new Rect(10 + offsetX, 20 + offsetY, 200, 20), "pRight " + pNextDirection[0].ToString());
    //        GUI.Label(new Rect(10 + offsetX, 30 + offsetY, 200, 20), "pUp " + pNextDirection[1].ToString());
    //        GUI.Label(new Rect(10 + offsetX, 40 + offsetY, 200, 20), "pLeft " + pNextDirection[2].ToString());
    //        GUI.Label(new Rect(10 + offsetX, 50 + offsetY, 200, 20), "pDown " + pNextDirection[3].ToString());
    //        GUI.Label(new Rect(10 + offsetX, 60 + offsetY, 200, 20), "Dir Prev" + previousDirection.ToString());
    //        GUI.Label(new Rect(10 + offsetX, 70 + offsetY, 200, 20), "Dir Cur" + moveDirection.ToString());
    //        GUI.Label(new Rect(10 + offsetX, 80 + offsetY, 200, 20), "pSUM " + pNextDirection.Values.Sum().ToString());
    //        GUI.Label(new Rect(10 + offsetX, 90 + offsetY, 200, 20), "Target Cur: " + currentTarget.ToString());
    //        GUI.Label(new Rect(10 + offsetX, 100 + offsetY, 200, 20), "Barrier[0]: " + barrier0);
    //        GUI.Label(new Rect(10 + offsetX, 110 + offsetY, 200, 20), "Barrier[1]: " + barrier1);

    //        if (pNextDirection.Values.Sum() > 1)
    //            UnityEngine.Debug.Log("Probabilities greater than 1");
    //    }
    //}

    private void Awake()
    {
        //Initialize dictionary with probability of next random move distribution
        pNextDirection = new List<float>()
        {
            0f, //right
            0f, //up
            0f, //left
            1f  //down
        };

        rb2d = GetComponent<Rigidbody2D>();
        bigAmmoPool = GameObject.FindGameObjectWithTag("AmmoPool");

        brick = GameObject.FindGameObjectWithTag("Brick");
        water = GameObject.FindGameObjectWithTag("Water");
        concrete = GameObject.FindGameObjectWithTag("Concrete");
        eagle = GameObject.FindGameObjectWithTag("Eagle");

        InitializeAmmoPool();
    }

    private void Start()
    {
        //Set initial orientation;
        currentTarget = eagle.transform;
        Vector3 orientation = GetInitialOrientation();

        moveDirection = orientation;
        gameObject.transform.rotation = SetRotation(orientation);

        //Initialize enemy tank properties
        EnemyProperties properties = GetComponent<EnemyProperties>();

        enemySpeed = properties.tankSpeed;
        shellSpeed = properties.shellSpeed;

        primaryDirection = properties.primaryDirection;
        secondaryDirection = properties.secondaryDirection;
        awayDirection = properties.awayDirection;

        minMoveChangeDelay = properties.minMoveChangeDelay;
        maxMoveChangeDelay = properties.maxMoveChangeDelay;

        minFireDelay = properties.minFireDelay;
        maxFireDelay = properties.maxFireDelay;

        currentTarget = GetTarget();

        Invoke("WeightedRandomDirection", 3f); 
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
        if (collision.gameObject == brick || collision.gameObject.CompareTag("Enemy") || collision.gameObject == water || collision.gameObject == concrete
            || collision.gameObject.CompareTag("Boundary") || collision.gameObject == eagle)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            if (barrierDirection.Count == 2)
                barrierDirection.Remove(barrierDirection.First());

            barrierDirection.Add(contacts[0].normal);
            WeightedRandomDirection();
        }

        //Colliders to turn to and shoot, then turn away
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (!isTouchedByPlayer)
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

                if (barrierDirection.Count == 2)
                    barrierDirection.Remove(barrierDirection.First());

                barrierDirection.Add(contacts[0].normal);

                Invoke("WeightedRandomDirection", 1f);
                //Invoke("WeightedRandomDirection", UnityEngine.Random.Range(minMoveChangeDelay, maxMoveChangeDelay));

                isTouchedByPlayer = true;
            }
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
            SetBarrierDirectionProbablity(barrierDirection);
        }

        moveDirection = GetSelectedDirection();

        if (!isFrozen) //stop calling if frozen by pause manager
            gameObject.transform.rotation = SetRotation(moveDirection);

        //if WeightedRandomDirection not called by OnCollisionEnter, ignore recent barrier encounters
        StackTrace st = new StackTrace();
        if (st.FrameCount == 1)
           barrierDirection.Clear();

        Invoke("WeightedRandomDirection", UnityEngine.Random.Range(minMoveChangeDelay, maxMoveChangeDelay));
        isTouchedByPlayer = false;
    }

    private List<float> CalculateDirectionProbability()
    {
        Vector3 distance = currentTarget.position - transform.position;

        if (Math.Abs(distance.x) >= Math.Abs(distance.y))
        {
            return new List<float>
            {
                {distance.x >= 0 ? primaryDirection : awayDirection },
                {distance.y >= 0 ? secondaryDirection : awayDirection },
                {distance.x < 0 ? primaryDirection : awayDirection },
                {distance.y < 0 ? secondaryDirection : awayDirection }
            };
        }
        else
        {
            return new List<float>
            {
                {distance.x >= 0 ? secondaryDirection : awayDirection},
                {distance.y >= 0 ? primaryDirection : awayDirection },
                {distance.x < 0 ? secondaryDirection : awayDirection },
                {distance.y < 0 ? primaryDirection : awayDirection }
            };
        }
    }

    private void SetBarrierDirectionProbablity(List<Vector3> barrierDirection)
    {
        //Set probability of most recent barriers hit to zero
        for (int i = 0; i < barrierDirection.Count; i++)
        {
            pNextDirection[0] = barrierDirection[i].x == -1 ? 0 : pNextDirection[0];
            pNextDirection[1] = barrierDirection[i].y == -1 ? 0 : pNextDirection[1];
            pNextDirection[2] = barrierDirection[i].x == 1 ? 0 : pNextDirection[2];
            pNextDirection[3] = barrierDirection[i].y == 1 ? 0 : pNextDirection[3];
        }
    }

    private Vector3 GetSelectedDirection()
    {
        float pSum = pNextDirection[0] + pNextDirection[1] + pNextDirection[2] + pNextDirection[3];

        int selectedDirection = 0;
        float roll = UnityEngine.Random.Range(0f, pSum);
        float cumulative = 0f;

        for (int i = 0; i < pNextDirection.Count; i++) 
        {
            cumulative += pNextDirection[i];
            if (roll < cumulative)
            {
                selectedDirection = i;
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

    private Transform GetTarget()
    {
        EnemyProperties properties = GetComponent<EnemyProperties>();

        if (properties.preferredTarget == Target.Eagle)
        {
            currentTarget = eagle.transform;
        }
        else if (properties.preferredTarget == Target.Player)
        {
            currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
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

                ammoPool[i].GetComponent<ProjectileCollisions>().firedByPlayer = false;
                ammoPool[i].GetComponent<ProjectileCollisions>().shooter = gameObject;
                break;
            }

            if (shellsFired == ammoPoolCount)
            {
                UnityEngine.Debug.Log(gameObject + " Reached ammo pool limit");
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

    private Vector3 GetInitialOrientation()
    {
        Vector3 distance = currentTarget.position - transform.position;

        if (Math.Abs(distance.x) >= Math.Abs(distance.y))
        {
            return distance.x >= 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return distance.y >= 0 ? Vector3.up : Vector3.down;
        }
    }

    public void PauseAI()
    {
        CancelInvoke();
        isFrozen = true;
    }

    public void ResumeAI()
    {
        isFrozen = false;
        WeightedRandomDirection();
        Invoke("Fire", UnityEngine.Random.Range(minFireDelay, maxFireDelay));
    }
}