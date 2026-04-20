using UnityEngine;

public class DropperVR : MonoBehaviour
{
    [Header("References")]
    public Transform tip;
    public Renderer liquidRenderer;

    [Header("Settings")]
    public Color filledColor = Color.blue;

    private bool isFilled = false;

    void Start()
    {
        SetColor(Color.clear); // start empty
    }

    void OnTriggerEnter(Collider other)
    {
        // 🟦 TOUCHING INDICATOR LIQUID
        if (other.CompareTag("IndicatorLiquid") && !isFilled)
        {
            FillDropper();
        }

        // 🧪 TOUCHING FLASK
        if (other.CompareTag("BaseSolution") && isFilled)
        {
            FlaskReaction flask = other.GetComponent<FlaskReaction>();

            if (flask != null)
            {
                ReleaseDrop(flask);
            }
        }
    }

    void FillDropper()
    {
        isFilled = true;
        SetColor(filledColor);

        Debug.Log("Dropper filled");
    }

    void ReleaseDrop(FlaskReaction flask)
    {
        isFilled = false;
        SetColor(Color.clear);

        flask.AddIndicator();

        Debug.Log("Indicator dropped into flask");
    }

    void SetColor(Color color)
    {
        liquidRenderer.material.color = color;
    }
}