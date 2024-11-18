using UnityEngine;
using System.Collections;

public class EnemyFireController : MonoBehaviour
{
    [Header("----Projectile settings----")]
    public GameObject projectilePrefab;
    public float proj_Speed;
    public int proj_Damage;
    public Transform[] firepoints;

    [Header("----Randomisation----")]
    float inaccuracy = 2f;

    [Header("----Burst Fire settings----")]
    public int shotsPerBurst;
    public float timeBetweenShots;
    public float timeBetweenBursts;

    private int currentLaserIndex;

    private bool isFiring;
    private Coroutine firingCoroutine;

    public void StartFiring()
    {
        if (!isFiring)
        {
            isFiring = true;
            firingCoroutine = StartCoroutine(BurstFire());
        }
    }

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found. Ensure the player is tagged 'Player'.");
        }

        isFiring= false;
    }

    public void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
            }
        }
    }

    private IEnumerator BurstFire()
    {
        while (isFiring)
        {
            for (int i = 0; i < shotsPerBurst; i++)
            {
                FireSingleShot(firepoints[currentLaserIndex]);
                currentLaserIndex = (currentLaserIndex + 1) % firepoints.Length;

                if (i < shotsPerBurst - 1)
                {
                    yield return new WaitForSeconds(timeBetweenShots);
                }
            }

            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    private Vector3 CalculateInaccuracy()
    {

        // Get direction to player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Apply inaccuracy
        float inaccuracyAngle = inaccuracy;
        directionToPlayer = Quaternion.Euler(
            Random.Range(-inaccuracyAngle, inaccuracyAngle),
            Random.Range(-inaccuracyAngle, inaccuracyAngle),
            Random.Range(-inaccuracyAngle, inaccuracyAngle)
        ) * directionToPlayer;

        // Calculate target position far along the inaccurate direction
        float targetDistance = 1000f; // Arbitrary large distance
        Vector3 targetPosition = transform.position + directionToPlayer * targetDistance;

        return targetPosition;
    }

    private void FireSingleShot(Transform firePoint)
    {
        GameObject laser = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        EnemyProjectile projectile = laser.GetComponent<EnemyProjectile>();
        projectile.speed = proj_Speed;
        projectile.damage = proj_Damage;
        projectile.targetPosition = CalculateInaccuracy();
    }
}
