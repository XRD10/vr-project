using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine;
using TMPro;

public class CRTScreenEffect : MonoBehaviour
{
    private Material material;
    private TextMeshProUGUI text;
    
    [SerializeField] private float flickerIntensity = 0.03f;
    [SerializeField] private float scanlineSpeed = 10f;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        material = text.fontMaterial;
    }
    
    void Update()
    {
        // Update material properties
        material.SetFloat("_ScreenFlicker", flickerIntensity * Mathf.PerlinNoise(Time.time * 10f, 0));
        material.SetFloat("_ScanlineSpeed", scanlineSpeed);
    }
}