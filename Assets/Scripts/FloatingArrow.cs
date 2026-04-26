using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0.3f, 0);
    public float floatSpeed = 2f;
    public float floatHeight = 0.02f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (target == null) return;

        // Follow target
        Vector3 basePosition = target.position + offset;

        // Floating animation
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = basePosition + new Vector3(0, yOffset, 0);

        // Always face downward
        // transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        transform.Rotate(0, 60f * Time.deltaTime, 0);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        target = null;
        gameObject.SetActive(false);
    }
}