using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOptions : MonoBehaviour
{
    private InGameOptionsManager inGameOptionsManager;
    private PlayerInput playerInput;

    void Start()
    {
        inGameOptionsManager = FindAnyObjectByType<InGameOptionsManager>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnOptionButtonPressed(InputAction.CallbackContext context)
    {        
        if (!context.performed) return;

        Debug.Log($"[Player {GetComponent<PlayerInput>().playerIndex}] Start triggered | phase: {context.phase} | map: {context.action.actionMap.name}");

        if (!inGameOptionsManager.IsOptionsOpen() || inGameOptionsManager.GetActiveInput() == playerInput)
        {
            inGameOptionsManager.ToggleOptionsMenu(playerInput);
        }
    }
}
