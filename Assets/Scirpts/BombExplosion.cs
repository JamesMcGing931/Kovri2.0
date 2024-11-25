using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab; 
    public float explosionDelay = 1f; 
    public float explosionRadius = 5f; 
    public int explosionDamage = 50; 

    private bool hasExploded = false; 

    private void Start()
    {
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        if (hasExploded) return; 
        hasExploded = true;

        Debug.Log("Bomb exploded!");

        // Instantiate the explosion effect at the bomb's position and rotation
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

            // Get the ParticleSystem's total duration
            ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                float totalDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
                Destroy(explosion, totalDuration);
            }
            else
            {
                Destroy(explosion, 2f); 
            }
        }

        DamageNearbyObjects();

        Destroy(gameObject);
    }

    private void DamageNearbyObjects()
    {
        // Find all colliders within the explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
                continue; // Skip to the next collider
            }

            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(explosionDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draws explosion radius in the editor for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
