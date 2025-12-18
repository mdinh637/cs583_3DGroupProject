using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void UpdateHp(int value)
    {
        currentHp += value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }
}
