using Unity.XR.CoreUtils;
using UnityEngine;

public class VRFlowManager : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin; // XR Origin (NOT camera)
    public Transform cameraOffset; // Camera Offset (fallback if no XROrigin)
    public Transform spawnPoint;

    [Tooltip("Where the XR camera moves for the qualitative / copper test station (e.g. child under QualitativeAnysis).")]
    public Transform qualitativeSpawnPoint;

    [Tooltip("If true, XR Origin yaw matches spawn point Y rotation after teleport.")]
    public bool alignYawToSpawn = true;

    public GameObject menuCanvas;

    public TitrationExperimentManager experimentManager;

    public void StartTitrationExperiment()
    {
        TeleportToSpawn(spawnPoint);

        if (menuCanvas != null)
            menuCanvas.SetActive(false);
    }

    public void StartQualitativeAnalysisExperiment()
    {
        TeleportToSpawn(qualitativeSpawnPoint);

        if (menuCanvas != null)
            menuCanvas.SetActive(false);
    }

    void TeleportToSpawn(Transform target)
    {
        if (target == null || xrOrigin == null)
        {
            Debug.LogWarning("VRFlowManager: spawn transform or xrOrigin is not assigned.", this);
            return;
        }

        if (alignYawToSpawn)
        {
            var e = xrOrigin.eulerAngles;
            xrOrigin.rotation = Quaternion.Euler(e.x, target.eulerAngles.y, e.z);
        }

        var xrOriginComponent = xrOrigin.GetComponent<XROrigin>();
        if (xrOriginComponent != null)
        {
            xrOriginComponent.MoveCameraToWorldLocation(target.position);
            return;
        }

        if (cameraOffset == null)
        {
            Debug.LogWarning("VRFlowManager: cameraOffset is not assigned and no XROrigin found.", this);
            return;
        }

        Vector3 offset = xrOrigin.position - cameraOffset.position;
        xrOrigin.position = target.position + offset;
    }
}