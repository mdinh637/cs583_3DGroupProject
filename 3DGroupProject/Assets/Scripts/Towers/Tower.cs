using UnityEngine;
using System.Collections.Generic;


public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [SerializeField] protected float attackCooldown = 1f;
    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float attackRange = 3f;
    [SerializeField] protected Transform rangeOrigin;
    [SerializeField] protected LayerMask whatIsEnemy;
    private bool canRotate;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Update()
    {
        //if no current enemy, find a new one
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }

        //attack if able
        if (CanAttack()) Attack();

        //check if current enemy is still within range
        if (Vector3.Distance(currentEnemy.position, rangeOrigin.position) > attackRange) 
        {
            currentEnemy = null; //clear current enemy if out of range
            return;
        }

        RotateTowardsEnemy(); //rotate tower head towards current enemy
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

    protected Transform FindRandomEnemyWithinRange()
    {
        List<Transform> possibleTargets= new List<Transform>();
        Collider[] enemiesAround = Physics.OverlapSphere(rangeOrigin.position, attackRange, whatIsEnemy); //get all enemies within range

        //debug log all enemies found
        foreach (Collider enemy in enemiesAround)
        {
            possibleTargets.Add(enemy.transform);
        }

        int randomIndex = Random.Range(0, possibleTargets.Count); //get random index from list of possible targets

        if (possibleTargets.Count <= 0)
        {
            return null; //return null if no enemies found
        }

        return possibleTargets[randomIndex]; //return random enemy transform
    }

    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }
    protected virtual void RotateTowardsEnemy()
    {
        if (canRotate == false) return;
        if (currentEnemy == null) return;

        Vector3 directonToEnemy = currentEnemy.position - towerHead.position; //calc vector direction of tower aggro to current enemy
        Quaternion lookRotation = Quaternion.LookRotation(directonToEnemy); //calc rotation needed to look at enemy
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles; //calc smoothed rotation
        towerHead.rotation = Quaternion.Euler(rotation); //make tower head rotate towards enemy
    }

    protected Vector3 DirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.position - startPoint.position).normalized;
    }

    protected virtual void OnDrawGizmos()
    {
        if (rangeOrigin == null) return;
        
        Gizmos.DrawWireSphere(rangeOrigin.position, attackRange);
    }
}
