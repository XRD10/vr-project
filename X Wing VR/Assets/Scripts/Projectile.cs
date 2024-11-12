using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    public Vector3 targetPosition;

    private Vector3 direction;

    public int timeToLive = 3;
    
    void Start()
    {
        direction = (targetPosition - transform.position).normalized;
        if( direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        Destroy(gameObject, timeToLive);
    }

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
