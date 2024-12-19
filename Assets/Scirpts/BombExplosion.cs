using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab; 
    public float explosionDelay = 1f; 
    public float explosionRadius = 5f; 
    public int explosionDamage = 50;
    public AudioClip explosionSound;

    private bool hasExploded = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = explosionSound;

        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        if (hasExploded) return; 
        hasExploded = true;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        if (explosionSound != null)
        {
            audioSource.Play();
            audioSource.volume = 0.1f; 
        }

        DamageNearbyObjects();

        Destroy(gameObject, explosionSound != null ? explosionSound.length : 0.1f);
    }

    private void DamageNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
                continue; 
            }

            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Vector3 knockbackDirection = hitCollider.transform.position - transform.position;
                knockbackDirection.y = 0; 

                enemyHealth.TakeDamage(explosionDamage, knockbackDirection);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
