using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public Vector3 spawnLocation;

    private void Start()
    {
        currentHealth = maxHealth;
        spawnLocation = transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Taking damage!");
        Debug.Log("Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Woops you died!");
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        gameObject.transform.position = spawnLocation;
        currentHealth = maxHealth;
    }
}
