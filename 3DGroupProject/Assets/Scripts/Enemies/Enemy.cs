using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Basic, Fast, None, Tanky
}
public class Enemy : MonoBehaviour , IDamageable
{
    private GameManager gameManager;
    private NavMeshAgent agent;

    public int healthPoints = 2;

    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;

    [Header("Movement Settings")]
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private float totalDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);

        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Start()
    {
        waypoints = FindFirstObjectByType<WaypointManager>().GetWaypoints();

        CollectTotalDistance();
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
        if (waypointIndex >= waypoints.Length)
        {
            return transform.position;
        }
        Vector3 targetPoint = waypoints[waypointIndex].position;

        //if waypoint index is greater than 0, calculate distance between current and previous waypoint
        if (waypointIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex -1].position);
            totalDistance -= distance; //reduce total distance as waypoints are reached
        }

        waypointIndex++;

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
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance += distance; //sum up total distance for all waypoints
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
