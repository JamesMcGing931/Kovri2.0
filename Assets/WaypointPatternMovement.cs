using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPatternMovement : MonoBehaviour
{
    public GameObject[] pattern;
    private int patternIndex = 0;
    public float speed = 1;
    public float rotationSpeed = 5f;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim != null)
        {
            anim.SetBool("IsWalking", true);
        }

        GameObject waypoint = pattern[patternIndex];

        // Vector towards waypoint
        Vector3 rangeToClose = waypoint.transform.position - transform.position;

        float distance = rangeToClose.magnitude;
        float speedDelta = speed * Time.deltaTime;

        // Check if waypoint has been reached
        if (distance <= speedDelta)
        {
            patternIndex++;
            if (patternIndex >= pattern.Length)
            {
                patternIndex = 0;
            }

            waypoint = pattern[patternIndex];
            rangeToClose = waypoint.transform.position - transform.position;
        }

        Vector3 normalizedRangeToClose = rangeToClose.normalized;

        // Rotate to direction it is facing
        if (normalizedRangeToClose != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(normalizedRangeToClose);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 delta = speedDelta * normalizedRangeToClose;
        transform.Translate(delta, Space.World);
    }
}
