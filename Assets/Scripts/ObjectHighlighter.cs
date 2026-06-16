using UnityEngine;
using System.Collections.Generic;
using TMPro; // 🔴 NEW: Required for generating the text

public class ObjectHighlighter : MonoBehaviour
{
    [Header("UI Info")]
    public string objectDisplayName = "Medical Dropper";
    [Tooltip("How high above the object the text should float (in meters)")]
    public float textHeightOffset = 0.05f; 

    [Header("Highlight Settings")]
    public Color highlightColor = new Color(0.2f, 1f, 0.2f, 1f);
    public float pulseSpeed = 2f;
    public float maxBrightness = 1.5f;

    private List<Material> materialsToGlow = new List<Material>();
    private List<Color> originalEmissions = new List<Color>();
    private bool isHighlighted = false;

    // 🔴 NEW: Variables for the floating text
    private GameObject floatingTextObj;
    private Transform vrCamera;

    void Awake()
    {
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        
        foreach (Renderer r in allRenderers)
        {
            foreach (Material mat in r.materials)
            {
                if (mat != null)
                {
                    mat.EnableKeyword("_EMISSION");
                    materialsToGlow.Add(mat);
                    
                    if (mat.HasProperty("_EmissionColor"))
                    {
                        originalEmissions.Add(mat.GetColor("_EmissionColor"));
                    }
                    else
                    {
                        originalEmissions.Add(Color.black); 
                    }
                }
            }
        }
    }

    void Start()
    {
        // Find the player's headset camera automatically
        if (Camera.main != null)
        {
            vrCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        if (isHighlighted)
        {
            // 1. Pulse the colors
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            Color finalColor = highlightColor * (pulse * maxBrightness);
            
            foreach (Material mat in materialsToGlow)
            {
                if (mat != null)
                {
                    if (mat.HasProperty("_EmissionColor"))
                        mat.SetColor("_EmissionColor", finalColor);
                    
                    if (mat.HasProperty("_BaseColor"))
                        mat.SetColor("_BaseColor", Color.Lerp(Color.gray, finalColor, pulse));
                }
            }

            // 2. 🔴 NEW: Make the floating text always face the VR Player
            if (floatingTextObj != null && vrCamera != null)
            {
                // This math rotates the text to look at the camera without being backwards
                floatingTextObj.transform.rotation = Quaternion.LookRotation(floatingTextObj.transform.position - vrCamera.position);
            }
        }
    }

    public void EnableHighlight() 
    { 
        isHighlighted = true; 
        CreateFloatingText(); // 🔴 NEW
    }

    public void DisableHighlight()
    {
        isHighlighted = false;
        
        for (int i = 0; i < materialsToGlow.Count; i++)
        {
            if (materialsToGlow[i] != null)
            {
                if (materialsToGlow[i].HasProperty("_EmissionColor"))
                    materialsToGlow[i].SetColor("_EmissionColor", originalEmissions[i]);
                
                if (materialsToGlow[i].HasProperty("_BaseColor"))
                    materialsToGlow[i].SetColor("_BaseColor", Color.white); 
            }
        }

        DestroyFloatingText(); // 🔴 NEW
    }

    // 🔴 NEW: Code that automatically builds the 3D text
    private void CreateFloatingText()
    {
        if (floatingTextObj != null) return; // Already exists

        // Create an empty object
        floatingTextObj = new GameObject("HighlightLabel_" + objectDisplayName);
        
        // Position it right above the item
        floatingTextObj.transform.position = transform.position + (Vector3.up * textHeightOffset);
        
        // Add the TextMeshPro component
        TextMeshPro tmp = floatingTextObj.AddComponent<TextMeshPro>();
        
        // Setup the font and style to look professional
        tmp.text = $"<b>{objectDisplayName}</b>";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 0.2f; // Adjust this if the text is too big/small
        tmp.color = Color.white;
        
        // Add a dark outline so it's readable against the bright lab background
        tmp.fontSharedMaterial.EnableKeyword("OUTLINE_ON");
        tmp.outlineColor = new Color32(0, 0, 0, 255);
        tmp.outlineWidth = 0.2f;
    }

    private void DestroyFloatingText()
    {
        if (floatingTextObj != null)
        {
            Destroy(floatingTextObj);
        }
    }
}