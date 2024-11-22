using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FireController : MonoBehaviour
{
    [Header("----Laser Settings----")]
    public string laserTag = "PlayerLaser";
    public float fireRate;
    public float laserSpeed;
    public int damage;
    [SerializeField]
    GameObject laserPrefab;


    [Header("----Meta----")]
    public bool isFiring = false;
    private Coroutine firingCoroutine;

    [Header("----Firing points----")]
    [Tooltip("Adjust firing order here")]
    [SerializeField]
    Transform[] laserOrigins;
    private int currentLaserIndex = 0;

    [Header("----Target point----")]
    [Tooltip("Set where the lasers should aim")]
    [SerializeField]
    Transform targetPoint;



    private FlightControls flightControls;

    private void Awake()
    {
        flightControls = new FlightControls();
    }

    private void OnEnable()
    {
        flightControls.Flying.Enable();
        flightControls.Flying.ToggleMenu.Enable();
        flightControls.Flying.ToggleMenu.performed += ToggleMenu;
        flightControls.Flying.Shoot.performed += OnFirePerformed;
        flightControls.Flying.Shoot.canceled += OnFireCancelled;
    }

    private void OnDisable()
    {
        flightControls.Flying.Shoot.performed -= OnFirePerformed;
        flightControls.Flying.Shoot.canceled -= OnFireCancelled;
        flightControls.Flying.Disable();
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("TOGGLE YES NEW");
    }


    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (!isFiring)
        {
            isFiring = true;
            firingCoroutine = StartCoroutine(FireContinuously());
        }
    }

    private void OnFireCancelled(InputAction.CallbackContext context)
    {
        if (isFiring)
        {
            isFiring = false;
            if (firingCoroutine != null)
            {
                StopCoroutine(FireContinuously());
                firingCoroutine = null;
            }
        }
    }
    private IEnumerator FireContinuously()
    {
        // Calculate the interval between each laser shot
        // Total time for one full cycle (all lasers fired once)
        float cycleTime = laserOrigins.Length / fireRate;

        // Time between individual laser shots
        float timeBetweenShots = cycleTime / laserOrigins.Length;

        while (isFiring)
        {
            FireSingleLaser(laserOrigins[currentLaserIndex]);

            currentLaserIndex = (currentLaserIndex + 1) % laserOrigins.Length;

            // Wait for the specified time before firing the next laser
            float elapsedTime = 0f;
            while (elapsedTime < timeBetweenShots)
            {
                if (!isFiring)
                    yield break;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void FireSingleLaser(Transform origin)
    {
        Vector3 direction = (targetPoint.position - origin.position).normalized;

        GameObject laser = Instantiate(laserPrefab, origin.position, Quaternion.LookRotation(direction));
        Projectile projectile = laser.GetComponent<Projectile>();
        projectile.speed = laserSpeed;
        projectile.damage = damage; //To be added
        projectile.targetPosition = targetPoint.position;
    }

}
