using UnityEngine;
using UnityEngine.InputSystem;

public class Indicator : MonoBehaviour
{
    public FlaskReaction targetFlask;

    private bool used = false;

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame && !used)
        {
            targetFlask.AddIndicator();
            used = true;
        }
    }
}