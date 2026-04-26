using UnityEngine;

public class FlaskReaction : MonoBehaviour
{
    [Header("Liquid Settings")]
    public Renderer liquidRenderer;

    [Header("Titration Settings")]
    public float currentVolume = 0f;
    public float endpointVolume = 50f;

    [Header("Calculation Settings")]
    public float titrantConcentration = 0.1f; // C1
    public float analyteVolume = 25f; // V2 (mL)

    public TitrationExperimentManager experimentManager;

    private bool hasIndicator = false;
    private bool hasChangedColor = false;

    void Start()
    {
        SetColor(new Color(1f, 1f, 1f, 0.2f)); // colorless
    }

    public bool GetHasIndicator() { return hasIndicator; } // Add this little helper

    public void AddLiquid(float amount)
    {
        currentVolume += amount;

        // NEW: Tell the UI board exactly what is happening inside the flask!
        if (experimentManager != null)
        {
            experimentManager.UpdateLiveVolumeMonitor(currentVolume, endpointVolume, hasIndicator);
        }

        CheckReaction();
    }

    void CheckReaction()
    {
        if (!hasChangedColor && hasIndicator && currentVolume >= endpointVolume)
        {
            ChangeColor();
        }
    }

    float CalculateConcentration()
{
    return (titrantConcentration * currentVolume) / analyteVolume;
}

    void ChangeColor()
{
    hasChangedColor = true;

    SetColor(new Color(1f, 0.2f, 0.6f, 0.5f));

    float result = CalculateConcentration();

    Debug.Log("Endpoint reached!");
    Debug.Log("Concentration: " + result + " M");

    if (experimentManager != null)
    {
        experimentManager.OnEndpointReached(result);
    }
}

    void SetColor(Color color)
    {
        Material mat = liquidRenderer.material;

        mat.color = color;
        mat.SetColor("_BaseColor", color);
        mat.SetColor("_EmissionColor", color * 2f);
    }

    public void AddIndicator()
    {
        hasIndicator = true;

        // Slight yellow tint
        SetColor(new Color(1f, 1f, 0.5f, 0.3f));

        Debug.Log("Indicator added");
    }

    
}