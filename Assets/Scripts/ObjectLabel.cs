using UnityEngine;
using TMPro;

public class ObjectLabel : MonoBehaviour
{
    public string labelText;
    public GameObject labelUI;

    void Start()
    {
        if (labelUI != null)
            labelUI.SetActive(false);
    }

    public void ShowLabel()
    {
        if (labelUI != null)
        {
            labelUI.SetActive(true);
            labelUI.GetComponentInChildren<TextMeshProUGUI>().text = labelText;
        }
    }

    public void HideLabel()
    {
        if (labelUI != null)
            labelUI.SetActive(false);
    }
}