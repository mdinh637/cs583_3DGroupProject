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
    public List<EnemyPortal> enemyPortals;
    [SerializeField] private WaveDetails currentWave;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private void Start()
    {
        SetupNextWave();
    }
    private void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsOfType<EnemyPortal>());
    }

    [ContextMenu("Setup Next Wave")]

    private void SetupNextWave()
    {
        List<GameObject> newEnemies = NewEnemyWave();
        int portalIndex = 0;

        // split up enemies into the portals 
        for(int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToReceiveEnemy = enemyPortals[portalIndex];

            portalToReceiveEnemy.AddEnemy(enemyToAdd);
            portalIndex++;

            // make sure enemies are divided to correct amount of portals on field 
            if(portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
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
}
