using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private List<Waypoint> waypointList;
    [Space]
    [SerializeField] public float spawnCooldown;
    public float spawnTimer;
    [SerializeField] private Transform spawnPoint;

    private List<GameObject> enemiesToCreate = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private void Awake()
    {
        CollectWayPoints();
    }


    private void Update()
    {
        Debug.Log("Portal running. Queue: " + enemiesToCreate.Count);
        if(CanMakeNewEnemy())
        {
            CreateEnemy();
        }
    }

    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0 && enemiesToCreate.Count >0)
        {
            spawnTimer = spawnCooldown;
            return true;
        }
        return false;
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetupEnemy(waypointList, this);

        activeEnemies.Add(newEnemy);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject chosenEnemy = enemiesToCreate[randomIndex];

        enemiesToCreate.Remove(chosenEnemy);
        return chosenEnemy;
    }

    public void AddEnemy(GameObject enemyToAdd) => enemiesToCreate.Add(enemyToAdd);
    public List<GameObject> GetActiveEnemies() => activeEnemies;
    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if(activeEnemies.Contains(enemyToRemove))
        {
            activeEnemies.Remove(enemyToRemove);
        }
    }

    private void CollectWayPoints()
    {
        waypointList = new List<Waypoint>();

        foreach(Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();

            if(waypoint != null)
            {
                waypointList.Add(waypoint);
            }
        }
    }
// a check useful for dynamically changing levels
    public bool HasEnemiesRemaining()
{
    // true if there is anything still queued OR alive
    return enemiesToCreate.Count > 0 || activeEnemies.Count > 0;
}
}
