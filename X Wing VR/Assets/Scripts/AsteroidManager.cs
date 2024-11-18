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
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        Destroy(explosion);
    }
}
