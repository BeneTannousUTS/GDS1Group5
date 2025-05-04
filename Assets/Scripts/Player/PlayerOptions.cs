using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOptions : MonoBehaviour
{
    private PlayerInput playerInput;

    void Start()
    {
        
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnOptionButtonPressed(InputAction.CallbackContext context)
    {        
        if (!context.performed) return;

        Debug.Log($"[Player {GetComponent<PlayerInput>().playerIndex}] Start triggered | phase: {context.phase} | map: {context.action.actionMap.name}");

        InGameOptionsManager inGameOptionsManager = FindAnyObjectByType<InGameOptionsManager>();
        if (inGameOptionsManager == null) return;

        if (!inGameOptionsManager.IsOptionsOpen() || inGameOptionsManager.GetActiveInput() == playerInput)
        {
            inGameOptionsManager.ToggleOptionsMenu(playerInput);
        }
    }
}
