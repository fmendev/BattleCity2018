using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public variables
    public float playerSpeed;
    public string currentSizeParameter;

    //public Sprite s1, s2, s3;
    #endregion

    #region Movement variables
    private bool lastMoveUpOrDown;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private Animator anim;
    private Rigidbody2D rb2d;
    private Vector3 moveDirection = Vector3.zero;
    #endregion

    private bool onIce = false;
    private GameObject ice;
    private GameObject ground;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "X: " + horizontalMove.ToString());
        GUI.Label(new Rect(10, 20, 100, 20), "Y: " + verticalMove.ToString());
    }

    private void Awake()
    {
        //player should awake facing "front" of level, use eagle
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ice = GameObject.FindGameObjectWithTag("Ice");
        ground = GameObject.FindGameObjectWithTag("Ground");

        currentSizeParameter = "isMoving_S0";
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

        if (moveDirection == Vector3.zero)
        {
            anim.SetBool(currentSizeParameter, false);
        }
        else
        {
            anim.SetBool(currentSizeParameter, true);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (onIce)
        {
            rb2d.AddForce(moveDirection * playerSpeed * 0.7f * Time.fixedDeltaTime);
        }
        else
        {
            rb2d.velocity = moveDirection * playerSpeed * Time.fixedDeltaTime;
        }
    }

    #region Ice effects
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == ice)
        {
            onIce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == ice)
        {
            onIce = false;
        }
    }
    #endregion
}
