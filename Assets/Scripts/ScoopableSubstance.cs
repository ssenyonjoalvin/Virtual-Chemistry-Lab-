using UnityEngine;

public class ScoopableSubstance : MonoBehaviour
{
    [Header("Substance")]
    public string substanceId = "SubstanceA";
    public Material sampleMaterial;

    [Tooltip("-1 = infinite scoops.")]
    public int availableScoops = -1;

    public bool TryConsumeOne()
    {
        if (availableScoops == 0) return false;
        if (availableScoops > 0) availableScoops--;
        return true;
    }
}
