using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class EnsureXRGrabbable : MonoBehaviour
{
    [Header("Physics")]
    public float mass = 0.05f;
    public bool useGravity = true;
    public CollisionDetectionMode collisionDetection = CollisionDetectionMode.ContinuousDynamic;
    public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;

    [ContextMenu("Apply XR Grab Setup")]
    public void ApplySetup()
    {
        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        // Ensure there is at least one collider for interaction.
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }

        // Dynamic rigidbodies cannot use non-convex mesh colliders.
        if (col is MeshCollider meshCollider && !meshCollider.convex)
        {
            meshCollider.convex = true;
        }

        rb.mass = mass;
        rb.useGravity = useGravity;
        rb.isKinematic = false;
        rb.collisionDetectionMode = collisionDetection;
        rb.interpolation = interpolation;

        // Keep interaction on default layer mask unless overridden in inspector.
        //if (grab.interactionLayers.value == 0)
        //{
        //    grab.interactionLayers = InteractionLayerMask.GetMask("Default");
        //}
    }

    private void Reset()
    {
        ApplySetup();
    }

    private void Awake()
    {
        ApplySetup();
    }
}
