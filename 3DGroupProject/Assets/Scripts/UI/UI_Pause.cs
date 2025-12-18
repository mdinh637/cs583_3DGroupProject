using Unity.VisualScripting;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    private UI ui;
    private UI_InGame inGameUI;

    [SerializeField] private GameObject[] pauseUIElements;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        inGameUI = GetComponentInChildren<UI_InGame>(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ui.SwitchTo(inGameUI.gameObject);
        }
    }

    public void SwitchPauseUIElements(GameObject elementToEnable)
    {
        foreach(GameObject uiElement in pauseUIElements)
        {
            uiElement.SetActive(false);
        }

        elementToEnable.SetActive(true);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
