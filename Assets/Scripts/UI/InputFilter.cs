using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputFilter : InputSystemUIInputModule
{
    // Set this to the device you want to allow UI navigation.
    public static InputDevice ActiveDevice;

    public override void Process()
    {
        // If no active device is set, process normally.
        if (ActiveDevice == null)
        {
            base.Process();
            return;
        }

        bool processInput = false;

        // Check if the move action is being used by the active device.
        if (move != null && move.action != null)
        {
            Vector2 moveValue = move.action.ReadValue<Vector2>();
            if (moveValue.sqrMagnitude > 0.01f && move.action.activeControl != null)
            {
                processInput = move.action.activeControl.device == ActiveDevice;
            }
        }

        // Check submit input.
        if (!processInput && submit != null && submit.action != null)
        {
            float submitValue = submit.action.ReadValue<float>();
            if (submitValue > 0.5f && submit.action.activeControl != null)
            {
                processInput = submit.action.activeControl.device == ActiveDevice;
            }
        }

        // Check cancel input.
        if (!processInput && cancel != null && cancel.action != null)
        {
            float cancelValue = cancel.action.ReadValue<float>();
            if (cancelValue > 0.5f && cancel.action.activeControl != null)
            {
                processInput = cancel.action.activeControl.device == ActiveDevice;
            }
        }

        // Only process UI navigation if input is coming from the active device.
        if (processInput)
        {
            base.Process();
        }
    }
}