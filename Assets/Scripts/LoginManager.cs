using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

// These classes MUST match the JSON structure exactly
[System.Serializable]
public class StudentInfo
{
    public string name;
    public string id;
    public string email;
}

[System.Serializable]
public class LoginResponse
{
    public StudentInfo student;
    public string accessToken;
    public string expiresAt;
}

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI statusText;
    private string apiKey = "vrsps_unity_api_keys_2026";

    public void OnLoginButtonClicked()
    {
        if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            statusText.text = "Please enter email and password.";
            return;
        }
        StartCoroutine(LoginRoutine(usernameField.text, passwordField.text));
    }

    IEnumerator LoginRoutine(string email, string password)
    {
        string url = "https://vrspsweb.vercel.app/api/unity/auth/login";
        
        // Using a simple JSON format
        string jsonData = "{\"email\":\"" + email + "\", \"password\":\"" + password + "\", \"apiKey\":\"vrsps_unity_api_keys_2026\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        statusText.text = "Logging in...";

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try 
            {
                // Parse the JSON
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

                // Safety Checks
                if (UserData.instance == null) {
                    statusText.text = "Error: UserData missing in scene!";
                    yield break;
                }

                if (response == null || response.student == null) {
                    statusText.text = "Error: Server response format invalid.";
                    yield break;
                }

                // Save to global data
                UserData.instance.userName = response.student.name;
                UserData.instance.accessToken = response.accessToken;
                UserData.instance.isLoggedIn = true;

                statusText.text = "Welcome, " + response.student.name;

                // Trigger UI Flow
                MenuFlowManager menu = FindObjectOfType<MenuFlowManager>();
                if (menu != null) {
                    menu.OnLoginSuccess();
                } else {
                    Debug.LogError("MenuFlowManager not found!");
                }
            }
            catch (System.Exception e)
            {
                statusText.text = "Error parsing data.";
                Debug.LogError(e.Message);
            }
        }
        else
        {
            statusText.text = "Login Failed: " + request.responseCode;
            Debug.LogError("Login error: " + request.error);
        }
        
        request.Dispose();
    }
}