using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

    /// <summary>
    /// FSM States:
    /// RandomFlight
    /// ChaseFlight
    /// Attack?
    /// Flee?
    /// Group?
    /// </summary>
    private enum State
    {
        RandomFlight,
        ChasePlayer
    }
    
    private State currentState;

    [Header("----Random flight----")]
    [Tooltip("Add waypoints for random flight here")]
    public Transform[] waypoints;
    private Transform currentWaypoint;
    public float waypointThreshold = 1f;
    [Tooltip("Speed for random flight")]
    public float randomFlightSpeed = 5f;
    public float directionChangeInterval = 30f;
    private float directionChangeTimer;
    private Vector3 currentDirection;


    [Header("----ChasePlayer flight----")]
    private Transform playerTransform;
    [Tooltip("Speed for chasing")]
    public float chaseSpeed = 8f;
    [Tooltip("Distance to engage player")]
    public float detectionRadius = 20f;

    [Header("----Movement----")]
    public float maxSpeed = 10f;
    public float rotationSpeed = 5f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTransform = player.transform;
        }
        else
        Debug.Log("Player not found. Check player is tagged as Player");

        currentState = State.RandomFlight;
        SelectRandomWaypoint();

        directionChangeTimer = directionChangeInterval;
    }

    private void Update()
    {
        switch(currentState)
        {
            case State.RandomFlight:
                HandleRandomFlight();
                break;
            
            case State.ChasePlayer:
                HandleChasePlayer();
                break;
        }

        if( rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    /*private void HandleRandomFlight()
    {

        Vector3 direction = (currentWaypoint.position - transform.position).normalized;
        Vector3 expectedVelocity = direction * randomFlightSpeed;

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, expectedVelocity, Time.fixedDeltaTime * 2f);

        if(rb.linearVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }

        if(Vector3.Distance(transform.position, currentWaypoint.position) < waypointThreshold)
        {
            SelectRandomWaypoint();
        }

        changeDirectionTimer -= Time.fixedDeltaTime;
        if (changeDirectionTimer <= 0f)
        {
            SelectRandomWaypoint();
            changeDirectionTimer = changeDirectionInterval;
        }

        // Check for player detection
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                currentState = State.ChasePlayer;
            }
        }
    }*/

    private void HandleRandomFlight()
    {
        // Apply current direction
        rb.linearVelocity = currentDirection * randomFlightSpeed;

        // Timer for changing direction
        directionChangeTimer -= Time.fixedDeltaTime;
        if (directionChangeTimer <= 0f)
        {
            currentDirection = GetRandomDirection();
            directionChangeTimer = directionChangeInterval;
        }

        // Smooth rotation towards movement direction
        if (rb.linearVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2f);
        }

        // Check for player detection
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                SwitchState(State.ChasePlayer);
            }
        }
    }

    private void HandleChasePlayer()
    {
        if (playerTransform == null)
            return;

        // Calculate direction towards the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Vector3 desiredVelocity = direction * chaseSpeed;

        // Smoothly interpolate current velocity towards desired velocity
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, Time.fixedDeltaTime * 2f);

        // Rotate towards movement direction smoothly
        if (rb.linearVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }

        // Check if player is out of detection radius
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > detectionRadius)
        {
            SwitchState(State.RandomFlight);
            SelectRandomWaypoint();
        }
    }

    private void SwitchState(State state)
    {
        currentState = state;
    }
    private void SelectRandomWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        int index = Random.Range(0, waypoints.Length);
        currentWaypoint = waypoints[index];
    }

    private Vector3 GetRandomDirection()
    {
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Clamp(randomDir.y, -0.5f, 0.5f);
        return randomDir.normalized;
    }

    // Optional: Visualize detection radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with player
            // Apply damage, effects, etc.

            // Return to pool instead of destroying
            EnemyPool.Instance.ReturnEnemy(gameObject);
        }
    }
}
