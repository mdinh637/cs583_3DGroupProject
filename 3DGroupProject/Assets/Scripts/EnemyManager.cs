using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
}
public class EnemyManager : MonoBehaviour
{
    private GameManager gameManager;    // Section 9 Addition
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [SerializeField] private Transform respawn;
    [SerializeField] public float spawnCooldown;
    public float spawnTimer;

    private List<GameObject> enemiesToCreate;
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private void Awake()
    {

        gameManager = FindAnyObjectByType<GameManager>(); // Section 9 Addition
    }
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
        return newEnemyList;
    }

    //public void TakeDamage(int damage)  // Section 9 Addition
    //{
    //    healthPoints = healthPoints - damage;

    //    if(healthPoints <= 0)
    //    {
    //        Die();
    //    }
    //}
    
    //public void Die()   Section 9 Addition
    //{
    //    myPortal.RemoveActiveEnemy(gameObject);
    //    gameManager.UpdateCurrency(1);
    //    Destroy(gameObject);
    //}

    //public void DestroyEnemy()   Section 9 Addition - Instance of enemy reaching castle (no reward)
    //{
    //    myPortal.RemoveActiveEnemy(gameObject);
    //    Destroy(gameObject);
    //}
}
