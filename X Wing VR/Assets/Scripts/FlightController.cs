using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    private readonly float _topSpeed = 50.0f;
    private readonly float _minSpeed = 10.0f;
    private float speedChange = 0.0f;
    private float _currentSpeed;

    private Vector2 rotationInput;
    private float speedInput;
    private float rollInput;

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

        // Apply roration based on input (pitch and yaw)
        float pitch = rotationInput.y * _currentSpeed * Time.deltaTime;
        float yaw = rotationInput.x * _currentSpeed * Time.deltaTime;
        float roll = rollInput * _currentSpeed * Time.deltaTime;
        transform.Rotate(pitch, yaw, -roll);

        Debug.Log("Pitch: " + pitch + " Yaw: " + yaw + " Roll: " + roll);

        // Adjust the position of the object based on the speed
        transform.position += _currentSpeed * Time.deltaTime * transform.forward;
    }

    public void OnThrustAndRollChange(InputAction.CallbackContext context)
    {

        speedInput = context.ReadValue<Vector2>().y;
        rollInput = context.ReadValue<Vector2>().x * 4f;

        if (speedInput > 0)
        {
            speedChange = 10.0f;
        }
        else if (speedInput < 0)
        {
            speedChange = -10.0f;
        }
        else
        {
            speedChange = 0.0f;
        }
    }

    public void OnRotation(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>() * 2f;
    }
}
