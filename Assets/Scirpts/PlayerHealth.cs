using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public int attack = 10;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public Renderer playerRenderer;
    public Color damageColor = Color.red;

    public Animator animator; // Reference to the Animator component
    private Color originalColor;
    private bool isInvulnerable = false;
    private bool isDead = false; // Prevent input when dead

    private void Start()
    {
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable || isDead) return;

        health -= amount;
        Debug.Log($"Player took damage. Current Health: {health}");

        if (health <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            PlayGetHitAnimation();
            StartCoroutine(HandleDamageEffects());
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player has died.");

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable player inputs
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.DisableInputs();
        }
    }


    private void PlayGetHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
        }
        else
        {
            Debug.LogWarning("Animator not assigned!");
        }
    }

    private IEnumerator HandleDamageEffects()
    {
        isInvulnerable = true;

        // Knockback logic
        Vector3 knockbackDirection = -transform.forward;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            transform.position += knockbackDirection * knockbackForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Flashing logic
        if (playerRenderer != null)
        {
            playerRenderer.material.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            playerRenderer.material.color = originalColor;
        }

        yield return new WaitForSeconds(0.8f);
        isInvulnerable = false;
    }
}
