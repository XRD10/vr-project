using System.Collections;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private GameObject asteroid;
    [SerializeField] private GameObject rocket;
    [SerializeField] private float spawnInterval = 5f;
    private int asteroidsPerSpawn = 1;

    private BoxCollider boxCollider;

    private void Start()
    {
        StartCoroutine(SpawnAsteroidsRoutine());
    }

    private IEnumerator SpawnAsteroidsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnAsteroids();
        }
    }

    private void SpawnAsteroids()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider not found on the GameObject.");
            return;
        }

        Vector3 spawnAreaSize = boxCollider.size;
        Vector3 spawnAreaCenter = boxCollider.center;

        for (int i = 0; i < asteroidsPerSpawn; i++)
        {
            float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
            float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

            Vector3 randomPosition = transform.position + spawnAreaCenter + new Vector3(randomX, randomY, randomZ);

            Instantiate(asteroid, randomPosition, Quaternion.identity);
        }

        Debug.Log("Spawn end");
    }
}
