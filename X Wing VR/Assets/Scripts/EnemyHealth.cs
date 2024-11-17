using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    [SerializeField]
    private Animation explode;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Current Health = " + currentHealth);

        if (currentHealth <= 0)
        {
       
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boom");
        //explode.Play();
        Destroy(gameObject);
    }
}
