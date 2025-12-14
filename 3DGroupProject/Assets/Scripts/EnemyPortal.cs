using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private List<Waypoint> waypointList;
    [Space]
    [SerializeField] public float spawnCooldown;
    public float spawnTimer;

    private List<GameObject> enemiesToCreate = new List<GameObject>();

    private void Awake()
    {
        CollectWayPoints();
    }


    private void Update()
    {
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
        enemyScript.SetupEnemy(waypointList);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject chosenEnemy = enemiesToCreate[randomIndex];

        enemiesToCreate.Remove(chosenEnemy);
        return chosenEnemy;
    }

    public void AddEnemy(GameObject enemyToAdd) => enemiesToCreate.Add(enemyToAdd);

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
}
