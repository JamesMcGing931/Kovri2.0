using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to PlayerHealth script
    public Image healthBarFill; // Reference to the HealthBarFill UI image

    private void Update()
    {
        if (playerHealth != null && healthBarFill != null)
        {
            // Calculate the health percentage
            float healthPercentage = (float)playerHealth.health / 100f;

            // Debug the calculated health percentage
            Debug.Log($"Updating Health Bar: Player Health = {playerHealth.health}, Health Percentage = {healthPercentage}");

            // Update the fill amount
            healthBarFill.fillAmount = healthPercentage;
        }
        else
        {
            Debug.LogWarning("PlayerHealth or HealthBarFill is not assigned.");
        }
    }
}
