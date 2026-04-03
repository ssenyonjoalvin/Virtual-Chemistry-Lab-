using UnityEngine;

public class BuretteSystem : MonoBehaviour
{
    public ParticleSystem dropParticles;
    public GameObject liquidInBurette;
    public bool isValveOpen = false;

    // Use this function for your Switch/Button On Click()
    public void ToggleValve()
    {
        isValveOpen = !isValveOpen;
        if (isValveOpen) dropParticles.Play();
        else dropParticles.Stop();
    }

    void Update()
    {
        if (isValveOpen && liquidInBurette.transform.localScale.y > 0)
        {
            // The liquid in the burette slowly goes down
            Vector3 scale = liquidInBurette.transform.localScale;
            scale.y -= Time.deltaTime * 0.05f; // Adjust speed here
            liquidInBurette.transform.localScale = scale;
        }
        else if (liquidInBurette.transform.localScale.y <= 0)
        {
            dropParticles.Stop(); // Stop if empty
        }
    }
}