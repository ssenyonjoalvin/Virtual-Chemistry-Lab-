using UnityEngine;

public class FlaskReaction : MonoBehaviour
{
    [Header("Liquid Settings")]
    public Renderer liquidRenderer;

    [Header("Titration Settings")]
    public float currentVolume = 0f; // Added titrant
    public float endpointVolume = 50f;
    public float maxFlaskVolume = 250f; // Total capacity (mL)

    [Header("Visual Mapping")]
    public float shaderFillMin = 0f;
    public float shaderFillMax = 0.1f; // Matches liquid mesh height (0.1 units)

    [Header("Calculation Settings")]
    public float titrantConcentration = 0.1f; // C1
    public float analyteVolume = 25f; // Initial volume in flask (mL)

    
    [Tooltip("Real-life drop = 0.05 mL. If it fills too fast, lower this number!")]
    public float volumePerDrop = 0.002f; 

    [Header("Visual Colors")]

    public Color initialColor = new Color(1f, 1f, 1f, 0.2f);
    public Color indicatorColor = new Color(1f, 1f, 0.5f, 0.3f);
    public Color endpointColor = new Color(1f, 0.2f, 0.6f, 0.5f);
    public Color overTitrationColor = new Color(0.8f, 0f, 0.4f, 0.7f);

    public TitrationExperimentManager experimentManager;

    private bool hasIndicator = false;
    private bool hasChangedColor = false;
    private Vector3 lastPosition;
    private float swirlSpeed = 0f;

    void Start()
    {
        SetColor(initialColor);
        lastPosition = transform.position;
    }

    void Update()
    {
        UpdateSwirl();
        UpdateVisuals();
    }

   void OnParticleCollision(GameObject other)
    {
        // Adds accurate volume every time a particle physically hits
        AddLiquid(volumePerDrop);
    }

    void UpdateSwirl()
    {
        float dist = Vector3.Distance(transform.position, lastPosition);
        float currentSpeed = dist / Mathf.Max(Time.deltaTime, 0.001f);
        swirlSpeed = Mathf.Lerp(swirlSpeed, currentSpeed, Time.deltaTime * 5f);
        lastPosition = transform.position;
    }

    void UpdateVisuals()
    {
        if (liquidRenderer == null) return;

        // 1. Calculate true scientific percentage (e.g. 25mL / 250mL = 0.1 or 10%)
        float totalVolume = analyteVolume + currentVolume;
        float trueFillPercentage = Mathf.Clamp01(totalVolume / maxFlaskVolume);
        
        // 2. Map the 0-100% value to the Shader's height range (0 to 0.1 units)
        float finalShaderValue = Mathf.Lerp(shaderFillMin, shaderFillMax, trueFillPercentage);
        liquidRenderer.material.SetFloat("_Float", finalShaderValue);

        if (!hasIndicator) return;


        Color targetColor;
        float diff = currentVolume - endpointVolume;

        if (diff < 0)
        {
            float t = Mathf.Clamp01(currentVolume / endpointVolume);
            float pinkInfluence = Mathf.Pow(t, 8); 
            if (swirlSpeed > 0.1f) pinkInfluence *= 0.1f;
            targetColor = Color.Lerp(indicatorColor, endpointColor, pinkInfluence);
        }
        else if (diff < 2f)
        {
            targetColor = endpointColor;
        }
        else
        {
            float t = Mathf.Clamp01((diff - 2f) / 10f);
            targetColor = Color.Lerp(endpointColor, overTitrationColor, t);
        }

        SetColor(targetColor);
    }

    public bool GetHasIndicator() { return hasIndicator; }

    public void AddLiquid(float amount)
    {
        currentVolume += amount;
        if (experimentManager != null) experimentManager.UpdateLiveVolumeMonitor(currentVolume, endpointVolume, hasIndicator);
        CheckReaction();
    }

    void CheckReaction()
    {
        if (!hasChangedColor && hasIndicator && currentVolume >= endpointVolume)
        {
            if (swirlSpeed > 0.5f || currentVolume > endpointVolume + 1f)
            {
                ChangeColor();
            }
        }
    }

    void ChangeColor()
    {
        hasChangedColor = true;
        float result = CalculateConcentration();
        if (experimentManager != null) experimentManager.OnEndpointReached(result);
    }

    float CalculateConcentration()
    {
        return (titrantConcentration * currentVolume) / analyteVolume;
    }

    void SetColor(Color color)
    {
        Material mat = liquidRenderer.material;
        mat.color = color;
        mat.SetColor("_BaseColor", color);
        mat.SetColor("_Color", color);
        mat.SetColor("_EmissionColor", color * 1.2f);
    }

    public void AddIndicator()
    {
        hasIndicator = true;
        SetColor(indicatorColor);
    }

    // 🔴 NEW: Add this to the bottom of FlaskReaction.cs
    public void ResetFlask()
    {
        currentVolume = 0f;
        hasIndicator = false;
        hasChangedColor = false;
        
        SetColor(initialColor);
        
        // Randomize the next test so it isn't the same answer!
        // Generates an unknown concentration between 0.05 and 0.50 M
        float secretConcentration = Random.Range(0.05f, 0.5f); 
        endpointVolume = (secretConcentration * analyteVolume) / titrantConcentration;

        Debug.Log("Flask Reset! New endpoint is: " + endpointVolume);
    }
}