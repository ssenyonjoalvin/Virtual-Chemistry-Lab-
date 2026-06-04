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

    // Removed Update() to avoid double-counting volume. 
    // Particles now handle volume addition via OnParticleCollision in FlaskReaction.


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