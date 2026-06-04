using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [Header("UI Info")]
    [Tooltip("The clean name of this object to show the player")]
    public string objectDisplayName = "Medical Dropper";

    [Header("Highlight Settings")]
    public Color highlightColor = new Color(0.2f, 1f, 0.2f, 1f);
    public float pulseSpeed = 2f;
    public float maxBrightness = 1.5f;

    private Renderer meshRenderer;
    private Material mat;
    private Color originalEmission;
    private bool isHighlighted = false;

    void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (meshRenderer != null)
        {
            mat = meshRenderer.material; 
            mat.EnableKeyword("_EMISSION"); 
            if (mat.HasProperty("_EmissionColor"))
                originalEmission = mat.GetColor("_EmissionColor");
        }
    }

    void Update()
    {
        if (isHighlighted && mat != null)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            Color finalColor = highlightColor * (pulse * maxBrightness);
            mat.SetColor("_EmissionColor", finalColor);
        }
    }

    public void EnableHighlight() { isHighlighted = true; }

    public void DisableHighlight()
    {
        isHighlighted = false;
        if (mat != null) mat.SetColor("_EmissionColor", originalEmission);
    }
}