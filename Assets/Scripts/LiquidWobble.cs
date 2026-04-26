using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("The Renderer containing the liquid material")]
    public Renderer liquidRenderer;

    [Header("Wobble Settings")]
    [Tooltip("Maximum amount the liquid can tilt")]
    public float maxWobble = 0.05f;
    [Tooltip("How fast the liquid sloshes back and forth")]
    public float wobbleSpeed = 15f;
    [Tooltip("How quickly the liquid settles down (lower = settles slower)")]
    public float recoveryRate = 1.5f; 
    [Tooltip("Multiplier to convert world velocity into wobble amount")]
    public float velocityMultiplier = 0.1f;

    // Shader property IDs (more efficient than using strings every frame)
    private int rotXId;
    private int rotZId;

    // Internal tracking variables
    private Vector3 lastPos;
    private Vector3 lastRot;
    private Vector3 velocity;
    private Vector3 angularVelocity;

    // Current amplitudes of the wobble
    private float wobbleAmountX;
    private float wobbleAmountZ;

    // The continuously running time value for the sine wave
    private float time;

    void Start()
    {
        if (liquidRenderer == null)
            liquidRenderer = GetComponent<Renderer>();

        // Cache the shader properties
        rotXId = Shader.PropertyToID("_RotX");
        rotZId = Shader.PropertyToID("_RotZ");

        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }

    void Update()
    {
        // 1. Calculate how fast the object is moving and rotating
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;

        // 2. Add the movement/rotation to the current wobble amount
        // We use clamp to ensure you can't shake it so hard the liquid breaks the shader
        wobbleAmountX += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * velocityMultiplier, -maxWobble, maxWobble);
        wobbleAmountZ += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * velocityMultiplier, -maxWobble, maxWobble);

        // 3. Keep the overall wobble within limits
        wobbleAmountX = Mathf.Clamp(wobbleAmountX, -maxWobble, maxWobble);
        wobbleAmountZ = Mathf.Clamp(wobbleAmountZ, -maxWobble, maxWobble);

        // 4. Smoothly decay the wobble back to 0 over time
        // A recovery rate of ~1.5 to 2.0 with Lerp naturally settles completely in about 2 seconds
        wobbleAmountX = Mathf.Lerp(wobbleAmountX, 0, Time.deltaTime * recoveryRate);
        wobbleAmountZ = Mathf.Lerp(wobbleAmountZ, 0, Time.deltaTime * recoveryRate);

        // 5. Calculate the sine wave bounce
        time += Time.deltaTime;
        float currentWobbleX = wobbleAmountX * Mathf.Sin(wobbleSpeed * time);
        float currentWobbleZ = wobbleAmountZ * Mathf.Sin(wobbleSpeed * time);

        // 6. Send the final values to the Material Shader
        if (liquidRenderer != null && liquidRenderer.material != null)
        {
            liquidRenderer.material.SetFloat(rotXId, currentWobbleX);
            liquidRenderer.material.SetFloat(rotZId, currentWobbleZ);
        }

        // 7. Store current position/rotation for the next frame's math
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }
}