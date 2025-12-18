using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    private void Update()
    {
        //if pressing esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); //go to pause menu
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused; //default not paused

        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f; //stop timer, paused game
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reloads scene
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu"); //goes back to main menu
    }
}

