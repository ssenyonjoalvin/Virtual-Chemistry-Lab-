using UnityEngine;
using UnityEngine.SceneManagement; // This line is required to switch scenes

public class SceneLoader : MonoBehaviour
{
    public void LoadLabScene()
    {
        // Replace "LaboratoryScene" with the EXACT name of your lab scene file
        SceneManager.LoadScene("LaboratoryScene");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}