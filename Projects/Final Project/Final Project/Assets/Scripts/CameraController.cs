using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTarget;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;


    public float minX = -10f;
    public float maxX = 38f;
    public float minY = -10f;
    public float maxY = 14f;

    void Start()
    {
    
    }

    void LateUpdate()
    {
        if (playerTarget == null)
        {
            Debug.LogError("Player Target not set for CameraController!");
            return;
        }


        Vector3 desiredPosition = playerTarget.position + offset;


        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}