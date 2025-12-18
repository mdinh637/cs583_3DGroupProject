using TMPro;
using UnityEngine;

public class UI_InGame1 : MonoBehaviour
{
    private UI ui;
    private UI_Pause pauseUI;
    private UI_Animator uiAnimator;
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private float waveTimerOffset;

    private void Awake()
    {
        uiAnimator = GetComponent<UI_Animator>();
        ui = GetComponentInParent<UI>();
        pauseUI = GetComponentInChildren<UI_Pause>(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ui.SwitchTo(pauseUI.gameObject);
        }
    }

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
    public void EnableWaveTimerUI(bool enable)
    {
        Transform waveTimerTransform = waveTimerText.transform.parent;
        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new Vector3(0, yOffset);


        uiAnimator.ChangePosition(waveTimerTransform, offset);
        //waveTimerText.transform.parent.gameObject.SetActive(enable);
    }

    public void ForceWaveButton()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.ForceNextWave();
    }
}