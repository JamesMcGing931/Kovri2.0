using UnityEngine;
using TMPro;

public class BombManager : MonoBehaviour
{
    public int bombCount = 0; // Number of bombs the player has
    public TMP_Text bombCountText; // Assign this in the Inspector to the bomb UI text
    public GameObject bombPrefab; // Assign the bomb prefab in the Inspector
    public float bombSpawnDistance = 2f; // Distance to spawn the bomb in front of the player

    private void Start()
    {
        UpdateUI();
    }

    public void AddBomb(int amount)
    {
        bombCount += amount;
        UpdateUI();
    }

    public void UseBomb()
    {
        if (bombCount > 0)
        {
            // Decrement bomb count and update UI
            bombCount--;
            UpdateUI();

            // Spawn the bomb
            PlaceBomb();
        }
        else
        {
            Debug.Log("No bombs left to place!");
        }
    }

    private void PlaceBomb()
    {
        if (bombPrefab != null)
        {
            // Calculate the spawn position in front of the player
            Vector3 spawnPosition = transform.position + transform.forward * bombSpawnDistance;

            // Instantiate the bomb at the calculated position
            Instantiate(bombPrefab, spawnPosition, Quaternion.identity);

            Debug.Log("Bomb placed at: " + spawnPosition);
        }
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
