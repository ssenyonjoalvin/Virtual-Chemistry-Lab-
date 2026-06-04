using TMPro;
using UnityEngine;

/// <summary>
/// Controls the magnesium combustion practical in clear steps.
/// This is the main script that updates the in-scene UI and validates step progression.
/// </summary>
public class CombustionExperimentManager : MonoBehaviour
{
    [Header("World Space UI")]
    [Tooltip("Main instruction label shown above the table.")]
    [SerializeField] private TextMeshProUGUI instructionText;

    [Tooltip("Optional label for equation/progress text.")]
    [SerializeField] private TextMeshProUGUI detailText;

    [Header("Flow")]
    [SerializeField] private bool startAutomatically = true;
    [SerializeField] private float autoStartDelay = 0.2f;
    [Tooltip("If enabled, UI Next/Back buttons can manually move between steps.")]
    [SerializeField] private bool allowManualStepNavigation = true;

    /// <summary>
    /// Read-only step index for other scripts.
    /// 1..6 where 6 = completed.
    /// </summary>
    public int CurrentStep => _currentStep;

    private int _currentStep;
    private bool _isRunning;
    private bool _reactionInProgress;

    private const string Step1Text = "Put on Safety Goggles before proceeding";
    private const string Step2Text = "Turn on the Bunsen Burner";
    private const string Step3Text = "Hold the Magnesium ribbon with tongs";
    private const string Step4Text = "Bring the Magnesium close to the flame";
    private const string Step5Text = "Combustion started... observe reaction progress";
    private const string Step6Text = "2Mg + O2 -> 2MgO - Combustion Complete!";
    private const string EquationText = "2Mg + O2 -> 2MgO";
    private const int FirstStep = 1;
    private const int FinalStep = 6;

    private void Start()
    {
        if (startAutomatically)
        {
            Invoke(nameof(BeginExperiment), autoStartDelay);
        }
    }

    /// <summary>
    /// Starts (or restarts) the full combustion practical flow.
    /// Call this from your teleport flow if you want manual start.
    /// </summary>
    public void BeginExperiment()
    {
        _isRunning = true;
        _reactionInProgress = false;
        _currentStep = FirstStep;
        RefreshUi();
    }

    /// <summary>
    /// UI button method for moving one step forward (Back/Next canvas flow).
    /// Assign this to your "Next" button OnClick event.
    /// </summary>
    public void GoToNextStepManual()
    {
        if (!_isRunning || !allowManualStepNavigation) return;
        SetStepManually(_currentStep + 1);
    }

    /// <summary>
    /// UI button method for moving one step backward.
    /// Assign this to your "Back" button OnClick event.
    /// </summary>
    public void GoToPreviousStepManual()
    {
        if (!_isRunning || !allowManualStepNavigation) return;
        SetStepManually(_currentStep - 1);
    }

    /// <summary>
    /// Optional UI button method if you want direct jump buttons (e.g., Step 1, Step 2...).
    /// </summary>
    public void SetStepFromButton(int stepNumber)
    {
        if (!_isRunning || !allowManualStepNavigation) return;
        SetStepManually(stepNumber);
    }

    /// <summary>
    /// Called when goggles are selected/used.
    /// </summary>
    public void OnGogglesPutOn()
    {
        if (!CanAdvanceFrom(1)) return;
        AdvanceTo(2);
    }

    /// <summary>
    /// Called when burner toggles ON.
    /// </summary>
    public void OnBurnerTurnedOn()
    {
        if (!CanAdvanceFrom(2)) return;
        AdvanceTo(3);
    }

    /// <summary>
    /// Called when tongs (while held) touch magnesium ribbon.
    /// </summary>
    public void OnTongsHoldingMagnesium()
    {
        if (!CanAdvanceFrom(3)) return;
        AdvanceTo(4);
    }

    /// <summary>
    /// Called when magnesium comes close to flame area.
    /// </summary>
    public void OnRibbonNearFlame()
    {
        if (!CanAdvanceFrom(4)) return;
        AdvanceTo(5);
    }

    /// <summary>
    /// Called by reaction script when visual combustion begins.
    /// </summary>
    public void OnCombustionReactionStarted()
    {
        if (!_isRunning || _currentStep != 5) return;
        _reactionInProgress = true;
        RefreshUi();
    }

    /// <summary>
    /// Live progress update during the 3-second combustion effect.
    /// progress01 should be 0..1
    /// </summary>
    public void UpdateReactionProgress(float progress01)
    {
        if (!_isRunning || _currentStep != 5) return;

        _reactionInProgress = true;
        int percent = Mathf.RoundToInt(Mathf.Clamp01(progress01) * 100f);

        if (detailText != null)
        {
            detailText.text = EquationText + "\nReaction Progress: " + percent + "%";
        }
        else if (instructionText != null)
        {
            instructionText.text = Step5Text + "\nReaction Progress: " + percent + "%";
        }
    }

    /// <summary>
    /// Called by reaction script once reaction sequence ends.
    /// </summary>
    public void OnCombustionFinished()
    {
        if (!_isRunning) return;
        _reactionInProgress = false;
        _currentStep = 6;
        RefreshUi();
    }

    /// <summary>
    /// Helper for trigger scripts.
    /// </summary>
    public bool IsWaitingForNearFlame()
    {
        return _isRunning && _currentStep == 4;
    }

    /// <summary>
    /// Helper for reaction script.
    /// </summary>
    public bool IsWaitingForReaction()
    {
        return _isRunning && _currentStep == 5;
    }

    private bool CanAdvanceFrom(int step)
    {
        return _isRunning && _currentStep == step;
    }

    private void AdvanceTo(int nextStep)
    {
        _currentStep = nextStep;
        RefreshUi();
    }

    private void SetStepManually(int targetStep)
    {
        _reactionInProgress = false;
        _currentStep = Mathf.Clamp(targetStep, FirstStep, FinalStep);
        RefreshUi();
    }

    private void RefreshUi()
    {
        if (!_isRunning)
            return;

        string stepMessage = _currentStep switch
        {
            1 => Step1Text,
            2 => Step2Text,
            3 => Step3Text,
            4 => Step4Text,
            5 => Step5Text,
            _ => Step6Text
        };

        if (instructionText != null)
        {
            instructionText.text = stepMessage;
        }

        if (detailText != null)
        {
            if (_currentStep == 6)
            {
                detailText.text = EquationText;
            }
            else if (_currentStep == 5 && _reactionInProgress)
            {
                // Reaction progress text is actively updated by UpdateReactionProgress.
            }
            else
            {
                detailText.text = string.Empty;
            }
        }
    }
}
