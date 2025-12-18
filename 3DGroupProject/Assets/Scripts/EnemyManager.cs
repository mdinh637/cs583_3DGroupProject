using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
    public int tankyEnemy;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [SerializeField] private Transform respawn;
    [SerializeField] public float spawnCooldown;
    public float spawnTimer;

    private List<GameObject> enemiesToCreate;
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;
    [SerializeField] private GameObject tankyEnemy;

    private void Start()
    {
        enemiesToCreate = NewEnemyWave();
    }
    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, respawn.position, Quaternion.identity);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject chosenEnemy = enemiesToCreate[randomIndex];

        enemiesToCreate.Remove(chosenEnemy);
        return chosenEnemy;
    }

    private List<GameObject> NewEnemyWave()
    {
        List<GameObject> newEnemyList = new List<GameObject>();

        for (int i = 0; i < currentWave.basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }

        for (int i = 0; i < currentWave.fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemy);
        }

        for (int i = 0; i < currentWave.tankyEnemy; i++)
        {
            newEnemyList.Add(tankyEnemy);
        }

        return newEnemyList;
    }
}
