using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Basic, Fast, None}
public class Enemy : MonoBehaviour, IDamagable
{
    private NavMeshAgent agent;

    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private List<Transform> myWaypoints;
    private int nextWaypointIndex;
    public float totalDistance;
    private int currentWaypointIndex;
    private EnemyPortal myPortal;
    private float healthPoints = 10f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    public void SetupEnemy(List<Waypoint> newWaypoints, EnemyPortal myNewPortal)
    {
        myWaypoints = new List<Transform>();
        foreach(var point in newWaypoints)
        {
            myWaypoints.Add(point.transform);
        }

        CollectTotalDistance();

        myPortal = myNewPortal;
    }

    private void Update()
    {
        FaceTarget(agent.steeringTarget);
        // Check whether enemy is close to the current target waypoint
        if (ShouldChangeWaypoint())
        {
            agent.SetDestination(GetNextWaypoint());
        }
    }

    private bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
            return false;

        if (agent.remainingDistance < .5f)
            return true;

        Vector3 currentWaypoint = myWaypoints[currentWaypointIndex].position;
        Vector3 nextWaypoint = myWaypoints[nextWaypointIndex].position;

        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distnaceBeetwenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        
        return distnaceBeetwenPoints > distanceToNextWaypoint;
    }

    // Rotate enemy to face the target waypoint
    private void FaceTarget(Vector3 newTarget)
    {
        // Calculate direction from current position to new target
        Vector3 directionToTarget = newTarget - transform.position;
        directionToTarget.y = 0; // Ignore vertical difference

        // Create a rotation that points forward vector towards the target direction
        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate from current rotation to the new rotation at the defined speed
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime); 

    }

    // Move through the waypoints on the map
    private Vector3 GetNextWaypoint()
    {
        // check if current waypoint index is beyond total amount of waypoints
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            return transform.position;
        }

        // calc distance between prev waypoint and next one
        if(nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(myWaypoints[nextWaypointIndex].position, myWaypoints[nextWaypointIndex - 1].position);
            totalDistance = totalDistance - distance;
        }
        
        Vector3 targetPoint = myWaypoints[nextWaypointIndex].position;
        // find next wayypoint
        nextWaypointIndex++;
        currentWaypointIndex = nextWaypointIndex - 1; // current wapoint

        return targetPoint;
    }

    private void CollectTotalDistance()
    {
        for (int i = 0; i < myWaypoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);
            totalDistance = totalDistance + distance;
        }
    }

    public Vector3 CenterPoint() => centerPoint.position;
    public EnemyType GetEnemyType() => enemyType;

    public void TakeDamage(int damage)
    {
        healthPoints = healthPoints - damage;
        if(healthPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        myPortal.RemoveActiveEnemy(gameObject);
        Destroy(gameObject);
    }
}
