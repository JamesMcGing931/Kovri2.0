using UnityEngine;
using TMPro;

public class BombManager : MonoBehaviour
{
    public int bombCount = 0; // Number of bombs the player has
    public TMP_Text bombCountText; // Assign this in the Inspector to the bomb UI text

    private void Start()
    {
        UpdateUI();
    }

    public void AddBomb(int amount)
    {
        bombCount += amount;
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with a bomb collectible
        if (other.CompareTag("BombCollectible"))
        {
            AddBomb(1); // Increment bomb count
            Destroy(other.gameObject); // Destroy the collectible
        }
    }

    private void UpdateUI()
    {
        if (bombCountText != null)
        {
            bombCountText.text = $"Bombs: {bombCount}";
        }
    }
}
