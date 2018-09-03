using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb2d.velocity = new Vector3(0, -1, 0) * Time.fixedDeltaTime * 250;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            rb2d.velocity = new Vector3(0, 0, 0);
        }
    }
}