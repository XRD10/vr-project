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
        Debug.Log("COLLISION");

        if (explosion != null)
        {
            // Instantiate the explosion at the asteroid's position
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);

            // Destroy the explosion instance after some time (optional, can adjust based on need)
            Destroy(explosionInstance, 2f); // Adjust time as needed
        }
    }
}
