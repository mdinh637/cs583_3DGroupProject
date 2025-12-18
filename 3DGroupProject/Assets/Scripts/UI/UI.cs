using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;

    private UI_Settings settingsUI;
    private UI_MainMenu mainMenuUI;
    private UI_InGame inGameUI;

    private void Awake()
    {
        settingsUI = GetComponentInChildren<UI_Settings>(true);
        mainMenuUI = GetComponentInChildren<UI_MainMenu>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);

        SwitchTo(settingsUI.gameObject);
        SwitchTo(mainMenuUI.gameObject);
        //SwitchTo(inGameUI.gameObject);
    }

    public void SwitchTo(GameObject uiToEnable)
    {
        foreach(GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);
    }

    public void ExitButton()
    {
        if(EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}