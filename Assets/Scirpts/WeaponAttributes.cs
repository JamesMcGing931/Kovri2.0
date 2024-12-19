using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public PlayerHealth playerHealth; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Vector3 knockbackDirection = other.transform.position - transform.position;
                knockbackDirection.y = 0; 

                enemyHealth.TakeDamage(playerHealth.attack, knockbackDirection);
            }
        }
    }
}
