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

    public Animator animator; 
    private Color originalColor;
    private bool isInvulnerable = false;
    private bool isDead = false;

    [Header("Game Over UI")]
    public GameObject gameOverUI; 
    public CanvasGroup canvasGroup; 
    public float gameOverDelay = 3f;
    public float fadeDuration = 2f;

    private void Start()
    {
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            RestoreHealth(20); 
            Destroy(other.gameObject); 
        }
    }

    public void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth; 
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable || isDead) return;

        health -= amount;

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

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.DisableInputs();
        }

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

        canvasGroup.alpha = 1;
    }

    private void PlayGetHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("GetHit");
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

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
