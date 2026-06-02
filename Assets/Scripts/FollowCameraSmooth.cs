using UnityEngine;

public class FollowCameraSmooth : MonoBehaviour
{
    public Transform cameraTransform;

    [Header("Positioning")]
    public float distance = 1.5f;
    public float heightOffset = -0.2f;
    public float smoothSpeed = 5f;
    public float rotationSmoothSpeed = 10f;

    [Header("Constraints")]
    [Tooltip("Minimum height from the ground (Y=0) or a specific plane.")]
    public float minHeight = 0.8f;
    [Tooltip("Maximum height the menu can nudge up to avoid obstacles.")]
    public float maxHeight = 2.5f;
    [Tooltip("Radius for obstacle detection.")]
    public float collisionRadius = 0.2f;
    public LayerMask obstacleLayers = -1;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;
            
        // Snap to initial position
        if (cameraTransform != null)
        {
            transform.position = CalculateTargetPosition();
            transform.rotation = CalculateTargetRotation();
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 targetPosition = CalculateTargetPosition();
        Quaternion targetRotation = CalculateTargetRotation();

        // 1. Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );

        // 2. Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmoothSpeed
        );
    }

    private Vector3 CalculateTargetPosition()
    {
        // Base target position in front of camera
        Vector3 baseTarget = cameraTransform.position + cameraTransform.forward * distance;
        baseTarget.y += heightOffset;

        // A. Prevent ground penetration
        if (baseTarget.y < minHeight) baseTarget.y = minHeight;

        // B. Robust Obstacle Avoidance:
        if (IsPositionBlocked(baseTarget, out RaycastHit hit))
        {
            float nudgeStep = 0.25f;
            float maxNudge = 0.75f;
            
            // Priority 1: Move UP (on top of tables/desks)
            if (TryFindClearPosition(baseTarget, Vector3.up, nudgeStep, maxNudge, out Vector3 upTarget))
            {
                if (upTarget.y <= maxHeight) return upTarget;
            }

            // Priority 2: Move RIGHT
            if (TryFindClearPosition(baseTarget, cameraTransform.right, nudgeStep, maxNudge, out Vector3 rightTarget))
            {
                return rightTarget;
            }

            // Priority 3: Move LEFT
            if (TryFindClearPosition(baseTarget, -cameraTransform.right, nudgeStep, maxNudge, out Vector3 leftTarget))
            {
                return leftTarget;
            }

            // Fallback: Pull closer to the camera (ensures it doesn't clip through)
            baseTarget = cameraTransform.position + (baseTarget - cameraTransform.position).normalized * (hit.distance - collisionRadius);
        }

        return baseTarget;
    }

    private bool TryFindClearPosition(Vector3 basePosition, Vector3 nudgeDirection, float step, float max, out Vector3 foundPosition)
    {
        foundPosition = basePosition;
        for (float nudge = step; nudge <= max; nudge += step)
        {
            Vector3 testPos = basePosition + nudgeDirection * nudge;
            if (!IsPositionBlocked(testPos, out _))
            {
                foundPosition = testPos;
                return true;
            }
        }
        return false;
    }

    private bool IsPositionBlocked(Vector3 target, out RaycastHit hit)
    {
        Vector3 origin = cameraTransform.position;
        Vector3 direction = target - origin;
        float dist = direction.magnitude;
        return Physics.SphereCast(origin, collisionRadius, direction.normalized, out hit, dist, obstacleLayers);
    }

    private Quaternion CalculateTargetRotation()
    {
        // Face the camera but keep it upright (usually preferred for menus)
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0; // Flatten the rotation so it stays vertical
        
        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            return Quaternion.LookRotation(-directionToCamera);
        }
        
        return transform.rotation;
    }
}
