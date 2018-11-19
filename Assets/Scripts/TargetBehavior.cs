using System.Collections;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public bool isSelfClosing = false;

    private float crosshairDismissDelay = 2f;

    private void Update()
    {
        if (!isSelfClosing)
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

