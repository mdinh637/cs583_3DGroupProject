using UnityEngine;
using System.Collections.Generic;

/*
*   Hold info about each wave
*   Used by wave spawner 
*
*/

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Scriptable Objects/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyPrefabs; // for determining what kind of enemies spawn in which waves
    [SerializeField] public float waveDelay = 5f; // can tune, controls how long it takes before spawning waves 
    [SerializeField] public float spawnInterval = 1.25f; // can tune, determines spawn interval of each enemy in a wave 
    [SerializeField] public int enemyCount = 5;  // can tune, num enemies per wave 

    /* Can add additional fields for special waves, such as boss waves which can spawn every couple of waves */

}
