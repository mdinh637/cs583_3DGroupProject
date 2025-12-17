using UnityEngine;

public class Tower_Crossbow : Tower
{
    private Crossbow_Visuals visuals;

    [Header("Crossbow Tower Setup")]
    [SerializeField] private int damage;


    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<Crossbow_Visuals>();
    }

    protected override void Attack()
    {
        base.Attack(); //call base attack to update last attack time

        //check for valid target and bullet point
        if (currentEnemy == null || gunPoint == null)
            return;

        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity, whatIsTargetable))
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
            
            visuals.PlayAttackVFX(gunPoint.position, hitInfo.point); //play attack visual
        }
    }
}
