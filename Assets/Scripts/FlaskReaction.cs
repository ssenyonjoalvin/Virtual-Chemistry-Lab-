using UnityEngine;

public class FlaskReaction : MonoBehaviour
{
    [Header("Liquid Settings")]
    public Renderer liquidRenderer;

    [Header("Titration Settings")]
    public float currentVolume = 0f;
    public float endpointVolume = 50f;

    private bool hasIndicator = false;

    private bool hasChangedColor = false;

    void Start()
    {
        SetColor(new Color(1f, 1f, 1f, 0.2f)); // colorless
    }

    public void AddLiquid(float amount)
    {
        currentVolume += amount;

        Debug.Log("Current Volume: " + currentVolume);

        CheckReaction();
    }

   void CheckReaction()
{
    if (!hasChangedColor && hasIndicator && currentVolume >= endpointVolume)
    {
        ChangeColor();
    }
}

    void ChangeColor()
    {
        hasChangedColor = true;

        // Pink color (phenolphthalein)
        SetColor(new Color(1f, 0.2f, 0.6f, 0.5f));

        Debug.Log("Endpoint reached → Color changed!");
    }

    void SetColor(Color color)
    {
        liquidRenderer.material.color = color;
    }

    public void AddIndicator()
    {
        hasIndicator = true;
        Debug.Log("Indicator added");

        // Slight yellow tint to show indicator presence
        SetColor(new Color(1f, 1f, 0.5f, 0.3f));
    }
}