using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroZone : MonoBehaviour
{
    public GameObject target; // Player or object to chase
    public GameObject enemy; // Enemy GameObject
    public float movementSpeed = 5f; // Speed of enemy movement

    private Rigidbody enemyRigidBody; // 3D Rigidbody for the enemy
    private Vector3 calculatedDirection; // Direction toward the target
    private Vector3 calculatedDistance; // Distance from the target
    private bool targetDetected = false; // Whether the target is in range

    private Animator enemyAnimator; // Animator for enemy animations

    void Start()
    {
        // Get the Rigidbody and Animator components
        enemyRigidBody = enemy.GetComponentInChildren<Rigidbody>();
        if (enemyRigidBody == null)
        {
            Debug.LogError("Enemy does not have a Rigidbody component.");
        }

        enemyAnimator = enemy.GetComponent<Animator>();
        if (enemyAnimator == null)
        {
            Debug.LogError("Enemy does not have an Animator component.");
        }
    }

    void Update()
    {
        if (targetDetected)
        {
            // Play walking animation
            enemyAnimator.SetBool("IsWalking", true);
        }
        else
        {
            // Stop walking animation
            enemyAnimator.SetBool("IsWalking", false);
        }
    }

    private void FixedUpdate()
    {
        if (targetDetected)
        {
            // Calculate the direction toward the target
            calculatedDirection = (target.transform.position - enemy.transform.position).normalized;

            // Calculate the distance from the target
            calculatedDistance = target.transform.position - enemy.transform.position;

            // Check if the enemy is far enough from the target
            if (calculatedDistance.magnitude >= 2f)
            {
                // Move the enemy toward the target
                enemyRigidBody.velocity = new Vector3(
                    calculatedDirection.x * movementSpeed,
                    enemyRigidBody.velocity.y, // Maintain gravity
                    calculatedDirection.z * movementSpeed
                );
            }
            else
            {
                // Stop moving when close enough
                enemyRigidBody.velocity = new Vector3(0, enemyRigidBody.velocity.y, 0);
            }
        }
        else
        {
            // Stop the enemy's movement if the target is not detected
            enemyRigidBody.velocity = new Vector3(0, enemyRigidBody.velocity.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the detected object is the target
        if (other.gameObject == target)
        {
            targetDetected = true;
            Debug.Log("Target detected!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the target
        if (other.gameObject == target)
        {
            targetDetected = false;
            Debug.Log("Target lost!");
        }
    }
}
