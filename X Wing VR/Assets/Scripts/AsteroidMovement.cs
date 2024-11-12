using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed = 5f;

    private Transform xWing;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotationSpeed;

        GameObject xWingObject = GameObject.FindGameObjectWithTag("X-Wing");

        if (xWingObject != null)
        {
            xWing = xWingObject.transform;
        }
        else
        {
            Debug.LogWarning("No X-Wing GameObject found with tag 'X-Wing'");
        }

        StartCoroutine(DestroyAsteroidAfterTime(70f));
    }

    void Update()
    {
        if (xWing != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, xWing.position, moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator DestroyAsteroidAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
