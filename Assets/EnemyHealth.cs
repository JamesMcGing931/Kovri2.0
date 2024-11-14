using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;
    private Animator animator; // Reference to the Animator

    private void Start()
    {
        animator = GetComponentInParent<Animator>(); // Assumes Animator is attached to the same GameObject
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        // Trigger the death animation
        animator.SetTrigger("Die");

        // Optional: Disable further interactions, like movement or attacks
        float deathAnimationDuration = 2f; // Set this to match the length of your death animation
        Destroy(gameObject, deathAnimationDuration);
    }
}
