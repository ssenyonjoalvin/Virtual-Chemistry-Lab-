using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class ShowKeyBoard : MonoBehaviour

{
    private TMP_InputField inputField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        if (inputField != null)
        {
            inputField.onSelect.AddListener(x =>OpenKeyboard());
        }
        
    }

   public void OpenKeyboard()
{
    // Fix: Use capital 'I' for Instance
    NonNativeKeyboard.Instance.InputField = inputField;
    NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);
}
}
