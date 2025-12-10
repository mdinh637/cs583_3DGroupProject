using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpd = 3f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private Transform castleTarget; 

    // pass in castle information 

    void Start()
    {
        // find target 
        GameObject castle = GameObject.FindGameObjectWithTag("Castle"); // make sure to add tag to castle game object
        castleTarget = castle.transform;
    }

    void Update()
    {
        /* The loop */
        // target moves towards target at every frame 

        // direction from this enemy to the casle 
        Vector3 toCastle = castleTarget.position - transform.position;
        float distanceToCastale = toCastle.magnitude;

        // if target reached, attack
        if( distanceToCastale <= attackRange )
        {
            // HitCastle();         // not implemented yet 
            Destroy(gameObject);
            
        }

        Vector3 direction = toCastle.normalized;

        // move towards castle object 
        Vector3 movement = direction * movementSpd * Time.deltaTime;
        transform.position += movement; 

    }
}
