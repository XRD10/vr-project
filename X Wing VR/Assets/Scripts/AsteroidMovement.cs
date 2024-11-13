using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;


    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotationSpeed;
    }
}
