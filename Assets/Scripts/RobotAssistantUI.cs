using UnityEngine;

public class RobotAssistantUI : MonoBehaviour
{
    [Header("UI & Robot References")]
    [Tooltip("The main Instruction Canvas that will move around")]
    public Transform instructionPanel; 
    [Tooltip("The Collider on the physical Robot model")]
    public Collider robotCollider; 

    [Header("Positions (Anchors)")]
    [Tooltip("An Empty GameObject placed next to the Robot's head")]
    public Transform robotAnchor; 
    [Tooltip("An Empty GameObject placed where the UI should normally sit (e.g. on the wall/desk)")]
    public Transform defaultAnchor; 

    [Header("Gaze Settings")]
    [Tooltip("How wide the 'cone of vision' is. 15-20 degrees feels natural in VR.")]
    public float gazeAngleThreshold = 20f;
    [Tooltip("How fast the panel flies through the air")]
    public float flySpeed = 5f;

    private Transform vrCamera;
    private Transform currentTarget;

    void Start()
    {
        // Automatically find the player's headset
        if (Camera.main != null) vrCamera = Camera.main.transform;
        
        currentTarget = defaultAnchor;
    }

    void Update()
    {
        if (vrCamera == null || robotCollider == null || instructionPanel == null) return;

        // 1. Calculate the angle between where the user is looking, and where the robot is
        Vector3 directionToRobot = (robotCollider.bounds.center - vrCamera.position).normalized;
        float angleToRobot = Vector3.Angle(vrCamera.forward, directionToRobot);

        // 2. Decide where the UI should be based on the Gaze
        if (angleToRobot < gazeAngleThreshold)
        {
            // User is looking at the robot!
            currentTarget = robotAnchor;
        }
        else
        {
            // User looked away!
            currentTarget = defaultAnchor;
        }

        // 3. Smoothly fly the UI to the chosen target
        instructionPanel.position = Vector3.Lerp(instructionPanel.position, currentTarget.position, Time.deltaTime * flySpeed);
        
        // 4. Smoothly rotate the UI to match the anchor's rotation
        instructionPanel.rotation = Quaternion.Lerp(instructionPanel.rotation, currentTarget.rotation, Time.deltaTime * flySpeed);
    }
}