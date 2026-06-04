using UnityEngine;

public class GazeDetector : MonoBehaviour
{
    public float distance = 10f;

    private ObjectLabel currentLabel;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            ObjectLabel label = hit.collider.GetComponent<ObjectLabel>();

            if (label != null)
            {
                if (currentLabel != label)
                {
                    if (currentLabel != null)
                        currentLabel.HideLabel();

                    currentLabel = label;
                    currentLabel.ShowLabel();
                }
            }
        }
        else
        {
            if (currentLabel != null)
            {
                currentLabel.HideLabel();
                currentLabel = null;
            }
        }
    }
}