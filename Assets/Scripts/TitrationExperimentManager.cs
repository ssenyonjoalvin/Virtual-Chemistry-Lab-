using UnityEngine;
using TMPro;

public class TitrationExperimentManager : MonoBehaviour
{
    [System.Serializable]
    public class StepVoiceLine
    {
        public int stepNumber;
        public AudioClip clip;
    }

    [Header("UI")]
    public GameObject startPanel;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI volumeText; 

    [Header("Experiment Values (Displayed to Student)")]
    public string knownTitrantConcentration = "0.1 M NaOH";
    public string knownAnalyteVolume = "25.0 mL HCl";
    
    [Header("Objects")]
    public GameObject indicatorBottle;
    public GameObject dropper;
    public GameObject flask;
    public GameObject Stopcock; 
    public GameObject finishButton; 

    [Header("Helpers")]
    public FloatingArrow arrow;

    [Header("Voice Guidance")]
    public AudioSource voiceSource;
    public bool playVoiceGuidance = true;
    public StepVoiceLine[] stepVoiceLines;
    public bool loadNarrationFromResources = true;
    public string resourcesNarrationFolder = "TitrationNarration";
    [Range(0f, 1f)]
    public float voiceVolume = 1f;

    private int currentStep = 0;
    private GameObject currentlyHighlightedObject;

    private Vector3 dropperStartPos;
    private Quaternion dropperStartRot;


[Header("Scenario")]
public GameObject scenarioPanel;
public TextMeshProUGUI scenarioText;

void Start()
{
    if (scenarioPanel != null)
        scenarioPanel.SetActive(true);

    if (startPanel != null)
        startPanel.SetActive(false);

    if (dropper != null)
    {
        dropperStartPos = dropper.transform.position;
        dropperStartRot = dropper.transform.rotation;
    }
}
    void Awake()
    {
        EnsureVoiceSource();
    }


    void EnsureVoiceSource()
    {
        if (voiceSource != null) return;
        voiceSource = GetComponent<AudioSource>();
        if (voiceSource == null) voiceSource = gameObject.AddComponent<AudioSource>();
        voiceSource.playOnAwake = false;
        voiceSource.loop = false;
        voiceSource.spatialBlend = 0f;
    }

    public void StartExperiment()
    {
        if (startPanel != null) startPanel.SetActive(false);
        currentStep = 1;
        ShowStep();
    }

    public void ResetExperiment()
    {
        currentStep = 0;

        // if (startPanel != null) startPanel.SetActive(true);
        if (finishButton != null) finishButton.SetActive(false);
        if (volumeText != null) volumeText.text = ""; 
        
        UpdateUI("Experiment Reset. Press Start to try again.");
        RemoveHighlight();

        if (dropper != null)
        {
            dropper.transform.position = dropperStartPos;
            dropper.transform.rotation = dropperStartRot;
            Rigidbody rb = dropper.GetComponent<Rigidbody>();
            if (rb != null) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
            DropperVR dropScript = dropper.GetComponent<DropperVR>();
            if (dropScript != null) dropScript.ResetDropper();
        }

        if (flask != null)
        {
            FlaskReaction flaskScript = flask.GetComponent<FlaskReaction>();
            if (flaskScript != null) flaskScript.ResetFlask();
        }

        if (Stopcock != null)
        {
            PipettePour pourScript = Stopcock.GetComponent<PipettePour>();
            if (pourScript != null) pourScript.StopPouring();
        }
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
                UpdateUI("Step 4: Pinch and hold the stopcock to dispense liquid.");
                Highlight(Stopcock);
                break;
            case 5:
                UpdateUI("Step 5: Pouring... Wait for the endpoint (Pink Color).");
                Highlight(flask);
                break;
            default:
                break;
        }
        PlayVoiceForStep(currentStep);
    }

    void UpdateUI(string currentTaskMessage) 
    { 
        if (instructionText != null) 
        {
            string header =
    $"<color=#000000><b><u>Mission:</u></b></color>\n" +
    $"Determine the concentration of the unknown HCl sample.\n" +
    $"<color=#000000><b><u>Lab Values:</u></b></color>\n" +
    $"Titrant (Burette): <b>{knownTitrantConcentration}</b>\n" +
    $"Analyte (Flask): <b>{knownAnalyteVolume}</b>";

            string task = $"\n<color=#000000><b><u>Current Task:</u></b>\n{currentTaskMessage}</color>";
            
            instructionText.text = header + task;
        }
    }

    public void DebugToVR(string debugMessage) 
    { 
        if (instructionText != null) instructionText.text += "\n<color=#00FF00>[DEBUG] " + debugMessage + "</color>"; 
    }

    public void UpdateLiveVolumeMonitor(float current, float target, bool hasIndicator)
    {
        if (currentStep == 5 && instructionText != null)
        {
            string indicatorStatus = hasIndicator ? "<color=#00FF00>YES</color>" : "<color=#FF0000>NO</color>";
            UpdateUI($"Step 5: Pouring...\nHas Indicator: {indicatorStatus}");
            
            if (volumeText != null) 
                volumeText.text = "Flask Volume:\n" + current.ToString("F2") + " mL";
        }
    }

    public void OnDropperGrabbed() { if (currentStep == 1) NextStep(); }
    public void OnDropperFilled() { if (currentStep == 2) NextStep(); }
    public void OnIndicatorAddedToFlask() { if (currentStep == 3) NextStep(); }
    public void OnPouringStarted() { if (currentStep == 4) NextStep(); }

    public void OnEndpointReached(float result)
    {
        currentStep = 6; 
        
        UpdateUI(
    "<color=#00FF00>Analysis Complete</color>\n" +
    "You have successfully determined the concentration of the unknown acid sample.\n" +
    $"Calculated Concentration: <b>{result:F3} M</b>\n" +
    "This value can now be used by the water treatment facility for quality control."
);
        
        if (finishButton != null) finishButton.SetActive(true);
        
        RemoveHighlight();
        PlayVoiceForStep(currentStep);
    }

    // 🔴 MISSING CODE ADDED BACK HERE:
    private void NextStep() 
    { 
        currentStep++; 
        ShowStep(); 
    }

    void Highlight(GameObject obj)
    {
        RemoveHighlight();
        currentlyHighlightedObject = obj;
        if (obj != null)
        {
            ObjectHighlighter highlighter = obj.GetComponent<ObjectHighlighter>();
            if (highlighter != null) highlighter.EnableHighlight();
        }
    }

    void RemoveHighlight() 
    { 
        if (currentlyHighlightedObject != null)
        {
            ObjectHighlighter highlighter = currentlyHighlightedObject.GetComponent<ObjectHighlighter>();
            if (highlighter != null) highlighter.DisableHighlight();
            currentlyHighlightedObject = null;
        }
        if (arrow != null) arrow.Hide(); 
    }

    void PlayVoiceForStep(int stepNumber)
    {
        if (!playVoiceGuidance) return;
        EnsureVoiceSource();
        if (voiceSource == null) return;
        AudioClip clip = GetClipForStep(stepNumber);
        if (clip == null) return;
        voiceSource.Stop();
        voiceSource.volume = voiceVolume;
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    AudioClip GetClipForStep(int stepNumber)
    {
        if (stepVoiceLines != null)
        {
            for (int i = 0; i < stepVoiceLines.Length; i++)
            {
                StepVoiceLine line = stepVoiceLines[i];
                if (line != null && line.stepNumber == stepNumber && line.clip != null) return line.clip;
            }
        }
        if (loadNarrationFromResources && !string.IsNullOrEmpty(resourcesNarrationFolder))
        {
            string path = $"{resourcesNarrationFolder}/step_{stepNumber}";
            return Resources.Load<AudioClip>(path);
        }
        return null;
    }

    public void ContinueFromScenario()
    {
        if (scenarioPanel != null)
        {
            scenarioPanel.SetActive(false);
        }

        if (instructionText != null)
        {
            instructionText.text = @"You are a quality control technician working at a water treatment plant.
A sample of hydrochloric acid has been received without a concentration label.
Your supervisor has asked you to determine its concentration before it can be used in production.
To solve this problem, perform an acid-base titration using a standard sodium hydroxide solution.
Objective:
Determine the concentration of the unknown hydrochloric acid sample.";
        }

        PlayVoiceForStep(0);

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }
    }
}