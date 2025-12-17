using UnityEngine;

public class Castle : MonoBehaviour
{
    private GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            //other.GetComponent<Enemy>().TakeDamage(9999);

            if(gameManager == null)
            {
                gameManager = FindAnyObjectByType<GameManager>();
            }

            if(gameManager != null)
            {
                gameManager.UpdateHp(1);
            }
        }
    }
}
