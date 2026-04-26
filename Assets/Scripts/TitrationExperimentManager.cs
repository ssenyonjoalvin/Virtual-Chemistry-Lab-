using UnityEngine;
using TMPro;

public class TitrationExperimentManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject startPanel;
    public TextMeshProUGUI instructionText;

    [Header("Objects")]
    public GameObject indicatorBottle;
    public GameObject dropper;
    public GameObject flask;
    public GameObject button; // For starting the pour in Step 4
    public GameObject finishButton; // For starting the pour in Step 4




    [Header("Helpers")]
    public FloatingArrow arrow;

    private int currentStep = 0;
    private GameObject currentlyHighlightedObject;

    public void StartExperiment()
    {
        if (startPanel != null) startPanel.SetActive(false);
        currentStep = 1;
        ShowStep();
    }

    void ShowStep()
    {
        switch (currentStep)
        {
            case 1:
                UpdateUI("Step 1: Pick up the dropper");
                Highlight(dropper);
                break;

            case 2:
                UpdateUI("Step 2: Dip dropper into the indicator bottle to fill it");
                Highlight(indicatorBottle);
                break;

            case 3:
                UpdateUI("Step 3: Move dropper over the flask and press Trigger to add it");
                Highlight(flask);
                break;

            case 4:
                UpdateUI("Step 4: Press the button on the stand to start pouring");
                Highlight(button); // Add button reference later if you want
                break;

            case 5:
                UpdateUI("Step 5: Pouring... Waiting for endpoint.");
                Highlight(flask);
                break;

            default:
                UpdateUI("Experiment Complete! Color Changed.");
                RemoveHighlight();
                break;
        }
    }

    // --- UI HELPER METHODS ---
    
    void UpdateUI(string message) 
    {
        if (instructionText != null) 
            instructionText.text = message;
    }

    public void DebugToVR(string debugMessage)
    {
        if (instructionText != null) 
            instructionText.text += "\n<color=#00FF00>[DEBUG] " + debugMessage + "</color>";
    }

    // NEW: Live Volume Monitor for debugging the color issue
    public void UpdateLiveVolumeMonitor(float current, float target, bool hasIndicator)
    {
        if (currentStep == 5 && instructionText != null)
        {
            string indicatorStatus = hasIndicator ? "<color=#00FF00>YES</color>" : "<color=#FF0000>NO</color>";
            instructionText.text = $"Step 5: Pouring...\n" +
                                   $"Flask Volume: {current:F1} / {target}\n" +
                                   $"Has Indicator: {indicatorStatus}";
        }
    }

    // --- STEP EVENTS ---

    public void OnDropperGrabbed()
    {
        // DebugToVR("Dropper Grabbed!");
        if (currentStep == 1) NextStep();
    }

    public void OnDropperFilled()
    {
        // DebugToVR("Dropper Filled!");
        if (currentStep == 2) NextStep();
    }

    public void OnIndicatorAddedToFlask()
    {
        // DebugToVR("Indicator Added!");
        if (currentStep == 3) NextStep();
    }

    public void OnPouringStarted()
    {
        // DebugToVR("Pouring Started!");
        if (currentStep == 4) NextStep();
    }

    public void OnEndpointReached(float result)
    {
        // 1. Move the step to 6 so the Live Monitor stops running!
        currentStep = 6; 

        // 2. Update the final UI text
        UpdateUI("Experiment Complete!\nConcentration = " + result.ToString("F3") + " M");
        
        // 3. Turn on the finish button
        if (finishButton != null) finishButton.SetActive(true);

        // 4. Remove any remaining highlights
        RemoveHighlight();
    }

    private void NextStep()
    {
        currentStep++;
        ShowStep();
    }

    // --- HIGHLIGHT SYSTEM (Fully Activated) ---

    void Highlight(GameObject obj)
    {
        RemoveHighlight();
        currentlyHighlightedObject = obj;

        if (obj != null)
        {
            ObjectHighlighter highlighter = obj.GetComponent<ObjectHighlighter>();
            if (highlighter != null)
            {
                highlighter.EnableHighlight();
            }

            if (arrow != null)
            {
                arrow.SetTarget(obj.transform);
            }
        }
    }

    void RemoveHighlight()
    {
        if (currentlyHighlightedObject != null)
        {
            ObjectHighlighter highlighter = currentlyHighlightedObject.GetComponent<ObjectHighlighter>();
            if (highlighter != null)
            {
                highlighter.DisableHighlight();
            }
            currentlyHighlightedObject = null;
        }

        if (arrow != null)
        {
            arrow.Hide();
        }
    }
}