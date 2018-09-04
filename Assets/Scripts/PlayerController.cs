using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public variables
    public float playerSpeed;
    public float bulletSpeed;
    public List<GameObject> bullets;
    #endregion

    #region Movement variables
    private bool lastMoveUpOrDown;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private Rigidbody2D rb2d;
    private Vector3 moveDirection = Vector3.zero;
    #endregion

    #region Bullet variables
    public GameObject bulletPrefab;
    private int bulletsTotal = 20;
    private int bulletsFiredLimit = 2;
    #endregion

    private bool onIce = false;
    private GameObject ice;
    private GameObject ground;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "X: " + horizontalMove.ToString());
        GUI.Label(new Rect(10, 20, 100, 20), "Y: " + verticalMove.ToString());
        GUI.Label(new Rect(10, 30, 100, 20), "Velocity: " + rb2d.velocity.ToString());
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ice = GameObject.FindGameObjectWithTag("Ice");
        ground = GameObject.FindGameObjectWithTag("Ground");

        bullets = new List<GameObject>();
        for (int i = 0; i < bulletsTotal; i++)
        {
            bullets.Add(Instantiate(bulletPrefab));
            bullets[i].SetActive(false);
        }
    }

    private void Update()
    {
        #region Player movement and orientation
        if (lastMoveUpOrDown)
        {
            verticalMove = Input.GetAxisRaw("Vertical");
            if (verticalMove != 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * verticalMove));

            }
        }
        else if (!lastMoveUpOrDown)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            if (horizontalMove != 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Math.Max(0, -horizontalMove)));
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            horizontalMove = 0f;
            lastMoveUpOrDown = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            verticalMove = 0f;
            lastMoveUpOrDown = false;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                verticalMove = 0f;
                lastMoveUpOrDown = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                horizontalMove = 0f;
                lastMoveUpOrDown = true;
            }
        }

        moveDirection = new Vector3(horizontalMove, verticalMove);
        #endregion

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(onIce);
        if (onIce)
        {
            rb2d.AddForce(moveDirection * playerSpeed * 0.7f * Time.fixedDeltaTime);
        }
        else
        {
            rb2d.velocity = moveDirection * playerSpeed * Time.fixedDeltaTime;
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
                bullets[i].GetComponent<BulletCollisions>().firedByPlayer = true;
                break;
            }
        }
    }

    private Vector3 PositionBulletInBarrel(float rotation)
    {
        float distanceToBarrelTip = 2f;
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

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == ice)
        {
            Debug.Log("onIce");
            onIce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == ice)
        {
            Debug.Log("onGround");
            onIce = false;
        }
    }

}
