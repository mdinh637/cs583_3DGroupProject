using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Basic, Fast, None, Tanky
}
public class Enemy : MonoBehaviour , IDamageable
{
    private GameManager gameManager;
    private EnemyPortal myPortal;
    private NavMeshAgent agent;

    public int healthPoints = 2;

    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;
    

    [Header("Movement Settings")]
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private List<Transform> myWaypoints;
    private int nextWaypointIndex;
    private float totalDistance;
    private int currentWaypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);

        gameManager = FindFirstObjectByType<GameManager>();
    }

    // private void Start()
    // {
    //     myWaypoints = FindFirstObjectByType<WaypointManager>().GetmyWaypoints();

    //     CollectTotalDistance();
    // }

    public void SetupEnemy(List<Waypoint> newmyWaypoints,EnemyPortal myNewPortal)
    {
        myWaypoints = new List<Transform>();

        foreach (var point in newmyWaypoints)
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
        if (agent.remainingDistance < 0.5f)
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
        float distanceBetweenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        
        return distanceBetweenPoints > distanceToNextWaypoint;
    }

    //public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;

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

    // Move through the myWaypoints on the map
    private Vector3 GetNextWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            return transform.position;
        }
        Vector3 targetPoint = myWaypoints[nextWaypointIndex].position;

        //if waypoint index is greater than 0, calculate distance between current and previous waypoint
        if (nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(myWaypoints[nextWaypointIndex].position, myWaypoints[nextWaypointIndex -1].position);
            totalDistance -= distance; //reduce total distance as myWaypoints are reached
        }

        nextWaypointIndex++;

        return targetPoint;
    }

    public Vector3 CenterPoint() => centerPoint.position; //get center point of enemy where turret head will aim at, looks cleaner
    public EnemyType GetEnemyType() => enemyType; //get enemy type for turrts to target

    public virtual void TakeDamage(int damage)
    {
        //reduce health points by damage amount
        healthPoints = healthPoints - damage;

        //destroy enemy if health points are 0 or less, mimics killing them
        if (healthPoints <= 0) 
        {
            Die();
        }
    }

    public float DistanceToFinishLine()
    {
        //if agent is null, return max value
        if (agent == null)
            return float.MaxValue;
        //if agent isnt active or on navmesh or has no path, return max value
        if (!agent.enabled || !agent.isOnNavMesh || !agent.hasPath)
            return float.MaxValue;

        return totalDistance + agent.remainingDistance; //return sum of total distance and remaining distance to next waypoint
    }

    private void CollectTotalDistance()
    {
        //sets initial spot of waypoint
        for (int i = 0; i < myWaypoints.Count - 1; i++)
        {
            float distance = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);
            totalDistance += distance; //sum up total distance for all myWaypoints
        }
    }

    private void Die()
    {
        gameManager.UpdateCurrency(1);
        Destroy(gameObject);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
