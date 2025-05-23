// AUTHOR: Zac
// Manages the options pop up

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class InGameOptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private InputActionAsset defaultActions;
    [SerializeField] private Button audioMenuButton;
    [SerializeField] private TMP_Text playerNameText;

    private PlayerInput activeInput = null;
    private PlayerManager playerManager;
    private InputSystemUIInputModule UIInputModule;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
    }

    public void ToggleOptionsMenu(PlayerInput playerInput)
    {
        if (optionsMenu.activeSelf)
        {
            if (playerInput != activeInput) return;

            if (optionsMenu.GetComponent<OptionsMenuManager>().GetIsConfirming()) return;

            Debug.Log("Closing Menu");
            optionsMenu.SetActive(false);
            Time.timeScale = 1;

            ResetActionMapsAfterClosing();
            activeInput = null;
        }
        else
        {
            Debug.Log("Opening Menu");

            EventSystem.current.SetSelectedGameObject(null);
            int playerIndex = playerInput.GetComponent<PlayerIndex>().playerIndex;
            playerNameText.text = $"Player {playerIndex + 1} is in control...";
            playerNameText.color = FindAnyObjectByType<PlayerManager>().players[playerIndex].playerColour;

            Time.timeScale = 0;
            optionsMenu.SetActive(true);
            OptionsMenuManager optionsMenuManager = optionsMenu.GetComponent<OptionsMenuManager>();
            optionsMenuManager.SetOptions();
            optionsMenuManager.LoadAudio();
            activeInput = playerInput;

            // Lock all players
            foreach (PlayerData playerData in playerManager.GetPlayers())
            {
                if (!playerData.isJoined) break;
                playerData.playerInput.SwitchCurrentActionMap("Locked");
            }

            activeInput.SwitchCurrentActionMap("Menu");

            // Update UI module
            UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
            UIInputModule.actionsAsset = activeInput.actions;
            UIInputModule.move = InputActionReference.Create(activeInput.actions.FindAction("Menu/Navigation"));
            UIInputModule.submit = InputActionReference.Create(activeInput.actions.FindAction("Menu/Select"));
            UIInputModule.point = null;
            UIInputModule.leftClick = null;

            audioMenuButton.Select();
        }
    }

    private void ResetActionMapsAfterClosing()
    {
        CardSelection cardSelection = FindAnyObjectByType<CardSelection>();
        PlayerData[] players = playerManager.GetPlayers();

        if (cardSelection == null)
        {
            foreach (PlayerData playerData in players)
            {
                if (!playerData.isJoined) break;
                playerData.playerInput.SwitchCurrentActionMap("Gameplay");
            }

            RestoreDefaultUIInputModule();
        }
        else
        {
            foreach (PlayerData playerData in players)
            {
                if (!playerData.isJoined) break;

                int currentIndex = cardSelection.currentPlayerIndex;
                var state = cardSelection.selectionState;

                switch (state)
                {
                    case SelectionState.ConfirmingTurn:
                        playerData.playerInput.SwitchCurrentActionMap(playerData.playerIndex == currentIndex ? "Confirm/Skip" : "Locked");
                        break;
                    case SelectionState.ConfirmingSwap:
                    case SelectionState.TraitorConfirming:
                        playerData.playerInput.SwitchCurrentActionMap("Confirm/Skip");
                        break;
                    case SelectionState.Selecting:
                        playerData.playerInput.SwitchCurrentActionMap(playerData.playerIndex == currentIndex ? "CardSelection" : "Locked");

                        if (playerData.playerIndex == currentIndex)
                        {
                            foreach (GameObject card in cardSelection.cardList)
                            {
                                if (card.TryGetComponent(out Button btn))
                                {
                                    btn.Select();
                                    card.GetComponent<CardHandler>().OnSelect(null);
                                    break;
                                }
                            }
                        }
                        break;

                    case SelectionState.Waiting:
                        playerData.playerInput.SwitchCurrentActionMap("Locked");
                        break;
                }
            }
            RestoreCardUIInputModule();
        }
    }

    private void RestoreDefaultUIInputModule()
    {
        if (UIInputModule == null) return;

        UIInputModule.actionsAsset = defaultActions;
        UIInputModule.point = InputActionReference.Create(defaultActions.FindAction("UI/Point"));
        UIInputModule.leftClick = InputActionReference.Create(defaultActions.FindAction("UI/Click"));
        UIInputModule.move = null;
        UIInputModule.submit = null;
    }

    private void RestoreCardUIInputModule()
    {
        PlayerData[] players = PlayerManager.instance.GetPlayers();

        int currentPlayerIndex = FindAnyObjectByType<CardSelection>().currentPlayerIndex;

        if (currentPlayerIndex == -1) return;

        InputActionAsset playerActions = players[currentPlayerIndex].playerInput.actions;
        UIInputModule.actionsAsset = playerActions;
        UIInputModule.point = null;
        UIInputModule.leftClick = null;
        UIInputModule.move = InputActionReference.Create(playerActions.FindAction("CardSelection/CardNav"));
        UIInputModule.submit = InputActionReference.Create(playerActions.FindAction("CardSelection/CardSelect"));
    }

    public bool IsOptionsOpen() => optionsMenu.activeSelf;

    public PlayerInput GetActiveInput() => activeInput;
}