using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;

    public void UpdateHealthPointsUI(int value, int maxValue)
    {
        int newValue = value;
        healthPointsText.text = "Health: " + newValue + "/" + maxValue;
    }

    public void UpdateCurrencyUI(int value)
    {
        int newValue = value;
        currencyText.text = "Gold: " + newValue;
    }

    public void UpdateWaveTimerUI(float value) => waveTimerText.text = "seconds: " + value.ToString("00");
}
