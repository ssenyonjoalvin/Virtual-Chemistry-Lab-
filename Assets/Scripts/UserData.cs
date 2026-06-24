using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;

    public string userName;
    public string studentId; // Save this from login!
    public string accessToken; 
    public string sessionId; // Save this from session start
    public bool isLoggedIn = false;
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}