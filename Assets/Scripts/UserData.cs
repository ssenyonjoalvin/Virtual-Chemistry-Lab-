using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;

    public string userName;
    public string accessToken; 
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