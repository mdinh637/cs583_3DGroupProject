using UnityEngine;

public class Castle : MonoBehaviour
{
    private GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            if(gameManager == null)
                gameManager = FindAnyObjectByType<GameManager>();

            if(gameManager != null)
                gameManager.UpdateHp(-1);
        }
    }
}
