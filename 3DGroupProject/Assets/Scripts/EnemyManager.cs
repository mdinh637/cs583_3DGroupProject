// using NUnit.Framework;
// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public class WaveDetails
// {
//     public int basicEnemy;
//     public int fastEnemy;
//     public int tankyEnemy;
// }
// public class EnemyManager : MonoBehaviour
// {
//     [SerializeField] private WaveDetails currentWave;
//     [Space]
//     [SerializeField] private Transform respawn;
//     [SerializeField] public float spawnCooldown;
//     public float spawnTimer;

//     private List<GameObject> enemiesToCreate;
//     [Header("Enemy Prefabs")]
//     [SerializeField] private GameObject basicEnemy;
//     [SerializeField] private GameObject fastEnemy;
//     [SerializeField] private GameObject tankyEnemy;

//     private void Start()
//     {
//         enemiesToCreate = NewEnemyWave();
//     }
//     private void Update()
//     {
//         spawnTimer -= Time.deltaTime;

//         if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
//         {
//             CreateEnemy();
//             spawnTimer = spawnCooldown;
//         }
//     }

//     private void CreateEnemy()
//     {
//         GameObject randomEnemy = GetRandomEnemy();
//         GameObject newEnemy = Instantiate(randomEnemy, respawn.position, Quaternion.identity);
//     }

//     private GameObject GetRandomEnemy()
//     {
//         int randomIndex = Random.Range(0, enemiesToCreate.Count);
//         GameObject chosenEnemy = enemiesToCreate[randomIndex];

//         enemiesToCreate.Remove(chosenEnemy);
//         return chosenEnemy;
//     }

//     private List<GameObject> NewEnemyWave()
//     {
//         List<GameObject> newEnemyList = new List<GameObject>();

//         for (int i = 0; i < currentWave.basicEnemy; i++)
//         {
//             newEnemyList.Add(basicEnemy);
//         }

//         for (int i = 0; i < currentWave.fastEnemy; i++)
//         {
//             newEnemyList.Add(fastEnemy);
//         }

//         for (int i = 0; i < currentWave.tankyEnemy; i++)
//         {
//             newEnemyList.Add(tankyEnemy);
//         }

//         return newEnemyList;
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform respawn;
    [SerializeField] private float spawnCooldown = 0.5f;
    [SerializeField] private int enemiesPerCluster = 10;
    [SerializeField] private float clusterDelay = 5f;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;
    [SerializeField] private GameObject tankyEnemy;

    [Header("Spawn Weights")]
    [SerializeField] private float basicWeight = 1f;
    [SerializeField] private float fastWeight = 0f;
    [SerializeField] private float tankyWeight = 0f;

    [Header("Difficulty Scaling")]
    [SerializeField] private float fastIncreaseRate = 0.02f;
    [SerializeField] private float tankyIncreaseRate = 0.01f;

    private int enemiesSpawnedInCluster;
    private float spawnTimer;
    private float clusterTimer;
    private bool spawningCluster = true;

    private void Update()
    {
        if (spawningCluster)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = spawnCooldown;

                enemiesSpawnedInCluster++;

                if (enemiesSpawnedInCluster >= enemiesPerCluster)
                {
                    spawningCluster = false;
                    clusterTimer = clusterDelay;
                    enemiesSpawnedInCluster = 0;
                }
            }
        }
        else
        {
            clusterTimer -= Time.deltaTime;

            if (clusterTimer <= 0f)
            {
                spawningCluster = true;
                IncreaseDifficulty();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = GetWeightedRandomEnemy();
        Instantiate(enemyPrefab, respawn.position, Quaternion.identity);
    }

    private GameObject GetWeightedRandomEnemy()
    {
        float totalWeight = basicWeight + fastWeight + tankyWeight;
        float randomValue = Random.Range(0f, totalWeight);

        if (randomValue < basicWeight)
            return basicEnemy;

        randomValue -= basicWeight;

        if (randomValue < fastWeight)
            return fastEnemy;

        return tankyEnemy;
    }

    private void IncreaseDifficulty()
    {
        fastWeight += fastIncreaseRate;
        tankyWeight += tankyIncreaseRate;
    }
}
