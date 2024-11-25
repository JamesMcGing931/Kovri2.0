using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player object to follow
    public Vector3 offset = new Vector3(0, 10, -10); // Camera offset from the player
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement

    private void LateUpdate()
    {
        if (player != null)
        {
            // Target position for the camera
            Vector3 desiredPosition = player.position + offset;

            // Smoothly interpolate between current position and the target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;
        }
    }
}
