using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(100, waitTillReset: 0);
    }
}
