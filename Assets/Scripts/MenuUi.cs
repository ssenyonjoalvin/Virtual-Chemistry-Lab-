using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MenuUi : MonoBehaviour
{
    public InputActionAsset menuInputActionAsset; 
    public GameObject menuPanel; // 🔴 DRAG YOUR PANEL HERE IN INSPECTOR
    [Header("UI Reference")]
    public TextMeshProUGUI userNameText; 
    
    private InputAction toggleMenuAction; 

    void Start()
    {
        Debug.Log("Menu script started");

        // Ensure the panel starts in the state you want
        if (menuPanel != null) menuPanel.SetActive(false);

        // Find the map - Make sure this name matches your Input Action Asset exactly!
        var map = menuInputActionAsset.FindActionMap("XRI Left"); 

        if (userNameText != null)
        {
            userNameText.text = UserData.instance.userName;
        }

        if (map != null)
        {
            toggleMenuAction = map.FindAction("Menu");
            if (toggleMenuAction != null)
            {
                toggleMenuAction.Enable();
                toggleMenuAction.performed += ToggleMenu;
            }
            else
            {
                Debug.LogError("Could not find action 'Menu' in map 'XRI LeftHand'");
            }
        }
        else
        {
            Debug.LogError("Could not find action map 'XRI LeftHand'");
        }
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("MENU BUTTON PRESSED");

        if (menuPanel != null)
        {
            // Toggle the Panel's visibility
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);
        }
    }

    private void OnDestroy()
    {
        if (toggleMenuAction != null)
            toggleMenuAction.performed -= ToggleMenu;
    }
}