using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgroZone : MonoBehaviour
{
    public GameObject target; // Player target
    public GameObject enemy; // Enemy itself
    public float movementSpeed = 3f; // Speed of movement
    public float rotationSpeed = 5f; // Rotation speed
    public float attackRange = 1.5f; // Distance within which the enemy can attack
    public float attackCooldown = 2f; // Cooldown between attacks

    private Rigidbody enemyRigidBody;
    private Animator animator;
    private WaypointPatternMovement waypointMovement;
    private Vector3 calculatedDirection;
    private Vector3 calculatedDistance;
    private bool targetDetected = false;
    private bool isAttacking = false;

    // Start is called before the first frame update
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
            // Disable waypoint movement while chasing the player
            if (waypointMovement != null)
            {
                waypointMovement.enabled = false;
            }

            // Calculate direction to the player
            calculatedDirection = (target.transform.position - enemy.transform.position).normalized;

            // Calculate distance to the player
            calculatedDistance = target.transform.position - enemy.transform.position;

            if (calculatedDistance.magnitude <= attackRange)
            {
                // Stop moving and attack if within range
                enemyRigidBody.velocity = Vector3.zero;

                if (!isAttacking)
                {
                    StartCoroutine(PerformAttack());
                }
            }
            else
            {
                // Rotate to face the player
                if (calculatedDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(calculatedDirection);
                    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                // Move towards the player if out of attack range
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
                    enemyRigidBody.velocity = Vector3.zero; // Stop moving if too close but not in attack range
                }
            }
        }
        else
        {
            // Re-enable waypoint movement when the player is out of range
            if (waypointMovement != null)
            {
                waypointMovement.enabled = true;
            }

            // Stop moving when target is not detected
            enemyRigidBody.velocity = Vector3.zero;
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Trigger the attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Wait for the attack cooldown
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
