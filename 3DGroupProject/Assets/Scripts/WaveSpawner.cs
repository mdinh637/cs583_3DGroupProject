using UnityEngine;

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

    }

/* ----- ESSENTIAL FUNCTIONS ----- */

    // intialize, run and end a wave

    // pick type of enemy to spawn 


}
