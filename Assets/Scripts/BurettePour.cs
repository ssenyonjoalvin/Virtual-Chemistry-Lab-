using UnityEngine;

public class BurettePour : MonoBehaviour
{
    public FlaskReaction targetFlask;
    public ParticleSystem pourEffect;

    public float pourRate = 10f; // amount per second

    private bool isPouring = false;

 void Update()
    {
        if (transform.up.y < 0)
        {
            if (!pourEffect.isPlaying)
                pourEffect.Play();

            targetFlask.AddLiquid(pourRate * Time.deltaTime);
        }
        else
        {
            if (pourEffect.isPlaying)
                pourEffect.Stop();
        }
    }

    void CheckTilt()
    {
        float angle = transform.eulerAngles.x;

        // Detect tilt (adjust if needed)
        if (angle > 120f && angle < 240f)
        {
            if (!isPouring)
            {
                isPouring = true;
                Debug.Log("Started pouring");
            }
        }
        else
        {
            if (isPouring)
            {
                isPouring = false;
                Debug.Log("Stopped pouring");
            }
        }
    }
}