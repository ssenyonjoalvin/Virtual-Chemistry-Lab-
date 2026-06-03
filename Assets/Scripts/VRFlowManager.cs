using UnityEngine;

public class VRFlowManager : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin; // XR Origin (NOT camera)
    public Transform cameraOffset; // Camera Offset
    public Transform spawnPoint;

      public Transform explosivesArea;

    public GameObject menuCanvas;

    public TitrationExperimentManager experimentManager;

    public void StartTitrationExperiment()
    {
        // 1. Teleport player
        TeleportToLab();

        // 2. Hide menu
        menuCanvas.SetActive(false);

    }

     public void StartGotoExplosives()
    {
        // 1. Teleport player to the explosives area
        TeleportToExplosives();

        // 2. Hide menu after teleporting
        menuCanvas.SetActive(false);

    }

    void TeleportToLab()
    {
        // Move XR Origin so camera ends up at spawn point
        Vector3 offset = xrOrigin.position - cameraOffset.position;
        xrOrigin.position = spawnPoint.position + offset;
    }

  void TeleportToExplosives()
    {
        // Move XR Origin so camera ends up at spawn point
        Vector3 offset = xrOrigin.position - cameraOffset.position;
        xrOrigin.position = explosivesArea.position + offset;
    }

}