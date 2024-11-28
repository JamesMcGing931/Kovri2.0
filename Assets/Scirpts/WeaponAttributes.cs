using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the player's health and attack power

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Calculate the knockback direction
                Vector3 knockbackDirection = other.transform.position - transform.position;
                knockbackDirection.y = 0; // Ensure knockback is only horizontal (optional)

                // Apply damage and knockback
                enemyHealth.TakeDamage(playerHealth.attack, knockbackDirection);
            }
        }
    }
}
