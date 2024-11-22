using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroZone : MonoBehaviour
{
    public GameObject target; 
    public GameObject enemy; 
    public float movementSpeed = 5f; 

    private Rigidbody enemyRigidBody; 
    private Vector3 calculatedDirection; 
    private Vector3 calculatedDistance; 
    private bool targetDetected = false; 

    private Animator enemyAnimator; 

    void Start()
    {
        enemyRigidBody = enemy.GetComponent<Rigidbody>();
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
            enemyAnimator.SetBool("IsWalking", true);
        }
        else
        {
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
        if (other.gameObject == target)
        {
            targetDetected = true;
            Debug.Log("Target detected!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            targetDetected = false;
            Debug.Log("Target lost!");
        }
    }
}
