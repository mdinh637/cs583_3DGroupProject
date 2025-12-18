using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public GridBuilder nextGrid; // for creating dynamically changing layouts
    public EnemyPortal[] newPortals; 
    public int basicEnemy;
    public int fastEnemy;
}
public class WaveManager : MonoBehaviour
{
    private UI_InGame inGameUI;
    [SerializeField] private GridBuilder currentGrid;
    public bool waveCompleted;
    public float timeBetweenWaves = 10;
    public float waveTimer;
    private List<EnemyPortal> enemyPortals;
    [SerializeField] private WaveDetails[] levelWaves;
    private int waveIndex;
    private float checkInterval = 0.5f;
    private float nextCheckTime;

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
        inGameUI = FindFirstObjectByType<UI_InGame>(FindObjectsInactive.Include);
    }

    private void Update()
    {
        HandleWaveCompletion();
        HandleWaveTiming();
    }

    private void HandleWaveCompletion()
    {
        if (!ReadyToCheck())
            return;

        if (!waveCompleted && AllEnemiesDefeated())
        {
            CheckForNewLevelLayout();
            waveCompleted = true;
            waveTimer = timeBetweenWaves;
            inGameUI.EnableWaveTimerUI(true);
        }
    }
    // only check every interval
    private bool ReadyToCheck()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            return true;
        }
        return false;
    }

    private void HandleWaveTiming()
    {
        if(waveCompleted)
        {
            waveTimer -= Time.deltaTime;
            inGameUI.UpdateWaveTimerUI(waveTimer);

            if(waveTimer <= 0)
            {
                inGameUI.EnableWaveTimerUI(false);
                SetupNextWave();
            }
        }
    }

    public void ForceNextWave()
    {
        if(AllEnemiesDefeated() == false)
        {
            Debug.LogWarning("Can't force while there are enemies in the game");
            return;
        }

        inGameUI.EnableWaveTimerUI(false);
        SetupNextWave();
    }
    [ContextMenu("Setup Next Wave")]

    private void SetupNextWave()
    {
        List<GameObject> newEnemies = NewEnemyWave();
        int portalIndex = 0;

        if(newEnemies == null)
        {
            Debug.LogWarning("No wave to setup");
            return;
        }

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
        waveCompleted = false;
    }

    private List<GameObject> NewEnemyWave()
    {
        // a check to see if all waves have already been completed
        if(waveIndex >= levelWaves.Length)
        {
            //Debug.LogWarning("You have no more waves");
            return null;
        }
        List<GameObject> newEnemyList = new List<GameObject>();
        for (int i = 0; i < levelWaves[waveIndex].basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }

        for (int i = 0; i < levelWaves[waveIndex].fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemy);
        }
        waveIndex++;
        return newEnemyList;
    }

    private void CheckForNewLevelLayout()
    {
        if(waveIndex >= levelWaves.Length)
        {
            return;
        }

        WaveDetails nextWave = levelWaves[waveIndex];
        if(nextWave.nextGrid != null)
        {
            UpdateLevelTiles(nextWave.nextGrid);
            EnableNewPortals(nextWave.newPortals);
            Debug.Log("Level should be updated");
        }

        currentGrid.UpdateNavMesh();
    }

    private void UpdateLevelTiles(GridBuilder nextGrid)
    {

        List<GameObject> grid = currentGrid.GetTileSetup();
        // the updating layout
        List<GameObject> newGrid = nextGrid.GetTileSetup(); 

        // for each tile, replace with new tile from new layout
        for (int i = 0; i < grid.Count; i++)
        {   
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot newTile = newGrid[i].GetComponent<TileSlot>();

            bool shouldBeUpdated = currentTile.GetMesh() != newTile.GetMesh() || 
                                    currentTile.GetMaterial() != newTile.GetMaterial() ||
                                    currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
                                    currentTile.transform.rotation != newTile.transform.rotation;
            if(shouldBeUpdated)
            {
                currentTile.gameObject.SetActive(false);

                newTile.gameObject.SetActive(true);
                newTile.transform.parent = currentGrid.transform;

                grid[i] = newTile.gameObject;
                Destroy(currentTile.gameObject);
            }
        }
    }

    private void EnableNewPortals(EnemyPortal[] newPortals)
    {
        foreach(EnemyPortal portal in newPortals)
        {
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }
    // used by wave manager to check if all enemies are defeated
    private bool AllEnemiesDefeated()
    {
        foreach(EnemyPortal portal in enemyPortals)
        {
            if(portal.HasEnemiesRemaining())
            {
                return false;
            }
        }

        return true;
    }

    public WaveDetails[] GetLevelWaves() => levelWaves;
}
