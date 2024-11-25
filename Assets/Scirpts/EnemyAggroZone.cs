using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroZone : MonoBehaviour
{
    public GameObject target;
    public GameObject enemy;
    public float movementSpeed;
    public float rotationSpeed = 5f;

    private Rigidbody enemyRigidBody;
    private Vector3 calculatedDirection;
    private Vector3 calculatedDistance;
    private bool targetDetected = false;

    private WaypointPatternMovement waypointMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = enemy.GetComponent<Rigidbody>();
        waypointMovement = enemy.GetComponent<WaypointPatternMovement>();
    }

    private void FixedUpdate()
    {
        if (targetDetected)
        {
            // Disable waypoint movement while chasing the player
            if (waypointMovement != null)
            {
                waypointMovement.enabled = false;
            }

            // Calculate direction to the player
            calculatedDirection = (target.transform.position - enemy.transform.position).normalized;

            // Calculate distance
            calculatedDistance = target.transform.position - enemy.transform.position;

            // Rotate spider to face the player
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
                    enemyRigidBody.velocity.y,
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
        if (other.gameObject == target)
        {
            targetDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            targetDetected = false;
        }
    }
}
