using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int attack = 10;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public Renderer playerRenderer;
    public Color damageColor = Color.red;

    public Animator animator; // Reference to the Animator component
    private Color originalColor;
    private bool isInvulnerable = false;
    private bool isDead = false; // Prevent input when dead

    [Header("Game Over UI")]
    public GameObject gameOverUI; // Assign the Game Over UI Canvas in the Inspector
    public CanvasGroup canvasGroup; // Reference to the Canvas Group
    public float gameOverDelay = 3f; // Delay before showing the Game Over screen
    public float fadeDuration = 2f; // Time it takes to fade in

    private void Start()
    {
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        // Ensure Canvas Group starts fully transparent
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a potion
        if (other.CompareTag("Potion"))
        {
            RestoreHealth(20); // Restore 20 health
            Destroy(other.gameObject); // Destroy the potion
        }
    }

    public void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth; // Clamp health to maximum value
        }

        Debug.Log($"Health restored. Current Health: {health}");
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

        // Show Game Over screen after a delay
        StartCoroutine(ShowGameOverScreen());
    }

    private IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (canvasGroup != null)
        {
            yield return StartCoroutine(FadeInUI());
        }
    }

    private IEnumerator FadeInUI()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Gradually increase alpha
            yield return null;
        }

        canvasGroup.alpha = 1; // Ensure it’s fully visible at the end
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

    // Restart the current scene
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
