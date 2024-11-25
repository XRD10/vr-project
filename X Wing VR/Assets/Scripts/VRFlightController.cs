using UnityEngine;
using UnityEngine.XR.Content.Interaction;

//This controller is used to control the flight in the VR environment (by moving the joystick and thrust controllers)
public class VRFlightController : MonoBehaviour
{
	[Tooltip("Number by which the pitch is multiplied")]
	[SerializeField] private int pitchDetailer = 1;
	[Tooltip("Number by which the roll is multiplied")]
	[SerializeField] private int rollDetailer = 1;
	[SerializeField] private float topSpeed = 50.0f;
	[SerializeField] private float minSpeed = 10.0f;

	[SerializeField]
	private AudioController audioController;

	private float _speedChange = 0.0f;
	private float _currentSpeed;
	private float _desiredSpeed;

	private float _joystickX;
	private float _joystickY;

	void Start()
	{
		_currentSpeed = minSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		//Speed change
		if (_desiredSpeed > _currentSpeed)
		{
			_currentSpeed += _speedChange * Time.deltaTime;
		}
		else if (_desiredSpeed < _currentSpeed)
		{
			_currentSpeed -= _speedChange * Time.deltaTime;
		}

		// Dont go below or above the speed limits
		if (_currentSpeed > topSpeed)
		{
			_currentSpeed = topSpeed;
		}
		else if (_currentSpeed < minSpeed)
		{
			_currentSpeed = minSpeed;
		}

		// Rotation change
		float pitch = _joystickY * _currentSpeed * Time.deltaTime * pitchDetailer;
		float roll = _joystickX * _currentSpeed * Time.deltaTime * rollDetailer;

		transform.Rotate(pitch, 0, -roll);
		transform.position += _currentSpeed * Time.deltaTime * transform.forward;

		if (audioController != null)
		{
			audioController.UpdateEngineSound(_currentSpeed, minSpeed, topSpeed);
		}
	}

	public void OnThrustChange(float value)
	{
		_desiredSpeed = Mathf.Lerp(minSpeed, topSpeed, value);
		_speedChange = Mathf.Abs(_desiredSpeed - _currentSpeed) * 0.5f;
	}

	public void OnJoystickXChange(float value)
	{
		_joystickX = value;
	}

	public void OnJoystickYChange(float value)
	{
		_joystickY = value;
	}

	public float GetCurrentSpeed() => _currentSpeed;
	public float GetMinSpeed() => minSpeed;
	public float GetTopSpeed() => topSpeed;
}
