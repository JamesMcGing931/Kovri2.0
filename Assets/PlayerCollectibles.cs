using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerCollectibles : MonoBehaviour
{
    public int gobletCount = 0; // Number of goblets collected
    public TMP_Text gobletText; // Use TMP_Text for TextMeshPro components

    private void Start()
    {
        UpdateUI();
    }

    public void CollectGoblet()
    {
        gobletCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gobletText != null)
        {
            gobletText.text = $"Goblets: {gobletCount}";
        }
    }
}
