using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Controls burner ON/OFF behavior.
/// Uses XRSimpleInteractable so the user can click/select it in VR.
/// </summary>
[RequireComponent(typeof(XRSimpleInteractable))]
public class BunsenBurner : MonoBehaviour
{
    [Header("Burner Visuals")]
    [Tooltip("Particle system for flame. Keep disabled by default in the scene.")]
    [SerializeField] private ParticleSystem flameParticles;

    [Tooltip("Point light for flame glow. Keep disabled by default in the scene.")]
    [SerializeField] private Light flameLight;

    [Header("Flame Light Settings")]
    [SerializeField] private Color flameColor = new Color(1f, 0.45f, 0.15f);
    [SerializeField] private float flameIntensity = 1.3f;
    [SerializeField] private float flameRange = 1.3f;

    [Header("Optional Step Progress Reference")]
    [SerializeField] private CombustionExperimentManager experimentManager;

    private XRSimpleInteractable _interactable;
    private bool _isOn;

    /// <summary>
    /// Other scripts can check if the burner is currently ON.
    /// </summary>
    public bool IsOn => _isOn;

    private void Awake()
    {
        _interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnSelected);
    }

    private void Start()
    {
        // Start OFF as requested.
        _isOn = false;
        ApplyState();
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        ToggleBurner();
    }

    /// <summary>
    /// Flips ON/OFF state and updates effects.
    /// </summary>
    public void ToggleBurner()
    {
        _isOn = !_isOn;
        ApplyState();

        // Step 2 should complete when burner turns ON.
        if (_isOn && experimentManager != null)
        {
            experimentManager.OnBurnerTurnedOn();
        }
    }

    private void ApplyState()
    {
        if (flameParticles != null)
        {
            if (_isOn)
            {
                flameParticles.Play();
            }
            else
            {
                flameParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        if (flameLight != null)
        {
            flameLight.enabled = _isOn;
            flameLight.color = flameColor;
            flameLight.intensity = flameIntensity;
            flameLight.range = flameRange;
        }
    }
}
