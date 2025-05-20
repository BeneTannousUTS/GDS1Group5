using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonConfirmListener : MonoBehaviour
{
    public string confirmSoundName = "UIConfirm";

    void Update()
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;

            if (current != null)
            {
                Button button = current.GetComponent<Button>();
                if (button != null)
                {
                    if (button.IsActive())
                    {
                        Debug.Log(button.name);
                        AudioManager.instance.PlaySoundEffect(confirmSoundName);
                    }
                }
            }
        }
    }
}
