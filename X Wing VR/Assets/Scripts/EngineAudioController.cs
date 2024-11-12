using UnityEngine;

public class EngineAudioController : MonoBehaviour
{
	private float minPitch = 0.9f;
	private float maxPitch = 1.2f;

	private float minVolume = 0.4f;
	private float maxVolume = 1.0f;


	public void UpdateEngineSound(AudioSource engineSound, float currentSpeed, float minSpeed, float maxSpeed)
	{
		if (engineSound == null) return;

		float speedRatio = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);

		engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);

		engineSound.volume = Mathf.Lerp(minVolume, maxVolume, speedRatio);
	}
}