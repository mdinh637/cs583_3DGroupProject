using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    [Header("Enemies")]
    public int basicEnemy; //number of basic enemies in wave
    public int fastEnemy; //number of fast enemies in wave

    [Header("Level Changes")]
    public GridBuilder nextGrid; //for creating dynamically changing layouts
    public EnemyPortal[] newPortals; //new portals to add for this wave
}
