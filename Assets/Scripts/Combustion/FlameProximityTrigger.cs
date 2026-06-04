using UnityEngine;

/// <summary>
/// Outer trigger around the flame.
/// When magnesium enters this zone, Step 4 advances to Step 5 (ready to react).
/// </summary>
[RequireComponent(typeof(Collider))]
public class FlameProximityTrigger : MonoBehaviour
{
    [SerializeField] private CombustionExperimentManager experimentManager;

    private Collider _trigger;

    private void Awake()
    {
        _trigger = GetComponent<Collider>();
        _trigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (experimentManager == null) return;

        CombustionMagnesiumRibbon magnesium = other.GetComponentInParent<CombustionMagnesiumRibbon>();
        if (magnesium == null) return;

        if (experimentManager.IsWaitingForNearFlame())
        {
            experimentManager.OnRibbonNearFlame();
        }
    }
}
