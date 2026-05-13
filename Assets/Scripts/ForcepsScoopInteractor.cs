using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[DisallowMultipleComponent]
[RequireComponent(typeof(XRGrabInteractable))]
public class ForcepsScoopInteractor : MonoBehaviour
{
    [Header("Tip Trigger (Optional)")]
    [Tooltip("If assigned, collisions are evaluated only when this transform touches.")]
    public Transform scoopTip;

    [Header("Held Sample Visual (Optional)")]
    public GameObject heldSampleVisual;

    [Header("State (Read Only)")]
    [SerializeField] private bool hasSample = false;
    [SerializeField] private string heldSubstanceId = "";

    private XRGrabInteractable grabInteractable;
    private Material heldSampleMaterial;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        SetHeldVisual(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBeingHeld()) return;

        if (!hasSample)
        {
            ScoopableSubstance source = other.GetComponentInParent<ScoopableSubstance>();
            if (source != null && source.TryConsumeOne())
            {
                hasSample = true;
                heldSubstanceId = source.substanceId;
                heldSampleMaterial = source.sampleMaterial;
                SetHeldVisual(true);
            }
            return;
        }

        TestTubeReceiver receiver = other.GetComponentInParent<TestTubeReceiver>();
        if (receiver != null)
        {
            bool accepted = receiver.TryReceive(heldSubstanceId, heldSampleMaterial);
            if (accepted)
            {
                hasSample = false;
                heldSubstanceId = "";
                heldSampleMaterial = null;
                SetHeldVisual(false);
            }
        }
    }

    private bool IsBeingHeld()
    {
        return grabInteractable != null && grabInteractable.isSelected;
    }

    private void SetHeldVisual(bool isVisible)
    {
        if (heldSampleVisual != null)
            heldSampleVisual.SetActive(isVisible);
    }
}
