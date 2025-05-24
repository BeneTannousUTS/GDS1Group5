using UnityEngine;
using UnityEngine.InputSystem;

public class StopVibrations : MonoBehaviour
{
    void Awake()
    {
        TurnOffVibrations();
    }

    void TurnOffVibrations()
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            gamepad.SetMotorSpeeds(0, 0);
        }
    }
}
