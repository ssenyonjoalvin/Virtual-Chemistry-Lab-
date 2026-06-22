using UnityEngine;

public class DropperVRUnguided : MonoBehaviour
{
    [Header("References")]
    public Transform tip;
    public Renderer liquidRenderer;

    [Header("Liquid Source Materials")]
    public Material indicatorMaterial; // Drag the blue liquid material here
    public Material baseMaterial;      // Drag the white liquid material here

    private bool isFilled = false;
    private string currentLiquidType = ""; 
    
    public TitrationExperimentManager experimentManager;
    private FlaskReactionUnguided currentFlask = null; 

    void Start() { SetColor(Color.clear); }

    void OnTriggerEnter(Collider other)
    {
        if (!isFilled)
        {
            // Try to find a Renderer on the object we just touched
            Renderer sourceRenderer = other.GetComponent<Renderer>();
            
            if (other.CompareTag("IndicatorLiquid") && indicatorMaterial != null) 
            {
                FillDropper("Indicator", indicatorMaterial.color);
            }
            else if (other.CompareTag("BaseSolutionSource") && baseMaterial != null) 
            {
                FillDropper("Base", baseMaterial.color);
            }
        }

        if (other.CompareTag("BaseSolution"))
        {
            currentFlask = other.GetComponentInParent<FlaskReactionUnguided>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseSolution")) { currentFlask = null; }
    }

    public void SqueezeDropper() 
    {
        if (isFilled && currentFlask != null)
        {
            ReleaseDrop();
        }
    }

    void FillDropper(string type, Color color)
    {
        isFilled = true;
        currentLiquidType = type;
        SetColor(color);
        if (experimentManager != null) experimentManager.OnDropperFilled();
    }

    void ReleaseDrop()
    {
        isFilled = false;
        SetColor(Color.clear);

        if (currentFlask != null)
        {
            if (currentLiquidType == "Indicator")
                currentFlask.AddIndicator();
            else if (currentLiquidType == "Base")
                currentFlask.AddBaseSolution(5.0f); 
        }
    }

    // This updates the internal liquid color
    void SetColor(Color color) 
    { 
        if(liquidRenderer != null) 
        {
            liquidRenderer.material.color = color;
            // If your shader uses _BaseColor, sync that too
            if(liquidRenderer.material.HasProperty("_BaseColor"))
                liquidRenderer.material.SetColor("_BaseColor", color);
        }
    }

    public void ResetDropper() { isFilled = false; SetColor(Color.clear); }
}