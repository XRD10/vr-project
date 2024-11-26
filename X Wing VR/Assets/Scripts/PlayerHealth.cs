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

    public void TakeDamage(int damage, int waitTillReset = 3)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            StartCoroutine(ResetPlayer(waitTillReset));
        }
    }

    private IEnumerator ResetPlayer(int waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        gameObject.transform.position = spawnLocation;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        currentHealth = maxHealth;
    }
}
