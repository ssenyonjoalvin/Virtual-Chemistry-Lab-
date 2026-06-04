using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Attach this to a trigger collider at the tip of the tongs.
/// When the tongs are being held and magnesium touches this tip trigger,
/// Step 3 is completed.
/// </summary>
public class TongsTipProximity : MonoBehaviour
{
    [Tooltip("XRGrabInteractable on the tongs root object.")]
    [SerializeField] private XRGrabInteractable tongsGrabInteractable;

    [Tooltip("Combustion experiment manager in the scene.")]
    [SerializeField] private CombustionExperimentManager experimentManager;

    private void OnTriggerStay(Collider other)
    {
        if (tongsGrabInteractable == null || experimentManager == null)
            return;

        // Require the player to actually be holding the tongs.
        if (!tongsGrabInteractable.isSelected)
            return;

        CombustionMagnesiumRibbon magnesium = other.GetComponentInParent<CombustionMagnesiumRibbon>();
        if (magnesium == null)
            return;

        experimentManager.OnTongsHoldingMagnesium();
    }
}
