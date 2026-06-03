using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Trigger zone near the burner flame that starts the magnesium combustion reaction.
/// It only reacts when:
/// 1) The entering object is magnesium ribbon
/// 2) Burner is ON
/// 3) Experiment manager is waiting for reaction (Step 5)
/// </summary>
[RequireComponent(typeof(Collider))]
public class CombustionReaction : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private BunsenBurner bunsenBurner;
    [SerializeField] private CombustionExperimentManager experimentManager;

    [Header("Reaction Effects")]
    [Tooltip("Assign Effect_02.prefab here.")]
    [SerializeField] private GameObject effect02Prefab;

    [Tooltip("Assign Effect_04.prefab here.")]
    [SerializeField] private GameObject effect04Prefab;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cracklingClip;

    [Header("Mg White Flame Light")]
    [SerializeField] private float whiteLightPeakIntensity = 9f;
    [SerializeField] private float whiteLightRange = 2.5f;
    [SerializeField] private float reactionDurationSeconds = 3f;

    [Header("MgO Product")]
    [SerializeField] private float productScale = 0.05f;
    [SerializeField] private float labelHeight = 0.07f;

    private bool _hasReacted;
    private Collider _triggerCollider;

    private void Awake()
    {
        _triggerCollider = GetComponent<Collider>();
        _triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasReacted) return;

        CombustionMagnesiumRibbon magnesium = other.GetComponentInParent<CombustionMagnesiumRibbon>();
        if (magnesium == null) return;

        if (bunsenBurner == null || !bunsenBurner.IsOn) return;
        if (experimentManager == null || !experimentManager.IsWaitingForReaction()) return;

        _hasReacted = true;
        StartCoroutine(PlayReactionSequence(magnesium.gameObject));
    }

    private IEnumerator PlayReactionSequence(GameObject magnesiumObject)
    {
        Vector3 reactionPoint = magnesiumObject != null ? magnesiumObject.transform.position : transform.position;
        experimentManager.OnCombustionReactionStarted();

        // Spawn flash/explosion effect.
        if (effect02Prefab != null)
        {
            Instantiate(effect02Prefab, reactionPoint, Quaternion.identity);
        }

        // Spawn smoke/sparks effect.
        if (effect04Prefab != null)
        {
            Instantiate(effect04Prefab, reactionPoint, Quaternion.identity);
        }

        // Optional crackling sound.
        if (audioSource != null && cracklingClip != null)
        {
            audioSource.PlayOneShot(cracklingClip);
        }

        // Create temporary intense white light to mimic magnesium flame.
        GameObject lightObject = new GameObject("MgWhiteFlameLight");
        lightObject.transform.position = reactionPoint;
        Light mgLight = lightObject.AddComponent<Light>();
        mgLight.type = LightType.Point;
        mgLight.color = Color.white;
        mgLight.range = whiteLightRange;
        mgLight.intensity = whiteLightPeakIntensity;

        float elapsed = 0f;
        while (elapsed < reactionDurationSeconds)
        {
            elapsed += Time.deltaTime;
            float progress01 = Mathf.Clamp01(elapsed / reactionDurationSeconds);

            // Slight flicker + fade.
            float flicker = 0.88f + (0.12f * Mathf.Sin(elapsed * 55f));
            mgLight.intensity = Mathf.Lerp(whiteLightPeakIntensity, 0f, progress01) * flicker;

            // Live progress callback for UI.
            experimentManager.UpdateReactionProgress(progress01);
            yield return null;
        }

        Destroy(lightObject);

        // Remove magnesium source object after full reaction.
        if (magnesiumObject != null)
        {
            Destroy(magnesiumObject);
        }

        SpawnMgoProduct(reactionPoint);
        experimentManager.OnCombustionFinished();
    }

    private void SpawnMgoProduct(Vector3 position)
    {
        // Create simple white sphere as MgO product placeholder.
        GameObject product = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        product.name = "MgO_Product";
        product.transform.position = position;
        product.transform.localScale = Vector3.one * productScale;

        Renderer rendererComponent = product.GetComponent<Renderer>();
        if (rendererComponent != null)
        {
            // Uses currently active render pipeline default material.
            Material whiteMaterial = new Material(rendererComponent.sharedMaterial);
            whiteMaterial.color = Color.white;
            rendererComponent.material = whiteMaterial;
        }

        // Add a small world-space label above the product.
        GameObject labelRoot = new GameObject("MgO_Label");
        labelRoot.transform.SetParent(product.transform, false);
        labelRoot.transform.localPosition = Vector3.up * labelHeight;

        Canvas canvas = labelRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        RectTransform rect = canvas.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200f, 60f);
        labelRoot.transform.localScale = Vector3.one * 0.001f;

        TextMeshProUGUI labelText = labelRoot.AddComponent<TextMeshProUGUI>();
        labelText.text = "MgO";
        labelText.fontSize = 40f;
        labelText.color = Color.black;
        labelText.alignment = TextAlignmentOptions.Center;
    }
}
