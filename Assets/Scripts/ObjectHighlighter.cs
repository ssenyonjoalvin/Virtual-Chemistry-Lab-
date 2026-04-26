using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    private Material mat;
    private Color originalEmission;
    private bool isHighlighted = false;

    public Color highlightColor = Color.yellow;
    public float intensity = 2f;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;

        // Save original emission
        if (mat.HasProperty("_EmissionColor"))
        {
            originalEmission = mat.GetColor("_EmissionColor");
        }
    }

    public void EnableHighlight()
    {
        if (mat == null) return;

        isHighlighted = true;

        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", highlightColor * intensity);
    }

    public void DisableHighlight()
    {
        if (mat == null) return;

        isHighlighted = false;

        mat.SetColor("_EmissionColor", originalEmission);
    }
}