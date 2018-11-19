﻿using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController singletonInstance;

    public bool enabledGUI = true;

    [SerializeField]
    private float playerSpeed;

    private string currentSizeParameter;
    private GameObject playerObject;

    #region Movement variables
    private bool lastMoveUpOrDown;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private Animator anim;
    private Rigidbody2D rb2d;
    private Vector3 moveDirection = Vector3.zero;
    #endregion

    private void OnGUI()
    {
        int offsetX = 45;
        int offsetY = 0;

        if (enabledGUI)
        {
            GUI.Label(new Rect(10 + offsetX, 20 + offsetY, 200, 20), "Lives:" + LivesController.GetCurrentLives().ToString());
            GUI.Label(new Rect(10 + offsetX, 30 + offsetY, 200, 20), "Armor " + ArmorController.GetArmor().ToString());
            //GUI.Label(new Rect(10 + offsetX, 40 + offsetY, 200, 20), "pLeft " + pNextDirection[2].ToString());
            //GUI.Label(new Rect(10 + offsetX, 50 + offsetY, 200, 20), "pDown " + pNextDirection[3].ToString());
            //GUI.Label(new Rect(10 + offsetX, 60 + offsetY, 200, 20), "Dir Prev" + previousDirection.ToString());
            //GUI.Label(new Rect(10 + offsetX, 70 + offsetY, 200, 20), "Dir Cur" + moveDirection.ToString());
            //GUI.Label(new Rect(10 + offsetX, 80 + offsetY, 200, 20), "pSUM " + pNextDirection.Values.Sum().ToString());
            //GUI.Label(new Rect(10 + offsetX, 90 + offsetY, 200, 20), "Target Cur: " + currentTarget.ToString());
            //GUI.Label(new Rect(10 + offsetX, 100 + offsetY, 200, 20), "Barrier[0]: " + barrier0);
            //GUI.Label(new Rect(10 + offsetX, 110 + offsetY, 200, 20), "Barrier[1]: " + barrier1);
        }
    }

    private void Awake()
    {
        InitializeSingleton();

        currentSizeParameter = "isMoving_S0";
    }

    private void Update()
    {
        if (lastMoveUpOrDown)
        {
            verticalMove = Input.GetAxisRaw("Vertical");
            if (verticalMove != 0)
            {
                playerObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * verticalMove));
            }
        }
        else if (!lastMoveUpOrDown)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            if (horizontalMove != 0)
            {
                playerObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Math.Max(0, -horizontalMove)));
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

        if (moveDirection == Vector3.zero)
        {
            anim.SetBool(currentSizeParameter, false);
        }
        else
        {
            anim.SetBool(currentSizeParameter, true);
        }
    }

    private void FixedUpdate()
    {
        if (playerObject.GetComponent<PlayerCollisions>().onIce)
        {
            rb2d.AddForce(moveDirection * playerSpeed * 0.7f * Time.fixedDeltaTime);
        }
        else
        {
            rb2d.velocity = moveDirection * playerSpeed * Time.fixedDeltaTime;
        }
    }

    private void InitializeSingleton()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
        }
        else if (singletonInstance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void RefreshPlayerController()
    {
        singletonInstance.StartCoroutine(singletonInstance.RefreshWhenPlayerActive());
    }

    private IEnumerator RefreshWhenPlayerActive()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);

        playerObject = GameObject.FindGameObjectWithTag("Player");
        rb2d = playerObject.GetComponent<Rigidbody2D>();
        anim = playerObject.GetComponent<Animator>();
    }

    public static GameObject GetPlayerObject()
    {
        return singletonInstance.playerObject;
    }
    public static Transform GetPlayerTransform()
    {
        return singletonInstance.playerObject.transform;
    }

    public static float GetPlayerSpeed()
    {
        return singletonInstance.playerSpeed;
    }

    public static void SetPlayerSpeed(float speed)
    {
        singletonInstance.playerSpeed = speed;
    }

    public static string GetSizeParameter()
    {
        return singletonInstance.currentSizeParameter;
    }

    public static void SetSizeParameter(string param)
    {
        singletonInstance.currentSizeParameter = param;
    }

    public static void SetAnimationParameter(string param, bool value)
    {
        singletonInstance.anim.SetBool(param, value);
    }
}

