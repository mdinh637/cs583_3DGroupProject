using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private Transform[] waypoint;
    private int waypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);

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
        if (waypointIndex >= waypoint.Length)
        {
            return transform.position;
        }
        Vector3 targetPoint = waypoint[waypointIndex].position;
        waypointIndex++;

        return targetPoint;
    }
}
