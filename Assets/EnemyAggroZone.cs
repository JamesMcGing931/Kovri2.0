using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroZone : MonoBehaviour
{
    public GameObject enemy;
    public float movementSpeed;
    public float rotationSpeed = 5f;  // Rotation speed to control how fast the enemy turns

    private Rigidbody enemyRigidBody;
    private Vector3 calculatedDirection;
    private Vector3 calculatedDistance;
    private bool targetDetected = false;

    private WaypointPatternMovement waypointMovement; // Reference to the waypoint movement script

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = enemy.GetComponent<Rigidbody>();
        waypointMovement = enemy.GetComponent<WaypointPatternMovement>(); // Get the waypoint script component
    }

    private void FixedUpdate()
    {
        if (targetDetected)
        {
            // Locate the player dynamically by tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Player object not found in the scene with tag 'Player'.");
                return;
            }

            // Disable waypoint movement while chasing the player
            if (waypointMovement != null)
            {
                waypointMovement.enabled = false;
            }

            // Calculate the direction to the player
            calculatedDirection = (player.transform.position - enemy.transform.position).normalized;

            // Calculate the distance from the player
            calculatedDistance = player.transform.position - enemy.transform.position;

            // Rotate the enemy to face the player
            if (calculatedDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(calculatedDirection);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Move towards the player if the distance is greater than 2 units
            if (calculatedDistance.magnitude >= 2)
            {
                enemyRigidBody.velocity = new Vector3(
                    calculatedDirection.x * movementSpeed,
                    enemyRigidBody.velocity.y, // Keep Y velocity for gravity
                    calculatedDirection.z * movementSpeed
                );
            }
            else
            {
                // Stop moving if too close to the player
                enemyRigidBody.velocity = Vector3.zero;
            }
        }
        else
        {
            // Re-enable waypoint movement when player is out of range
            if (waypointMovement != null)
            {
                waypointMovement.enabled = true;
            }

            // Stop moving when target is not detected
            enemyRigidBody.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect player by tag
        if (other.CompareTag("Player"))
        {
            targetDetected = true;
            Debug.Log("Player detected by aggro zone!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetDetected = false;
            Debug.Log("Player left aggro zone!");
        }
    }
}
