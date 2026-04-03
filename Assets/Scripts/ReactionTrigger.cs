using UnityEngine;

public class ReactionZone : MonoBehaviour
{
    public BuretteSystem burette; // Drag your burette here

    void OnTriggerStay(Collider other)
    {
        // If the beaker is under the burette AND the valve is open
        if (other.CompareTag("Beaker") && burette.isValveOpen)
        {
            // Find the liquid inside the beaker and change its color!
            MeshRenderer beakerLiquid = other.transform.Find("Liquid").GetComponent<MeshRenderer>();
            beakerLiquid.material.color = Color.Lerp(beakerLiquid.material.color, Color.magenta, Time.deltaTime * 0.2f);
        }
    }
}