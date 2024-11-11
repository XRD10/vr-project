using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    public Vector3 targetPosition;

    private Vector3 direction;

    public int timeToLive = 3;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = (targetPosition - transform.position).normalized;
        if( direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        Destroy(gameObject, timeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        float moveStep = speed * Time.deltaTime;

        transform.position += direction * moveStep;
        
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
       
    }
}
