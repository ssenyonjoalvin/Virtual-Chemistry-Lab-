using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;


[System.Serializable]
public class SessionResponse 
{ 
    public string sessionId; 
}
public class SessionManager : MonoBehaviour
{
    private string apiKey = "vrsps_unity_api_keys_2026";
    private string experimentId = "titration-001";
    private float startTime;
    private bool isSessionActive = false;
private bool isRequestInProgress = false;
   
    public void BeginSession()
    {
        if (isSessionActive) return; 
        
        startTime = Time.time;
        StartCoroutine(StartSession());
    }

    IEnumerator StartSession()
    {
        string url = "https://vrspsweb.vercel.app/api/unity/session/start";
        string jsonData = $"{{\"studentId\":\"{UserData.instance.studentId}\", \"experimentId\":\"{experimentId}\"}}";

        if (isRequestInProgress) yield break; // If a request is already running, stop!
        isRequestInProgress = true;

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-API-KEY", apiKey);

         yield return request.SendWebRequest();

        isRequestInProgress = false;
        if (request.result == UnityWebRequest.Result.Success)
        {
            SessionResponse response = JsonUtility.FromJson<SessionResponse>(request.downloadHandler.text);
            UserData.instance.sessionId = response.sessionId;
            isSessionActive = true;
            Debug.Log("Session Started: " + UserData.instance.sessionId);
        }
    }

    // 🔴 CALL THIS FROM YOUR FINISH/RESET BUTTON
    public void FinishSession(int wrongSteps, bool passed)
    {
        if (!isSessionActive) return; // Don't end if it never started
        StartCoroutine(EndSessionRoutine(wrongSteps, passed));
    }

    IEnumerator EndSessionRoutine(int wrongSteps, bool passed)
    {
        string url = "https://vrspsweb.vercel.app/api/unity/session/end";
        float timeTaken = Time.time - startTime;
        
        string jsonData = $"{{\"sessionId\":\"{UserData.instance.sessionId}\", \"timeTaken\":{timeTaken}, \"wrongSteps\":{wrongSteps}, \"passed\":{passed.ToString().ToLower()}}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-API-KEY", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Session Ended Successfully");
            isSessionActive = false;
        }
    }

    public void FinishSessionDefault()
    {
        // You can hardcode your default values here
        FinishSession(0, true); 
    }

    // If you ever need to trigger a 'failed' version from code:
    public void FinishSessionFailed()
    {
        FinishSession(1, false);
    }
}