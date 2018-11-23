using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
	void Start ()
    {
        Destroy(gameObject, 5f);
	}
}
