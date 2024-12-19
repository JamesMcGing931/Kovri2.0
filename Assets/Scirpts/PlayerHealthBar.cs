using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    public Image healthBarFill;

    private void Update()
    {
        if (playerHealth != null && healthBarFill != null)
        {
            float healthPercentage = (float)playerHealth.health / 100f;


            healthBarFill.fillAmount = healthPercentage;
        }
    }
}
