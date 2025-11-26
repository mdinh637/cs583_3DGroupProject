using UnityEngine;
using System.Collections;
/*
*   Handle what and where enemy spawns 
*   Add as component of an enemy spawnpoint empty object in scene
*
*   Used by a wave spawner and a wave config
* 
*/
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // attach enemy prefab here 

    public void SpawnEnemy()
    {
        // spawn an enemy object on spawnpoint 
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

}
