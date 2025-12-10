using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*   Handle spawning in waves with the help of wave config and enemy spawner
*/
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<WaveConfig> waves; // a list of waves, each having their own config
    [SerializeField] private List<EnemySpawner> spawnPoints; // list of locations of where to spawn enemies 

    private int currWaveIdx = 0;
    private bool isRunning = false;

    private void Start()
    {
        // when do we want the wave to start?
        // the player should have time to setup defenses

        // set isRunning to true when player decides or after a timer

        //for testing purposes, start waves automatically 
        BeginWaves();

    }

    public void BeginWaves()
    {
        if (isRunning) return;

        isRunning = true;
        StartCoroutine(RunWavesLoop());
    }

    /* ----- ESSENTIAL FUNCTIONS ----- */

        // intialize, run and end a wave

        // pick type of enemy to spawn 

    private IEnumerator RunWavesLoop()
    {
        while( currWaveIdx < waves.Count && isRunning )
        {
            WaveConfig currWave = waves[currWaveIdx];
            //  run this wave
            yield return StartCoroutine(RunSingleWave(currWave));
            // update to next wave 
            currWaveIdx++;

        }
        isRunning = false;
        
    }

    private IEnumerator RunSingleWave(WaveConfig wave)
    {
        yield return new WaitForSeconds(wave.waveDelay);

        // spawn enemy count 
        for(int i = 0; i < wave.enemyCount; i++)
        {   
            //choose spawn point 
            EnemySpawner spawner = PickSpawnPoint();

            //call from EnemySpawner which only accepts one prefab for now 
            spawner.SpawnEnemy();
        }
    }

    private EnemySpawner PickSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }


}
