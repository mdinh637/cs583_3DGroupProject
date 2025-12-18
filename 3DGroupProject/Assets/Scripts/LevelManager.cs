using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//drag onto LevelManager GameObject

public class LevelManager : MonoBehaviour
{
    public List<TowerUnlockData> towerUnlocks;

    private void InitializeTowerData()
    {
        towerUnlocks.Clear();
        towerUnlocks.Add(new TowerUnlockData("Crossbow", false));
        towerUnlocks.Add(new TowerUnlockData("Cannon", false));
    }
}

[System.Serializable]
public class TowerUnlockData
{
    public string towerName;
    public bool unlocked;

    public TowerUnlockData(string newTowerName, bool newUnlockedSTatus)
    {
        towerName = newTowerName;
        unlocked = newUnlockedSTatus;
    }
}
