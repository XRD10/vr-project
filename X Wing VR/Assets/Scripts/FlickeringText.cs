using UnityEngine;
using TMPro; // If using TextMeshPro
using System.Collections;

public class FlickeringText : MonoBehaviour
{
	[SerializeField] private float minFlickerDuration = 0.05f;
	[SerializeField] private float maxFlickerDuration = 0.2f;
	[SerializeField] private float minAlpha = 0.3f;
	private TextMeshProUGUI text; // Use Text if using Unity's default UI Text
	private Color originalColor;

	void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
		originalColor = text.color;
		StartCoroutine(FlickerText());
	}

	IEnumerator FlickerText()
	{
		while (true)
		{
			float flickerTime = Random.Range(minFlickerDuration, maxFlickerDuration);
			float randomAlpha = Random.Range(minAlpha, 1f);
			text.color = new Color(originalColor.r, originalColor.g, originalColor.b, randomAlpha);
			yield return new WaitForSeconds(flickerTime);
		}
	}
}