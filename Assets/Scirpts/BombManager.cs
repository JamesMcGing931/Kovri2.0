using UnityEngine;
using TMPro;

public class BombManager : MonoBehaviour
{
    public int bombCount = 0; 
    public TMP_Text bombCountText; 
    public GameObject bombPrefab; 
    public float bombSpawnDistance = 2f; 

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
            bombCount--;
            UpdateUI();

            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        if (bombPrefab != null)
        {
            // Position in front of player
            Vector3 spawnPosition = transform.position + transform.forward * bombSpawnDistance;

            Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BombCollectible"))
        {
            AddBomb(1); 
            Destroy(other.gameObject); 
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
