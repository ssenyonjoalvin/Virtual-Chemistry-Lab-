using UnityEngine;
using System.Collections;

public class DropperController : MonoBehaviour
{
    [Header("Visual References")]
    [Tooltip("Drag the FillZone child object here")]
    public Transform fillZoneTransform;
    [Tooltip("Drag the FillZone's MeshRenderer here")]
    public MeshRenderer fillZoneRenderer;

    [Header("Settings")]
    public float fillSpeed = 2f;
    public Vector3 emptyScale = new Vector3(1, 0, 1); // Adjust X and Z based on your model
    public Vector3 fullScale = new Vector3(1, 1, 1);  // Adjust X and Z based on your model

    private bool isFull = false;
    private Coroutine fillRoutine;

    void Start()
    {
        // Ensure it starts empty
        if (fillZoneTransform != null)
        {
            fillZoneTransform.localScale = emptyScale;
            fillZoneRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we touched is tagged as Liquid AND we aren't already full
        if (other.CompareTag("Liquid") && !isFull)
        {
            // Try to get the LiquidSource script to know what color/material to use
            LiquidSource source = other.GetComponent<LiquidSource>();
            
            if (source != null)
            {
                FillDropper(source.liquidMaterial);
            }
        }
    }

    public void FillDropper(Material liquidMat)
    {
        if (isFull) return;

        // 1. Change the material of the inside liquid
        fillZoneRenderer.material = liquidMat;
        fillZoneRenderer.enabled = true;

        // 2. Start the filling animation
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(ScaleOverTime(emptyScale, fullScale, fillSpeed));
        
        isFull = true;
    }

    // Coroutine to make the liquid visually rise smoothly
    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float speed)
    {
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            fillZoneTransform.localScale = Vector3.Lerp(startScale, endScale, progress);
            yield return null;
        }
        fillZoneTransform.localScale = endScale;
    }
}