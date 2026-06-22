using UnityEngine;
using TMPro;

public class SessionTimer : MonoBehaviour
{
    [Header("Settings")]
    public float maxSessionMinutes = 20f; // Limit to 20 minutes
    public GameObject warningPanel;      // Drag a UI panel here that says "Time for a break!"

    private float timer = 0f;
    private bool warningShown = false;

    void Update()
    {
        timer += Time.deltaTime;

        // Check if time is up
        if (timer >= maxSessionMinutes * 60f && !warningShown)
        {
            ShowSafetyWarning();
        }
    }

    void ShowSafetyWarning()
    {
        warningShown = true;
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
        }
        Debug.Log("Session limit reached: Please take a break.");
    }
}