using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;

    protected bool towerActive = true;
    protected Coroutine deactivatedTowerCo;
    protected GameObject currentEmpFx;

    [SerializeField] private bool dynamicTargetChange;
    [SerializeField] protected float attackCooldown = 1f;
    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected Transform gunPoint;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 3f;
    [SerializeField] protected Transform rangeOrigin;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected LayerMask whatIsTargetable;
    [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;

    private float targetCheckInterval = 0.1f;
    private float lastTimeCheckedTarget;

    [Header("SFX Details")]
    [SerializeField] protected AudioSource attackSfx;

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        LooseTargetIfNeeded(); //check if current target is still valid
        UpdateTargetIfNeeded(); //check for new target if dynamic targeting is enabled
        HandleRotation(); //handle tower rotation towards enemy

        if (CanAttack()) Attack(); //attack if able
    }

    public void DeactivateTower(float duration, GameObject empFxPrefab)
    {
        //start emp vfx and disable tower for duration
        if (deactivatedTowerCo != null)
            StopCoroutine(deactivatedTowerCo);

        //destroy current emp vfx if exists
        if (currentEmpFx != null)
            Destroy(currentEmpFx);

        currentEmpFx = Instantiate(empFxPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity); //spawn emp vfx
        deactivatedTowerCo = StartCoroutine(DeactivateTowerCo(duration)); //start disable coroutine
    }

    private IEnumerator DeactivateTowerCo(float duration)
    {
        towerActive = false; //disable tower

        yield return new WaitForSeconds(duration); //wait for duration

        towerActive = true; //enable tower
        lastTimeAttacked = Time.time; //reset last attack time to prevent immediate attack
        Destroy(currentEmpFx); //destroy emp vfx
    }

    private void LooseTargetIfNeeded()
    {
        if (currentEnemy == null) return;

        if (Vector3.Distance(currentEnemy.CenterPoint(), rangeOrigin.position) > attackRange) currentEnemy = null; //clear current enemy if out of range
    }

    private void UpdateTargetIfNeeded()
    {
        //if no current enemy, find one
        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinRange();
            return;
        }

        if (dynamicTargetChange == false) return; //only update target if dynamic targeting is enabled

        if (Time.time > lastTimeCheckedTarget + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            currentEnemy = FindEnemyWithinRange();
        }
    }

    protected virtual void Attack()
    {
        lastTimeAttacked = Time.time; //update last attack time
    }

    protected bool CanAttack()
    {
        return Time.time > lastTimeAttacked + attackCooldown && currentEnemy != null; //can atk if cd is over and there is a current enemy
    }

    protected virtual Enemy FindEnemyWithinRange()
    {
        List<Enemy> priorityTargets = new List<Enemy>(); //list of enemies that are priority targets
        List<Enemy> possibleTargets= new List<Enemy>(); //list of all possible targets
        Collider[] enemiesAround = Physics.OverlapSphere(rangeOrigin.position, attackRange, whatIsEnemy); //get all enemies within range

        //for each enemy around turret
        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>(); //get enemy component

            if (newEnemy == null) continue; //skip if no enemy component found

            EnemyType newEnemyType = newEnemy.GetEnemyType(); //get enemy type

            //categorize enemy based on priority type
            if (newEnemyType == enemyPriorityType)
            {
                priorityTargets.Add(newEnemy); //add to priority targets if matches priority type
            }
            else
            {
                possibleTargets.Add(newEnemy); //add to possible targets if not priority type
            }
        }

        //return most advanced enemy from priority targets if any exist, else return most advanced from possible targets
        if (priorityTargets.Count > 0)
        {
            return GetMostAdvancedEnemy(priorityTargets);
        }

        if (possibleTargets.Count > 0)
        {
            return GetMostAdvancedEnemy(possibleTargets);
        }

        return null; //return null if no enemies found
    }

    private Enemy GetMostAdvancedEnemy(List<Enemy> targets)
    {
        Enemy mostAdvancedEnemy = null;
        float minRemainingDistance = float.MaxValue;

        //find most advanced enemy towards finish line
        foreach (Enemy enemy in targets)
        {
            float remainingDistance = enemy.DistanceToFinishLine();
            //if enemy is more adv than current adv enemy
            if (remainingDistance < minRemainingDistance)
            {
                minRemainingDistance = remainingDistance; //update min distance
                mostAdvancedEnemy = enemy; //assign most advanced enemy
            }
        }

        return mostAdvancedEnemy;
    }

    protected virtual void HandleRotation()
    {
        RotateTowardsEnemy();
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (currentEnemy == null || towerHead == null) return;

        Vector3 directonToEnemy = DirectionToEnemyFrom(towerHead); //get direction to enemy
        Quaternion lookRotation = Quaternion.LookRotation(directonToEnemy); //calc rotation needed to look at enemy
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles; //calc smoothed rotation
        towerHead.rotation = Quaternion.Euler(rotation); //make tower head rotate towards enemy
    }

    public float GetAttackRange() => attackRange; //return attack range

    protected Vector3 DirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.CenterPoint() - startPoint.position).normalized;
    }

    protected virtual void OnDrawGizmos()
    {
        if (rangeOrigin == null) return;
        
        Gizmos.DrawWireSphere(rangeOrigin.position, attackRange);
    }
}
