using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower_Cannon : Tower
{
    [Header("Cannon Tower Setup")]
    [SerializeField] private float timeToTarget = 1.5f; //time for projectile to reach target
    [SerializeField] private GameObject projectilePrefab; //projectile prefab to spawn when attacking

    protected override void Attack()
    {
        base.Attack();

        Vector3 velocity = CalculateLaunchVelocity(); //calculate launch velocity needed to hit enemy
        GameObject newProjectile = Instantiate(projectilePrefab, gunPoint.position, Quaternion.identity); //spawn projectile at gun point
        newProjectile.GetComponent<Projectile_Cannon>().SetupProjectile(velocity); //setup projectile with launch velocity
    }

    protected override Enemy FindEnemyWithinRange()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(rangeOrigin.position, attackRange, whatIsEnemy); //get enemies within range
        Enemy bestTarget = null; //best target to return
        int maxNearbyEnemies = 0; //max number of nearby enemies found initially set to 0

        //for each enemy around tower
        foreach (Collider enemy in enemiesAround)
        {
            int amountOfEnemiesAround = EnemiesAroundEnemy(enemy.transform); //get number of enemies around this enemy

            //if this enemy has more nearby enemies than previous best, set as new best target
            if (amountOfEnemiesAround > maxNearbyEnemies)
            {
                maxNearbyEnemies = amountOfEnemiesAround;//set new max
                bestTarget = enemy.GetComponent<Enemy>(); //set new best target
            }
        }

        return bestTarget; //return best target found
    }

    private int EnemiesAroundEnemy(Transform enemyToCheck)
    {
        Collider[] enemiesAround = Physics.OverlapSphere(enemyToCheck.position, 1f, whatIsEnemy); //get enemies around target enemy
        return enemiesAround.Length; //return number of enemies found
    }

    protected override void HandleRotation()
    {
        if (currentEnemy == null) return; //no enemy to rotate towards

        RotateBodyTowardsEnemy(); //rotate the body towards enemy
        FaceLaunchDirection(); //make tower head face the launch direction
    }

    private void RotateBodyTowardsEnemy()
    {
        if (towerBody == null) return; //no body to rotate

        Vector3 directionToEnemy = DirectionToEnemyFrom(towerBody); //get direction to enemy
        directionToEnemy.y = 0; //keep only horizontal direction
        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy); //calc rotation needed to look at enemy
        towerBody.rotation = Quaternion.Slerp(towerBody.rotation, lookRotation, Time.deltaTime * rotationSpeed); //smoothly rotate body towards enemy
    }

    private void FaceLaunchDirection()
    {
        Vector3 attackDirection = CalculateLaunchVelocity(); //get launch velocity
        Quaternion lookRotation = Quaternion.LookRotation(attackDirection); //calc rotation needed to look at launch direction
        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles; //calc smoothed rotation
        towerHead.rotation = Quaternion.Euler(rotation.x, towerHead.eulerAngles.y, 0); //make tower head face launch direction
    }

    //gives projectile launch velocity needed to hit current enemy
    private Vector3 CalculateLaunchVelocity()
    {
        Vector3 direction = currentEnemy.CenterPoint() - gunPoint.position; //direction to target
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z); //horizontal direction
        Vector3 velocityXZ = directionXZ / timeToTarget; //horizontal velocity
        float yVelocity = (direction.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget; //vertical velocity
        Vector3 launchVelocity = velocityXZ + (Vector3.up * yVelocity); //total launch velocity

        return launchVelocity;
    }
}
