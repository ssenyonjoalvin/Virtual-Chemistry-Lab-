using UnityEngine;

public class DropperIndicator : MonoBehaviour
{
    public FlaskReaction targetFlask;
    public Transform flaskTransform;

    public float activationDistance = 0.3f;

    private bool used = false;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, flaskTransform.position);

        if (!used && distance < activationDistance)
        {
            targetFlask.AddIndicator();
            used = true;

            Debug.Log("Indicator added using dropper");
        }
    }
}