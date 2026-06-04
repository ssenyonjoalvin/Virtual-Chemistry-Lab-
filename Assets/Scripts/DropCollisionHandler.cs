using UnityEngine;

public class DropCollisionHandler : MonoBehaviour
{
    public FlaskReaction targetFlask;

    public float volumePerDrop = 0.5f; // adjust this

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("BaseSolution"))
        {
            if (targetFlask != null)
            {
                targetFlask.AddLiquid(volumePerDrop);

                Debug.Log("Drop hit flask → +volume");
            }
        }
    }
}