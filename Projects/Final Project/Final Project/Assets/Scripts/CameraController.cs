using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTarget;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Temporarily removing bounds variables as we disable clamping
    // public float minX = -10f; 
    // public float maxX = 38f; 
    // public float minY = -10f; 
    // public float maxY = 14f;

    void Start()
    {
        // Debug check to confirm Start() is running
        Debug.Log("CameraController initialized and active.");
    }

    void LateUpdate()
    {
        if (playerTarget == null)
        {
            Debug.LogError("Player Target not set for CameraController!");
            return;
        }

        // 1. Calculate the desired position
        Vector3 desiredPosition = playerTarget.position + offset;

        // Debug check to see where the camera WANTS to go
        Debug.Log($"Desired Position: {desiredPosition}");

        // 2. Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. Apply the position directly (CLAMPING IS TEMPORARILY DISABLED)
        transform.position = smoothedPosition;
    }
}