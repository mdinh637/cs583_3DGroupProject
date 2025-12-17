using UnityEngine;

public class Tower_Crossbow : Tower
{
    private Crossbow_Visuals visuals;

    [Header("Crossbow Tower Setup")]
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private int damage;


    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<Crossbow_Visuals>();
    }

    protected override void Attack()
    {
        //check for valid target and bullet point
        if (currentEnemy == null || bulletPoint == null)
            return;

        Vector3 directionToEnemy = DirectionToEnemyFrom(bulletPoint);

        if (Physics.Raycast(bulletPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy; //orient tower head to face enemy and make it look like loading
            
            Enemy enemyTarget = null;

            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>(); //get IDamageable component from hit object

            //if found, deal damage to enemy
            if (damageable != null)
            {
                damageable.TakeDamage(damage); //deal damage to enemy
                enemyTarget = currentEnemy;
            }
            
            visuals.PlayAttackVFX(bulletPoint.position, hitInfo.point, enemyTarget); //play attack visual

        }
    }
}
