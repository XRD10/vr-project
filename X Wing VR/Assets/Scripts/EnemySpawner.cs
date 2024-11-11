using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnInterval = 50f;

    private Coroutine spawnCoroutine;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (EnemyPool.Instance == null)
            {
                Debug.Log("EnemyPool instance not found");
                yield break;
            }

            if (EnemyPool.Instance.AvailableEnemies > 0)
            {
                SpawnEnemy();
            }
            else
            {
                Debug.Log("No enemies left");
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points assigned");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        GameObject enemy = EnemyPool.Instance.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = spawnPoints[index].position;
            enemy.transform.rotation = spawnPoints[index].rotation;
        }
        else
        {
            Debug.Log("Spawning stopped");
            StopSpawning();
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public void ResumeSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }
}
