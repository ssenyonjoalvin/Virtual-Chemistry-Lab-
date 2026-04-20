using UnityEngine;
using UnityEngine.InputSystem;

public class TestFlask : MonoBehaviour
{
    public FlaskReaction flask;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            flask.AddLiquid(10f);
        }
    }
}