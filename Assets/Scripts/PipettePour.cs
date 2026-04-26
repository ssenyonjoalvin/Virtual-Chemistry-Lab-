using UnityEngine;

public class PipettePour : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem dropParticles;
    public FlaskReaction targetFlask;
    public TitrationExperimentManager experimentManager;

    [Header("Settings")]
    public float pourRate = 5f;

    private bool isPouring = false;

    void Update()
    {
        if (isPouring && targetFlask != null)
        {
            targetFlask.AddLiquid(pourRate * Time.deltaTime);
        }
    }

    public void StartPouring()
    {
        isPouring = true;

        if (dropParticles != null && !dropParticles.isPlaying)
            dropParticles.Play();

        if (experimentManager != null)
            experimentManager.OnPouringStarted();

        Debug.Log("Started pouring");
    }

    public void StopPouring()
    {
        isPouring = false;

        if (dropParticles != null && dropParticles.isPlaying)
            dropParticles.Stop();

        Debug.Log("Stopped pouring");
    }
}