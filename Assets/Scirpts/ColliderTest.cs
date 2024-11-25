using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionNotifier : MonoBehaviour
{
    private void ColliderTest(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Player is in contact with the enemy: " + hit.collider.gameObject.name);
        }
    }
}
