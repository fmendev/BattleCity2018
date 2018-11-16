using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {

    public bool onIce = false;

    private GameObject ice;
    private GameObject ground;

    private void Awake()
    {
        ice = GameObject.FindGameObjectWithTag("Ice");
        ground = GameObject.FindGameObjectWithTag("Ground");
    }

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
}
