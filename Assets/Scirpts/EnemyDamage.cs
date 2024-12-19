using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 10; 

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Enemy") && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
