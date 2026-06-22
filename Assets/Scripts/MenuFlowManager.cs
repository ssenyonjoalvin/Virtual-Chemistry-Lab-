using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFlowManager : MonoBehaviour
{
    public GameObject loginPanel;      // The Login screen
    public GameObject experimentButtons; // The Guided/Sandbox buttons

    void Start()
    {
        // Check if the user is already logged in (using our UserData instance)
        if (UserData.instance != null && UserData.instance.isLoggedIn)
        {
            ShowMainMenu();
        }
        else
        {
            ShowLogin();
        }
    }

    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        experimentButtons.SetActive(false);
    }

    public void ShowMainMenu()
    {
        loginPanel.SetActive(false);
        experimentButtons.SetActive(true);
    }

    // This is called by your Login button after the API call succeeds
    public void OnLoginSuccess()
    {
        ShowMainMenu();
    }
}