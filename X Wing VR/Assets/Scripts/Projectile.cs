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
        damage = 1;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        } 
        
        else if(other.CompareTag("Meteorite"))
        {
            Destroy(gameObject);
        }
    }
}
