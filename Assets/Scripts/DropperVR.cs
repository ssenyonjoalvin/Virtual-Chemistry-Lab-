using UnityEngine;

public class DropperVR : MonoBehaviour
{
    [Header("References")]
    public Transform tip;
    public Renderer liquidRenderer;

    [Header("Settings")]
    public Color filledColor = Color.blue;

    private bool isFilled = false;
    public TitrationExperimentManager experimentManager;
    private FlaskReaction currentFlask = null; 

    void Start()
    {
        SetColor(Color.clear);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IndicatorLiquid") && !isFilled)
        {
            FillDropper();
        }

        if (other.CompareTag("BaseSolution"))
        {
            // FIXED: Now it searches the object AND its parents for the script!
            currentFlask = other.GetComponentInParent<FlaskReaction>();
            
            // if (currentFlask != null)
            // {
            //     if (experimentManager != null) 
            //         experimentManager.DebugToVR("Dropper is inside the flask!");
            // }
            // else
            // {
            //     if (experimentManager != null) 
            //         experimentManager.DebugToVR("ERROR: Found the trigger, but couldn't find the FlaskReaction script!");
            // }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseSolution"))
        {
            currentFlask = null;
            // if (experimentManager != null) 
            //     experimentManager.DebugToVR("Dropper left the flask.");
        }
    }

    // 🔴 THIS IS THE TRIGGER BUTTON METHOD
    public void SqueezeDropper() 
    {
        // 1. Did the button press actually register?
        // if (experimentManager != null) experimentManager.DebugToVR("Trigger Button Pressed!");

        if (isFilled) 
        {
            if (currentFlask != null)
            {
                ReleaseDrop(); // Success!
            }
            else
            {
                // 2. Button worked, it's full, but it can't find the flask!
                // if (experimentManager != null) experimentManager.DebugToVR("FAILED: Squeezed, but not touching flask.");
            }
        }
        else 
        {
            // 3. Button worked, but the dropper is empty!
            // if (experimentManager != null) experimentManager.DebugToVR("FAILED: Squeezed, but dropper is empty.");
        }
    }

    void FillDropper()
    {
        isFilled = true;
        SetColor(filledColor);

        if (experimentManager != null)
            experimentManager.OnDropperFilled();
    }

    void ReleaseDrop()
    {
        isFilled = false;
        SetColor(Color.clear);

        if (currentFlask != null)
        {
            currentFlask.AddIndicator();
            if (experimentManager != null) experimentManager.OnIndicatorAddedToFlask();
        }
    }

    void SetColor(Color color)
    {
        if(liquidRenderer != null) liquidRenderer.material.color = color;
    }
}