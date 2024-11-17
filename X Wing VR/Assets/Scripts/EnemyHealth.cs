using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    [SerializeField]
    private GameObject explosionPrefab;

    public event Action<GameObject> OnEnemyDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
       
            Die();
        }
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }
    public void Die()
    {
        Debug.Log("Boom");
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        OnEnemyDeath?.Invoke(gameObject);
    }
}
