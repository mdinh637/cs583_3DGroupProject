using UnityEngine;

public class Tower_Crossbow : Tower
{
    private Crossbow_Visuals visuals;

    [Header("Crossbow Tower Setup")]
    [SerializeField] private Transform bulletPoint;


    protected override void Awake()
    {
        base.Awake();
        visuals = GetComponent<Crossbow_Visuals>();
    }

    protected override void Attack()
    {
        if (currentEnemy == null || bulletPoint == null)
            return;

        Vector3 directionToEnemy = DirectionToEnemyFrom(bulletPoint);

        if (Physics.Raycast(bulletPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy; //orient tower head to face enemy and make it look like loading

            Debug.Log(hitInfo.collider.gameObject.name + " was attacked");
            Debug.DrawLine(bulletPoint.position, hitInfo.point);

            visuals.PlayAttackVFX(bulletPoint.position, hitInfo.point); //enable attack visuals
        }
    }
}
