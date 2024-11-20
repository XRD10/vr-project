using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    /// <summary>
    /// FSM States:
    /// RandomFlight - Enemy moves randomly.
    /// ChasePlayer - Enemy chases the player.
    /// Returning - Enemy returns to spawn location.
    /// EvadePlayer - Enemy evades the player.
    /// </summary>
    private enum State
    {
        RandomFlight,
        ChasePlayer,
        Returning,
        EvadePlayer
    }

    [Header("----Current State----")]
    [SerializeField]
    private State currentState;

    [Header("----Limit distance----")]
    [SerializeField]
    private float maxDistanceFromSpawn;
    [SerializeField]
    private float minDistanceFromSpawn;
    private Vector3 spawnLocation;

    [Header("----Random flight----")]
    public float directionChangeInterval;
    private float directionChangeTimer;
    [SerializeField]
    private Vector3 currentDirection;

    [Header("----ChasePlayer flight----")]
    private Transform playerTransform;
    public float detectionRadius;

    [Header("----EvadePlayer----")]
    public float evadeSpeed;
    public float evadeThreshold;
    public float evadeDistance;
    public float facingAngle;
    private bool hasEvaded;
    private Vector3 evadeDirection;

    [Header("----Movement----")]
    public float maxSpeed;
    public float minSpeed;
    [SerializeField]
    private float currentSpeed;
    public float rotationSpeed;
    private Quaternion targetRotation;

    [Header("----Seperation----")]
    public float separationRadius;
    public float separationStrength;

    private LayerMask enemyLayerMask;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnLocation = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Ensure the player is tagged as 'Player'");
        }
        SetRandomSpeed();
        currentState = State.RandomFlight;

        targetRotation = transform.rotation;

        // Set the enemy layer mask to only include the enemy layer
        enemyLayerMask = LayerMask.GetMask("Enemy");

        // Ensure the enemy GameObjects are on the "Enemy" layer
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.RandomFlight:
                HandleRandomFlight();
                break;
            case State.ChasePlayer:
                HandleChasePlayer();
                break;
            case State.Returning:
                HandleReturning();
                break;
            case State.EvadePlayer:
                HandleEvadePlayer();
                break;
        }
    }

    private void FixedUpdate()
    {
        // Limit linear velocity
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Check if the enemy is too far from the spawn point
        if (Vector3.Distance(transform.position, spawnLocation) > maxDistanceFromSpawn)
        {
            SwitchState(State.Returning);
        }

        ApplySeparation();

        RotateTowardsTarget();

        MoveForward();

    }

    private void MoveForward()
    {
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    private void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }
    private void SetRandomSpeed()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void ApplySeparation()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationRadius, enemyLayerMask);
        Vector3 separationForce = Vector3.zero;

        foreach (Collider enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject != this.gameObject)
            {
                Vector3 toOther = transform.position - enemyCollider.transform.position;
                float distance = toOther.magnitude;

                if (distance > 0)
                {
                    // Calculate repulsion strength inversely proportional to distance
                    float strength = separationStrength / distance;
                    separationForce += toOther.normalized * strength;
                }
            }
        }

        if (separationForce != Vector3.zero)
        {
            Quaternion separationRotation = Quaternion.LookRotation(separationForce.normalized);
            targetRotation = Quaternion.Slerp(targetRotation, separationRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }



    private void HandleReturning()
    {

        Vector3 direction = (spawnLocation - transform.position).normalized;

        targetRotation = Quaternion.LookRotation(direction);

        // Smooth rotation using angular velocity
        RotateTowards(direction);

        // Switch to random flight when close to the spawn point
        if (Vector3.Distance(transform.position, spawnLocation) < minDistanceFromSpawn)
        {
            SetRandomSpeed();
            SwitchState(State.RandomFlight);
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < detectionRadius)
        {
            SetRandomSpeed();
            SwitchState(State.ChasePlayer);
        }
    }

    private void HandleRandomFlight()
    {
        rb.linearVelocity = currentDirection * currentSpeed;

        // Timer for changing direction
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            SetRandomSpeed();
            currentDirection = GetRandomDirection();
            directionChangeTimer = directionChangeInterval;

            targetRotation = Quaternion.LookRotation(currentDirection);
        }

        // Smooth rotation towards movement direction
        RotateTowards(rb.linearVelocity);

        // Check for player detection
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                SetRandomSpeed();
                SwitchState(State.ChasePlayer);
            }
        }
    }

    private Vector3 GetRandomDirection()
    {
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Clamp(randomDir.y, -0.5f, 0.5f);
        return randomDir.normalized;
    }

    private void HandleChasePlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction);


        // Check if player is out of detection radius
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > detectionRadius * 3)
        {
            SetRandomSpeed();
            SwitchState(State.RandomFlight);
        }

        // Check if player is too close
        if (distanceToPlayer <= evadeThreshold && IsFacingPlayer())
        {
            SwitchState(State.EvadePlayer);
        }
    }

    private void HandleEvadePlayer()
    {
        if (playerTransform == null) return;

        if (!hasEvaded)
        {
            evadeDirection = GetEvadeDirection();
            hasEvaded = true;
        }

        targetRotation = Quaternion.LookRotation(evadeDirection);

        // Smooth rotation away from player
        RotateTowards(evadeDirection);

        // Switch back to ChasePlayer if far enough from the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > evadeDistance)
        {
            SetRandomSpeed();
            hasEvaded = false;
            SwitchState(State.ChasePlayer);
        }
    }

    private Vector3 GetEvadeDirection()
    {
        Vector3 toPlayer = (playerTransform.position - transform.position).normalized;

        Vector3 randomVector = Random.onUnitSphere;
        while (Mathf.Abs(Vector3.Dot(randomVector, toPlayer)) > 0.9f)
        {
            randomVector = Random.onUnitSphere;
        }

        Vector3 perpendicular = Vector3.Cross(toPlayer, randomVector).normalized;

        // Combine the vectors to get an evade direction in 3D space
        float randomFactor = Random.Range(0.4f, 1.0f);
        Vector3 evadeDir = (toPlayer + perpendicular * randomFactor).normalized;

        return evadeDir;
    }

    private bool IsFacingPlayer()
    {
        Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
        return angleToPlayer < facingAngle;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        // Calculate the target rotation based on the desired direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly interpolate towards the target rotation
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Apply the rotation using MoveRotation
        rb.MoveRotation(newRotation);
    }

    private void SwitchState(State newState)
    {
        currentState = newState;

        if (newState != State.EvadePlayer)
        {
            hasEvaded = false;
        }

        // Set appropriate target rotation when switching states
        switch (newState)
        {
            case State.RandomFlight:
                currentDirection = GetRandomDirection();
                targetRotation = Quaternion.LookRotation(currentDirection);
                break;
            case State.ChasePlayer:
                if (playerTransform != null)
                {
                    Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    targetRotation = Quaternion.LookRotation(directionToPlayer);
                }
                break;
            case State.Returning:
                Vector3 directionToSpawn = (spawnLocation - transform.position).normalized;
                targetRotation = Quaternion.LookRotation(directionToSpawn);
                break;
            case State.EvadePlayer:
                // Evade direction is set in HandleEvadePlayer
                break;
        }
    }

    // Optional: Visualize detection radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnLocation, maxDistanceFromSpawn);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, evadeThreshold);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with player, apply damage or other effects
            EnemyPool.Instance.ReturnEnemy(gameObject);
        }

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Handle collision with asteroid, apply damage or other effects
            EnemyPool.Instance.ReturnEnemy(gameObject);
        }
    }
}
