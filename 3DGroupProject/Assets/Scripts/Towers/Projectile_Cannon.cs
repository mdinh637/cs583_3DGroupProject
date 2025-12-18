using UnityEngine;

public class Projectile_Cannon : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetupProjectile(Vector3 newVelocity)
    {
        rb.linearVelocity = newVelocity; //set the projectile velocity to the calc of launch vlocity
    }
}
