using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;
    public Renderer enemyRenderer;
    public Color damageColor = Color.red;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public ParticleSystem deathEffectPrefab;

    public GameObject potionPrefab; // Assign the potion prefab in the Inspector
    public GameObject bombPrefab;   // Assign the bomb prefab in the Inspector
    public float potionDropChance = 0.5f; // 50% chance to drop a potion
    public float bombDropChance = 0.3f;   // 30% chance to drop a bomb

    private Color originalColor;
    private Animator animator;
    private Rigidbody rb;
    private bool isDead = false;
    private bool isKnockedBack = false;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    public void TakeDamage(int amount, Vector3 knockbackDirection)
    {
        if (isDead) return;

        health -= amount;

        StartCoroutine(HandleDamageEffects(knockbackDirection));

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Trigger the death animation
        animator.SetTrigger("Die");

        // Attempt to drop a collectible
        TryDropItem();

        // Destroy the object after the death animation
        float deathAnimationDuration = 1.2f;
        Destroy(gameObject, deathAnimationDuration);
    }

    private IEnumerator HandleDamageEffects(Vector3 knockbackDirection)
    {
        if (isKnockedBack) yield break;
        isKnockedBack = true;

        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = damageColor;
        }

        if (rb != null)
        {
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(knockbackDuration);

        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }

        isKnockedBack = false;
    }

    public void PlayDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            // Instantiate the particle system prefab
            ParticleSystem effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

            // Play the particle system
            effect.Play();
        }
    }

    private void TryDropItem()
    {
        float randomValue = Random.value; // Random value between 0 and 1

        if (randomValue <= potionDropChance)
        {
            // Spawn potion
            Instantiate(potionPrefab, transform.position, Quaternion.identity);
        }
        else if (randomValue <= potionDropChance + bombDropChance)
        {
            // Spawn bomb
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
        // No need for an else block; nothing spawns if randomValue > (potionDropChance + bombDropChance)
    }
}
