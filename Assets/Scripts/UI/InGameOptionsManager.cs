// AUTHOR: Zac
// Manages the options pop up

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class InGameOptionsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsMenu;
    private PlayerInput activeInput = null;
    private PlayerManager playerManager;
    InputSystemUIInputModule UIInputModule;
    [SerializeField]
    InputActionAsset defaultActions;
    [SerializeField]
    Button audioMenuButton;

    void Awake()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
    }

    public void ToggleOptionsMenu(PlayerInput playerInput)
    {
        if (optionsMenu.activeSelf)
        {
            if (playerInput == activeInput)
            {
                optionsMenu.SetActive(false);
            }

            PlayerData[] players = playerManager.GetPlayers();

            CardSelection cardSelection = FindAnyObjectByType<CardSelection>();

            if (cardSelection == null)
            {
                foreach (PlayerData playerData in players)
                {
                    if (!playerData.isJoined) break;

                    playerData.playerInput.SwitchCurrentActionMap("Gameplay");
                }

                UIInputModule.actionsAsset = defaultActions;
                UIInputModule.point = InputActionReference.Create(defaultActions.FindAction("UI/Point"));
                UIInputModule.leftClick = InputActionReference.Create(defaultActions.FindAction("UI/Click"));
            }
            else if (cardSelection.selectionState == SelectionState.ConfirmingTurn)
            {
                foreach (PlayerData playerData in players)
                {
                    if (!playerData.isJoined) break;

                    if (playerData.playerIndex == cardSelection.currentPlayerIndex)
                    {
                        playerData.playerInput.SwitchCurrentActionMap("Confirm/Skip");
                    }
                    else
                    {
                        playerData.playerInput.SwitchCurrentActionMap("Locked");
                    }
                }
            }
            else if (cardSelection.selectionState == SelectionState.Selecting)
            {
                foreach (PlayerData playerData in players)
                {
                    if (!playerData.isJoined) break;

                    if (playerData.playerIndex == cardSelection.currentPlayerIndex)
                    {
                        playerData.playerInput.SwitchCurrentActionMap("CardSelection");
                    }
                    else
                    {
                        playerData.playerInput.SwitchCurrentActionMap("Locked");
                    }

                    foreach (GameObject card in cardSelection.cardList)
                    {
                        if (card.GetComponent<Button>() != null)
                        {
                            card.GetComponent<Button>().Select();
                            card.GetComponent<CardHandler>().OnSelect(null);
                            break;
                        }
                    }
                }
            }
            else if (cardSelection.selectionState == SelectionState.Waiting)
            {
                foreach (PlayerData playerData in players)
                {
                    if (!playerData.isJoined) break;

                    playerData.playerInput.SwitchCurrentActionMap("Locked");
                }
            }
            else if (cardSelection.selectionState == SelectionState.ConfirmingSwap || cardSelection.selectionState == SelectionState.TraitorConfirming)
            {
                foreach (PlayerData playerData in players)
                {
                    if (!playerData.isJoined) break;

                    playerData.playerInput.SwitchCurrentActionMap("Confirm/Skip");
                }
            }



            Time.timeScale = 1;

        }
        else
        {
            // TO DO: ADD TEXT AT THE TOP OF THE SCREEN TO SHOW WHO IS THE CONTROLLING PLAYER

            Time.timeScale = 0;

            activeInput = playerInput;
            optionsMenu.SetActive(true);

            UIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
            UIInputModule.point = null;
            UIInputModule.leftClick = null;

            PlayerData[] players = playerManager.GetPlayers();

            foreach (PlayerData playerData in players)
            {
                if (!playerData.isJoined) break;

                playerData.playerInput.SwitchCurrentActionMap("Locked");
            }

            activeInput.SwitchCurrentActionMap("Menu");
            InputActionAsset playerActions = activeInput.actions;
            UIInputModule.actionsAsset = playerActions;
            UIInputModule.move = InputActionReference.Create(playerActions.FindAction("InGameOptions/Navigation"));
            UIInputModule.submit = InputActionReference.Create(playerActions.FindAction("InGameOptions/Select"));

            audioMenuButton.Select();
        }
    }
}
