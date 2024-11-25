using System.Collections;
using UnityEngine;

public class CrystalBall : MonoBehaviour
{
    public GameObject door; // Assign the door GameObject in the Inspector
    private Renderer crystalRenderer; // Reference to the CrystalBall's renderer
    private Animator doorAnimator; // Reference to the Door's Animator

    private Color originalColor; // Store the original color of the CrystalBall
    private Color highlightColor = Color.yellow; // The color to change when activated

    private void Start()
    {
        // Get the Renderer of the CrystalBall
        crystalRenderer = GetComponent<Renderer>();
        if (crystalRenderer != null && crystalRenderer.material.HasProperty("_Color"))
        {
            // Store the original color
            originalColor = crystalRenderer.material.color;
        }

        // Get the Animator from the door
        if (door != null)
        {
            doorAnimator = door.GetComponent<Animator>();
            if (doorAnimator == null)
            {
                Debug.LogError("No Animator found on the Door!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player interacted with the CrystalBall
        if (other.CompareTag("Player"))
        {
            Debug.Log("CrystalBall activated!");

            // Change the CrystalBall's color to yellow
            if (crystalRenderer != null && crystalRenderer.material.HasProperty("_Color"))
            {
                crystalRenderer.material.color = highlightColor;
            }

            // Trigger the door animation
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("DoorOpen");
            }
        }
    }
}
