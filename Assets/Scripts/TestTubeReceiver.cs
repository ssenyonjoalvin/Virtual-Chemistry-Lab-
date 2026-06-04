using UnityEngine;
using UnityEngine.Events;

public class TestTubeReceiver : MonoBehaviour
{
    [Header("Accept Rules")]
    [Tooltip("Leave empty to accept any substance.")]
    public string acceptedSubstanceId = "";

    [Header("Progress")]
    public int requiredScoops = 1;
    public int currentScoops = 0;

    [Header("Visual Fill (Optional)")]
    public Transform fillVisual;
    public Renderer fillRenderer;
    public Vector3 emptyScale = new Vector3(1f, 0f, 1f);
    public Vector3 fullScale = new Vector3(1f, 1f, 1f);

    [Header("Events")]
    public UnityEvent onScoopReceived;
    public UnityEvent onFilled;

    private bool hasInvokedFilled = false;

    private void Start()
    {
        UpdateFillVisual();
    }

    public bool TryReceive(string substanceId, Material sampleMaterial)
    {
        if (!string.IsNullOrWhiteSpace(acceptedSubstanceId) && acceptedSubstanceId != substanceId)
            return false;

        currentScoops++;

        if (fillRenderer != null && sampleMaterial != null)
            fillRenderer.material = sampleMaterial;

        UpdateFillVisual();
        onScoopReceived?.Invoke();

        if (!hasInvokedFilled && currentScoops >= requiredScoops)
        {
            hasInvokedFilled = true;
            onFilled?.Invoke();
        }

        return true;
    }

    private void UpdateFillVisual()
    {
        if (fillVisual == null) return;
        float t = requiredScoops <= 0 ? 1f : Mathf.Clamp01((float)currentScoops / requiredScoops);
        fillVisual.localScale = Vector3.Lerp(emptyScale, fullScale, t);
    }
}
