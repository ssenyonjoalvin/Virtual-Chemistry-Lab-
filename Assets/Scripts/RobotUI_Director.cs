using UnityEngine;

public class RobotUI_Director : MonoBehaviour
{
    [Header("Robot Settings")]
    public Collider robotCollider; 
    public Transform robotAnchor; 
    public float gazeAngleThreshold = 20f;
    public float flyToRobotSpeed = 5f;

    // Internal references
    private Transform vrCamera;
    private FollowCameraSmooth followScript; 
    private bool isLookingAtRobot = false;

    void Start()
    {
        if (Camera.main != null) vrCamera = Camera.main.transform;
        
        // Grab the existing follow script attached to this Canvas
        followScript = GetComponent<FollowCameraSmooth>();
    }

    void LateUpdate()
    {
        if (vrCamera == null || robotCollider == null || robotAnchor == null) return;

        // 1. Check if the user is looking at the Robot
        Vector3 directionToRobot = (robotCollider.bounds.center - vrCamera.position).normalized;
        float angleToRobot = Vector3.Angle(vrCamera.forward, directionToRobot);
        
        isLookingAtRobot = (angleToRobot < gazeAngleThreshold);

        // 2. Control the logic based on where they look
        if (isLookingAtRobot)
        {
            // Turn OFF the normal camera follow script so it stops fighting us!
            if (followScript != null && followScript.enabled)
            {
                followScript.enabled = false;
            }

            // Smoothly fly to the Robot's face
            transform.position = Vector3.Lerp(transform.position, robotAnchor.position, Time.deltaTime * flyToRobotSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, robotAnchor.rotation, Time.deltaTime * flyToRobotSpeed);
        }
        else
        {
            // Turn the normal camera follow script back ON!
            if (followScript != null && !followScript.enabled)
            {
                followScript.enabled = true;
            }
            
            // We don't need to do any movement math here, because the moment 
            // we re-enable FollowCameraSmooth, it will take over and do its job!
        }
    }
}