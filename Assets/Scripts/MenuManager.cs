using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject experimentPanel;

    public void ShowExperimentMenu()
    {
        mainMenuPanel.SetActive(false);
        experimentPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        experimentPanel.SetActive(false);
    }

    public void LoadFreeMode()
    {
        SceneManager.LoadScene("LaboratoryScene"); // Make sure your scene name matches!
    }

    public void LoadExperiment(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
