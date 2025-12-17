using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private CameraController camController;

    [Header("Keyboard Sensitivity")]
    [SerializeField] private Slider keyboardSensSlider;
    [SerializeField] private string keyboardSensParameter = "keyboardSens";
    [SerializeField] private TextMeshProUGUI keyboardSensText;

    [SerializeField] private float minKeyboardSensitivity = 60;
    [SerializeField] private float maxKeyboardSensitivity = 240;

    [Header("Mouse Sensitivity")]
    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private string mouseSensParameter = "mouseSens";
    [SerializeField] private TextMeshProUGUI mouseSensText;

    [SerializeField] private float minMouseSense = 1;
    [SerializeField] private float maxMouseSens = 10;

    private void Awake()
    {
        camController = FindFirstObjectByType<CameraController>();
    }

    public void MouseSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minMouseSense, maxMouseSens, value);
        camController.AdjustMouseSensitivity(newSensitivity);

        mouseSensText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void KeyboardSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minKeyboardSensitivity, maxKeyboardSensitivity, value);
        camController.AdjustKeyboardSensitivity(newSensitivity);

        keyboardSensText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSensParameter, keyboardSensSlider.value);
        PlayerPrefs.SetFloat(mouseSensParameter, mouseSensSlider.value);
    }

    private void OnEnable()
    {
        keyboardSensSlider.value = PlayerPrefs.GetFloat(keyboardSensParameter, .5f);
        mouseSensSlider.value = PlayerPrefs.GetFloat(mouseSensParameter, .6f);
    }
}
