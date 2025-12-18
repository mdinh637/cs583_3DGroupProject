using UnityEngine;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour
{
    [Header("Castle Health")]
    [SerializeField] private int maxHealth = 10; //starting health
    private int currentHealth; //current health

    private void Start()
    {
        currentHealth = maxHealth;  //initialize current health
    }

    //detect collision with enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1); //reduce health by 1 for each enemy collision
            Destroy(other.gameObject); //remove enemy on collision
        }
    }

    //reduce castle health
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Castle HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            GameOver(); //trigger game over if health is 0 or below
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        //stop the survival timer and save best time
        FindFirstObjectByType<GameTimer>()?.StopTimer();

        Time.timeScale = 1f;
        //reload start menu scene
        SceneManager.LoadScene("StartMenu");
    }
}

