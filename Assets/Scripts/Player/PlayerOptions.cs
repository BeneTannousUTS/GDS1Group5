using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOptions : MonoBehaviour
{
    private InGameOptionsManager inGameOptionsManager;
    void Start()
    {
        inGameOptionsManager = FindAnyObjectByType<InGameOptionsManager>();
    }
    public void OnOptionButtonPressed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        inGameOptionsManager.ToggleOptionsMenu(gameObject.GetComponent<PlayerInput>());
    }
}
