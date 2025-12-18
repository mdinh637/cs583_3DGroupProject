using UnityEngine;

public class Projectile_Cannon : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float damageRadius;//radius of damage on impact
    [SerializeField] private LayerMask whatIsEnemy; //layer mask for what is considered an enemy
    [SerializeField] private float damage; //damage dealt on impact

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetupProjectile(Vector3 newVelocity, float newDamage)
    {
        rb.linearVelocity = newVelocity; //set the projectile velocity to the calc of launch vlocity
        damage = newDamage; //set damage value
    }

    private void DamageEnemiesAround()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, damageRadius, whatIsEnemy); //get enemies within damage radius

        foreach (Collider enemy in enemiesAround)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>(); //get damageable component

            if (damageable != null) 
            {
                int newDamage = Mathf.RoundToInt(damage); //round damage to int
                damageable.TakeDamage(newDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageEnemiesAround(); //deal damage to enemies around impact point
        Destroy(gameObject); //destroy projectile on impact
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
