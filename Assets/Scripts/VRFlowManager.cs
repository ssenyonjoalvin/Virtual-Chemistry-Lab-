using UnityEngine;

public class VRFlowManager : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin; // XR Origin (NOT camera)
    public Transform cameraOffset; // Camera Offset
    public Transform spawnPoint;

    public GameObject menuCanvas;

    public TitrationExperimentManager experimentManager;

    public void StartTitrationExperiment()
    {
        // 1. Teleport player
        TeleportToLab();

        // 2. Hide menu
        menuCanvas.SetActive(false);

    }

    void TeleportToLab()
    {
        // Move XR Origin so camera ends up at spawn point
        Vector3 offset = xrOrigin.position - cameraOffset.position;
        xrOrigin.position = spawnPoint.position + offset;
    }
}