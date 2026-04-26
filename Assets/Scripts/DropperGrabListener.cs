using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DropperGrabListener : MonoBehaviour
{
    public TitrationExperimentManager experimentManager;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Dropper grabbed");

        if (experimentManager != null)
            experimentManager.OnDropperGrabbed();
    }
}