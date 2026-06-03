using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Place this on the safety goggles placeholder object.
/// On XR select, it notifies the experiment manager that goggles are put on.
/// </summary>
[RequireComponent(typeof(XRSimpleInteractable))]
public class CombustionGogglesInteractable : MonoBehaviour
{
    [SerializeField] private CombustionExperimentManager experimentManager;

    private XRSimpleInteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (experimentManager != null)
        {
            experimentManager.OnGogglesPutOn();
        }
    }
}
