using UnityEngine;
using TMPro; 

public class PlayerCollectibles : MonoBehaviour
{
    public int gobletCount = 0; 
    public TMP_Text gobletText; 

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
