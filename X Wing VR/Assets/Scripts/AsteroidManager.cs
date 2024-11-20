using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosion;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotationSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (explosion != null)
        {
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(explosionInstance, 2f);
        }

        Destroy(gameObject, 0.1f);
    }
}
