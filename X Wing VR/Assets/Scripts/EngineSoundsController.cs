using UnityEngine;

public class EngineSoundController : MonoBehaviour
{
	[SerializeField] private AudioSource engineSound;

	[SerializeField] private float minPitch = 0.9f;
	[SerializeField] private float maxPitch = 1.2f;

	[SerializeField] private float minVolume = 0.4f;
	[SerializeField] private float maxVolume = 1.0f;

	private void Start()
	{
		// Get the AudioSource component if not assigned in inspector
		if (engineSound == null)
		{
			engineSound = GetComponent<AudioSource>();
		}

		if (engineSound == null)
		{
			Debug.LogWarning("No AudioSource found for EngineSoundController!");
		}
	}

	public void UpdateEngineSound(float currentSpeed, float minSpeed, float maxSpeed)
	{
		if (engineSound == null) return;

		float speedRatio = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);

		engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);

		engineSound.volume = Mathf.Lerp(minVolume, maxVolume, speedRatio);
	}

	public void BoostEngineSound(float pitchMultiplier = 1.2f, float volumeMultiplier = 1.2f)
	{
		if (engineSound == null) return;

		engineSound.pitch *= pitchMultiplier;
		engineSound.volume = Mathf.Clamp(engineSound.volume * volumeMultiplier, 0f, 1f);
	}

	public void ResetEngineSound()
	{
		if (engineSound == null) return;

		engineSound.pitch = minPitch;
		engineSound.volume = minVolume;
	}
}