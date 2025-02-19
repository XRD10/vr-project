using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnInterval;

    private Coroutine spawnCoroutine;

    private void Start()
    {
       spawnCoroutine = StartCoroutine(SpawnEnemies());
    }


    private void Update()
    {
        if (EnemyPool.Instance.AvailableEnemies > 5)
        {
            ResumeSpawning();
        }
    }
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
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
                StopSpawning();
            }
            
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points assigned");
            return;
        }

        else if(spawnPoints.Length == 1)
        {
            
            GameObject enemy = EnemyPool.Instance.GetEnemy();
            if(enemy != null) { 
            enemy.transform.position = spawnPoints[0].transform.position;
            enemy.transform.rotation = spawnPoints[0].transform.rotation;
            }
            else {
                Debug.Log("Spawning stopped");
                StopSpawning();
            }
        }

        
        else { 
        int index = Random.Range(1, spawnPoints.Length);
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
