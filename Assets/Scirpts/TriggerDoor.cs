using System.Collections;
using UnityEngine;

public class CrystalBall : MonoBehaviour
{
    public GameObject door;
    public AudioClip colourChangeSound;

    private Renderer crystalRenderer; 
    private Animator doorAnimator;
    private AudioSource audioSource;

    private Color originalColor; 
    private Color highlightColor = Color.yellow; 

    private void Start()
    {
        crystalRenderer = GetComponent<Renderer>();
        if (crystalRenderer != null && crystalRenderer.material.HasProperty("_Color"))
        {
            originalColor = crystalRenderer.material.color;
        }

        if (door != null)
        {
            doorAnimator = door.GetComponent<Animator>();
            if (doorAnimator == null)
            {
            }
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = colourChangeSound;
        audioSource.volume = 0.5f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (crystalRenderer != null && crystalRenderer.material.HasProperty("_Color"))
            {
                crystalRenderer.material.color = highlightColor;
            }

            if (colourChangeSound != null)
            {
                audioSource.Play();
            }

            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("DoorOpen");
            }
        }
    }
}
