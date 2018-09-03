using UnityEngine;

public class EagleHit : MonoBehaviour
{
    public GameObject bullet;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.ToString() == bullet.GetComponent<Collider2D>().name)
        //if (collision.collider.ToString() == "Bullet (UnityEngine.CapsuleCollider2D)")
        Debug.Log("GameOver!!");
    }
}
