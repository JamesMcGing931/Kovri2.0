using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroZone : MonoBehaviour
{
    public GameObject target; 
    public GameObject enemy; 
    public float movementSpeed = 3f; 
    public float rotationSpeed = 5f;
    public float attackRange = 1.5f; 
    public float attackCooldown = 2f; 

    private Rigidbody enemyRigidBody;
    private Animator animator;
    private WaypointPatternMovement waypointMovement;
    private Vector3 calculatedDirection;
    private Vector3 calculatedDistance;
    private bool targetDetected = false;
    private bool isAttacking = false;

    void Start()
    {
        enemyRigidBody = enemy.GetComponent<Rigidbody>();
        animator = enemy.GetComponentInParent<Animator>();
        waypointMovement = enemy.GetComponent<WaypointPatternMovement>();
    }

    private void FixedUpdate()
    {
        if (targetDetected)
        {
            if (waypointMovement != null)
            {
                waypointMovement.enabled = false;
            }

            // Calculate direction to the player
            calculatedDirection = (target.transform.position - enemy.transform.position).normalized;

            // Calculate distance
            calculatedDistance = target.transform.position - enemy.transform.position;

            if (calculatedDistance.magnitude <= attackRange)
            {
                // Stop moving and attack if in range
                enemyRigidBody.velocity = Vector3.zero;

                if (!isAttacking)
                {
                    StartCoroutine(PerformAttack());
                }
            }
            else
            {
                if (calculatedDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(calculatedDirection);
                    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

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
                    enemyRigidBody.velocity = Vector3.zero; 
                }
            }
        }
        else
        {
            if (waypointMovement != null)
            {
                waypointMovement.enabled = true;
            }

            enemyRigidBody.velocity = Vector3.zero;
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
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
