using UnityEngine;
using System.Collections.Generic;


public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;

    [SerializeField] protected float attackCooldown = 1f;
    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 3f;
    [SerializeField] protected Transform rangeOrigin;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected EnemyType enemyPriorityType = EnemyType.None;
    private bool canRotate;

    [Space]
    [Tooltip("Enable to allow tower to change target between attacks")]
    [SerializeField] private bool dynamicTargetChange;
    private float targetCheckInterval = 0.1f;
    private float lastTimeCheckedTarget;

    protected virtual void Awake()
    {
        EnableRotation(true);
    }

    protected virtual void Update()
    {
        UpdateTargetIfNeeded(); //check for new target if dynamic targeting is enabled

        //if no current enemy, find a new one
        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinRange();
            return;
        }

        //attack if able
        if (CanAttack()) Attack();

        //check if current enemy is still within range
        if (Vector3.Distance(currentEnemy.CenterPoint(), rangeOrigin.position) > attackRange) 
        {
            currentEnemy = null; //clear current enemy if out of range
            return;
        }

        RotateTowardsEnemy(); //rotate tower head towards current enemy
    }

    private void UpdateTargetIfNeeded()
    {
        if (dynamicTargetChange == false) return;

        if (Time.time > lastTimeCheckedTarget + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            currentEnemy = FindEnemyWithinRange();
        }
    }

    protected virtual void Attack()
    {
    }

    protected bool CanAttack()
    {
        //check if tower can attack based on cooldown
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    protected Enemy FindEnemyWithinRange()
    {
        List<Enemy> priorityTargets = new List<Enemy>(); //list of enemies that are priority targets
        List<Enemy> possibleTargets= new List<Enemy>(); //list of all possible targets
        Collider[] enemiesAround = Physics.OverlapSphere(rangeOrigin.position, attackRange, whatIsEnemy); //get all enemies within range

        //for each enemy around turret
        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            EnemyType newEnemyType = newEnemy.GetEnemyType();

            //if new enemy isnt null
            if (newEnemy != null)
            {
                if (newEnemyType == enemyPriorityType)
                {
                    priorityTargets.Add(newEnemy); //add to priority targets
                } 
                else
                {
                    possibleTargets.Add(newEnemy); //add to possible targets
                }
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

    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }
    protected virtual void RotateTowardsEnemy()
    {
        if (canRotate == false) return;
        if (currentEnemy == null) return;

        Vector3 directonToEnemy = DirectionToEnemyFrom(towerHead); //get direction to enemy
        Quaternion lookRotation = Quaternion.LookRotation(directonToEnemy); //calc rotation needed to look at enemy
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles; //calc smoothed rotation
        towerHead.rotation = Quaternion.Euler(rotation); //make tower head rotate towards enemy
    }

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
