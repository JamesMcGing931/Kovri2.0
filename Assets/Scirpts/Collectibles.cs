using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Goblet }
    public CollectibleType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == CollectibleType.Goblet)
            {
                PlayerCollectibles playerCollectibles = other.GetComponent<PlayerCollectibles>();
                if (playerCollectibles != null)
                {
                    playerCollectibles.CollectGoblet();
                }
            }

            Destroy(gameObject);
        }
    }
}
