using UnityEngine;

public class FollowCameraSmooth : MonoBehaviour
{
    public Transform cameraTransform;

    public float distance = 1.5f;
    public float heightOffset = -0.2f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Target position in front of camera
        Vector3 targetPosition =
            cameraTransform.position +
            cameraTransform.forward * distance +
            Vector3.up * heightOffset;

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );

        // Always face camera
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }
}