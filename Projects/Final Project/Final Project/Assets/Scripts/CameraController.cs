using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target & Offset")]
    public Transform playerTarget;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    [Header("Bounds")]
    public float minX = -10f;
    public float maxX = 38f;
    public float minY = -10f;
    public float maxY = 14f;

    void LateUpdate()
    {
        if (playerTarget == null)
        {
            Debug.LogError("Player Target not set for CameraController!");
            return;
        }

        // Desired position with offset
        Vector3 desiredPosition = playerTarget.position + offset;

        // Clamp camera within bounds
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        desiredPosition.z = transform.position.z; // Keep original z

        // Smoothly move camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
