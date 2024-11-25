using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedDisplayController : MonoBehaviour
{
	[SerializeField] private VRFlightController flightController;
	[SerializeField] private TextMeshProUGUI speedDisplay;
	[SerializeField] private Image speedBar;

	private readonly float maxDisplaySpeed = 110f; // Max displayed speed for X-wing
	private float currentSpeed;
	private float minSpeed;
	private float topSpeed;

	void Start()
	{
		if (speedDisplay == null)
		{
			speedDisplay = GameObject.FindWithTag("SpeedDisplay").GetComponent<TextMeshProUGUI>();
		}

		minSpeed = flightController.GetMinSpeed();
		topSpeed = flightController.GetTopSpeed();
	}

	void Update()
	{
		if (flightController != null && speedDisplay != null)
		{
			currentSpeed = flightController.GetCurrentSpeed();

			// Calculate displayed speed (mapped to 110 max)
			float displayedSpeed = (currentSpeed - minSpeed) * (maxDisplaySpeed - minSpeed) / (topSpeed - minSpeed) + minSpeed;
			speedDisplay.text = $"{displayedSpeed:F1}";

			if (speedBar != null)
			{
				float fillAmount = Mathf.InverseLerp(minSpeed, topSpeed, currentSpeed);
				speedBar.fillAmount = fillAmount;
			}
		}
	}
}