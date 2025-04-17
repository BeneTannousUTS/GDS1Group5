using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyCanvasManager : MonoBehaviour
{
    [SerializeField]
    GameObject readyPlayerPrefab;
    [SerializeField]
    Sprite[] playerSprites = new Sprite[4];
    List<ReadyCheckHandler> readyCheckHandlers = new List<ReadyCheckHandler>();

    public void StartReadyCheck(PlayerData[] players)
    {
        StartCoroutine(ReadyCheckSequence(players));
    }

    IEnumerator ReadyCheckSequence(PlayerData[] players)
    {
        SetPlayerPrefabs(players);
        yield return WaitForAllConfirmations(readyCheckHandlers);

        yield return new WaitForSeconds(0.25f);

        transform.parent.gameObject.GetComponent<TraitorCanvasManager>().DestroyTraitorCanvas();
    }

    public void SetPlayerPrefabs(PlayerData[] players)
    {
        foreach (PlayerData playerData in players)
        {
            if (!playerData.isJoined) continue;

            GameObject readyCheckObject = Instantiate(readyPlayerPrefab, transform);

            readyCheckObject.GetComponent<ReadyCheckHandler>().playerIcon.sprite = playerSprites[playerData.playerIndex];
            readyCheckObject.GetComponent<ReadyCheckHandler>().assignedInput = playerData.playerInput;
            readyCheckObject.GetComponent<ReadyCheckHandler>().playerIndex = playerData.playerIndex;

            readyCheckHandlers.Add(readyCheckObject.GetComponent<ReadyCheckHandler>());
        }
    }

    IEnumerator WaitForAllConfirmations(List<ReadyCheckHandler> handlers)
    {
        List<Coroutine> activeCoroutines = new();

        foreach (var handler in handlers)
        {
            if (handler.assignedInput == null) continue; // skip players with no input
            Debug.Log($"All Handlers: Registered handler to p{handler.playerIndex}");
            activeCoroutines.Add(StartCoroutine(WaitForConfirm(handler)));
        }

        while (!handlers.Where(h => h.assignedInput != null).All(h => h.hasConfirmed))
            yield return null;
    }

    IEnumerator WaitForConfirm(ReadyCheckHandler handler)
    {
        InputAction confirmAction = handler.assignedInput.actions.FindAction("ConfirmButton");
        InputAction skipAction = handler.assignedInput.actions.FindAction("SkipButton");

        while (!handler.hasConfirmed)
        {
            if (confirmAction != null && confirmAction.WasPressedThisFrame())
            {
                handler.hasConfirmed = true;
                handler.toggle.isOn = true;
            }

            yield return null;
        }
    }
}
