using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private ParticleSystem explosionParticles;

    private void Start()
    {
        explosionParticles = GetComponent<ParticleSystem>();
        explosionParticles.Play(); 
        Destroy(gameObject, explosionParticles.main.duration + explosionParticles.main.startLifetime.constantMax);
    }
}
