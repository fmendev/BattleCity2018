using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairBehavior : MonoBehaviour
{
    public bool selfDestroys = false;

    private float crosshairDismissDelay = 2f;

    private void Update()
    {
        if (!selfDestroys)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetBool("isZoomingOut", true);
                StartCoroutine("DestroyCrosshairGameObject");
            }
        }
        else
        {
            StartCoroutine("DismissCrosshair");
            StartCoroutine("DestroyCrosshairGameObject");
        }
    }

    private IEnumerator DismissCrosshair()
    {
        yield return new WaitForSeconds(crosshairDismissDelay);
        GetComponent<Animator>().SetBool("isZoomingOut", true);
    }

    private IEnumerator DestroyCrosshairGameObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

