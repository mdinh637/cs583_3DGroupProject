using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;

    public void SwitchTo(GameObject uiToEnable)
    {
        foreach(GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);
    }

    public void LoadForestLevel()
    {
        SceneManager.LoadScene("Test Map");
    }

    public void ExitButton()
    {
        if(EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}