using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    private readonly float _topSpeed = 50.0f;
    private readonly float _minSpeed = 10.0f;
    private float speedChange = 0.0f;
    private float _currentSpeed;

    void Start()
    {
        _currentSpeed = _minSpeed;
    }

    void Update()
    {
        _currentSpeed += speedChange * Time.deltaTime;

        // Dont go below or above the speed limits
        if (_currentSpeed > _topSpeed)
        {
            _currentSpeed = _topSpeed;
        }
        else if (_currentSpeed < _minSpeed)
        {
            _currentSpeed = _minSpeed;
        }

        Debug.Log("Current speed: " + _currentSpeed);

        // Adjust the position of the object based on the speed
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }

    public void OnThrustChange(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());

        if (context.ReadValue<float>() > 0)
        {
            speedChange = 10.0f;
        }
        else if (context.ReadValue<float>() < 0)
        {
            speedChange = -10.0f;
        }
        else
        {
            speedChange = 0.0f;
        }
    }
}
