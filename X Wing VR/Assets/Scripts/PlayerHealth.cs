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
        spawnLocation = gameObject.transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        gameObject.transform.position = spawnLocation;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentHealth = maxHealth;
    }
}
