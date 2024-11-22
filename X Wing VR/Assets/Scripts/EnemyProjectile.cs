using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    [SerializeField]
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
        damage = 1;
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
        
        if(other.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}
