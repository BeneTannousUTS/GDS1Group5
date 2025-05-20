// AUTHOR: Zac
// A permanent instance to manage all of the UI confirming systems

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConfirmManager : MonoBehaviour
{
    private static ConfirmManager instance;
    private BaseConfirmHandler priorityHandler = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public static ConfirmManager Instance => instance;

    public IEnumerator WaitForAllConfirmations(List<BaseConfirmHandler> allHandlers)
    {
        List<Coroutine> activeCoroutines = new();

        foreach (BaseConfirmHandler handler in allHandlers)
        {
            if (handler.assignedInput == null) continue;
            Debug.Log($"Registered Handler for Player {handler.playerIndex}");
            activeCoroutines.Add(StartCoroutine(WaitForConfirm(handler)));
        }

        while (!allHandlers.Where(h => h.assignedInput != null).All(h => h.hasConfirmed))
            yield return null;
    }

    private IEnumerator WaitForConfirm(BaseConfirmHandler handler)
    {
        handler.assignedInput.SwitchCurrentActionMap("Confirm/Skip");
        InputAction confirmAction = handler.assignedInput.actions.FindAction("ConfirmButton");
        InputAction skipAction = handler.assignedInput.actions.FindAction("SkipButton");

        if (confirmAction != null)
        {
            while (confirmAction.IsPressed())
            {
                yield return null;
            }
        }

        Debug.Log($"Waiting for Handler p{handler.playerIndex}");
        float timePassed = 0f;

        while (!handler.hasConfirmed)
        {
            timePassed += Time.deltaTime;

            if (priorityHandler != null && priorityHandler != handler)
            {
                yield return null;
                continue;
            }

            if (confirmAction != null && confirmAction.WasPressedThisFrame())
            {
                handler.confirmedChoice = true;
                handler.hasConfirmed = true;
                Debug.Log($"Handler p{handler.playerIndex} YES");
                AudioManager.instance.PlaySoundEffect("UIConfirm");

                if (handler is ConfirmCardHandler confirmCardHandler)
                {
                    confirmCardHandler.yesText.color = Color.green;
                    confirmCardHandler.noText.color = Color.black;
                }

                if (handler is ReadyCheckHandler readyCheckHandler)
                {
                    readyCheckHandler.toggle.isOn = true;
                }
            }
            else if (skipAction != null && skipAction.WasPressedThisFrame())
            {
                handler.confirmedChoice = false;
                handler.hasConfirmed = true;
                Debug.Log($"Handler p{handler.playerIndex} NO");
                AudioManager.instance.PlaySoundEffect("UIReject");

                if (handler is ConfirmCardHandler confirmCardHandler)
                {
                    confirmCardHandler.yesText.color = Color.black;
                    confirmCardHandler.noText.color = Color.red;
                }

                if (handler is ReadyCheckHandler readyCheckHandler)
                {
                    readyCheckHandler.toggle.isOn = true;
                }
            }
            else if (timePassed >= 5f && !handler.hasConfirmed)
            {
                timePassed = 0;
                handler.ShakeCard();
            }

            yield return null;
        }
    }

    public void SetPriorityHandler(BaseConfirmHandler handler)
    {
        priorityHandler = handler;
    }
}