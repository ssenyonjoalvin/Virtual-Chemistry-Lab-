using UnityEngine;
using TMPro;

[ExecuteInEditMode] // This makes the label update in the Unity editor before you even hit Play!
public class ChemicalLabel : MonoBehaviour
{
    [Header("Label Info")]
    public string chemicalFormula = "HCl";
    public string chemicalName;
    public string concentration;

    [Header("Colors")]
    public Color labelColor = Color.white;
    public Color textColor = Color.black;

    [Header("UI References")]
    public UnityEngine.UI.Image background;
    public TextMeshProUGUI textMesh;

    [Header("VR Settings")]
    [Tooltip("If true, the label will always rotate to face the player's headset.")]
    public bool alwaysFacePlayer = true;
    
    // Internal reference to the VR Headset (Main Camera)
    private Transform vrCamera;

    void Start()
    {
        // Find the VR Headset camera when the game starts
        if (Application.isPlaying && Camera.main != null)
        {
            vrCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        // 1. UPDATE THE UI TEXT AND COLORS
        if (textMesh != null)
        {
            textMesh.text = $"<b><size=80%>{chemicalFormula}</size></b>\n{chemicalName}\n<i>{concentration}</i>";
            textMesh.color = textColor;
        }

        if (background != null)
        {
            background.color = labelColor;
        }

        // 2. VR BILLBOARDING (Only runs in Play Mode)
        if (alwaysFacePlayer && Application.isPlaying && vrCamera != null)
        {
            // This math makes the canvas face the camera WITHOUT the text being backwards
            transform.forward = transform.position - vrCamera.position;
            
            // Optional: If you want the label to stay perfectly straight up/down 
            // (like a real bottle) and ONLY pivot left/right, uncomment the 3 lines below:
            
            // Vector3 flatRotation = transform.eulerAngles;
            // flatRotation.x = 0; flatRotation.z = 0;
            // transform.eulerAngles = flatRotation;
        }
    }
}