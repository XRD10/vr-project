using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
   public static EnemyPool Instance;

   [Header("----Pooling Settings----")]
   public GameObject enemyPrefab;
   public int poolSize = 20;

   private Queue<GameObject> enemyPool = new Queue<GameObject>();

   private void Awake()
   {
    if(Instance == null)
    {
        Instance = this;
        InitialisePool();
    }
    else
    {
        Destroy(gameObject);
    }
   }

   private void InitialisePool()
   {
    for(int i = 0; i < poolSize; i++)
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
   }

   public GameObject GetEnemy()
   {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else 
        {
            Debug.Log("EnemyPool is empty");
            return null;
        }
   }

   public void ReturnEnemy(GameObject enemy)
   {
    enemy.SetActive(false);
    enemyPool.Enqueue(enemy);
   }

    public int AvailableEnemies
        { get { return enemyPool.Count; } }
}
