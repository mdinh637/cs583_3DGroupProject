using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TowerUnlockData towerData;
}

[System.Serializable]
public class TowerUnlockData
{
    public string towerName;
    public bool unlocked;
}
