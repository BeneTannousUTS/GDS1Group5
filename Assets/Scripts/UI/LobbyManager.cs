// AUTHOR: BENEDICT
// This script handles player joining and logic to update the UI as players join and leave


using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    
    [System.Serializable]
    public class PlayerSlot
    {
        public GameObject slotPanel;
        public GameObject unjoinedIndicator;
        public GameObject joinedIndicator;
        public bool isOccupied = false;
        internal Gamepad panelGamepad;
    }

    [SerializeField]
    public GameObject pressStartPrefab;
    
    public PlayerSlot[] playerSlots;
    private int joinedPlayers = 0;
    private int maxPlayers = 4;
    private bool canStartGame = false;

 void Update()
    {
        // Loop through all connected gamepads
        foreach (Gamepad gamepad in Gamepad.all)
        {
            // Check if the player wants to join
            if (gamepad.buttonEast.wasPressedThisFrame && !IsGamepadAssigned(gamepad))
            {
                OnControlsChanged(gamepad);
                AssignGamepadSlot(gamepad);
            }
            // Check if the player wants to leave
            else if (gamepad.buttonSouth.wasPressedThisFrame && IsGamepadAssigned(gamepad))
            {
                OnControlsChanged(gamepad);
                UnassignGamepadSlot(gamepad);
            }
            // Check if the first player wants to start the game
            else if (gamepad.startButton.wasPressedThisFrame && gamepad == playerSlots[0].panelGamepad)
            {
                StartGame();
            }
        }

        if (playerSlots[0].isOccupied && (playerSlots[1].isOccupied || playerSlots[2].isOccupied || playerSlots[3].isOccupied))
        {
            pressStartPrefab.SetActive(true);
            canStartGame = true;
        }
        else
        {
            pressStartPrefab.SetActive(false);
            canStartGame = false;
        }
    }

    // Assign the gamepad to the first available slot
    void AssignGamepadSlot(Gamepad gamepad)
    {
        // Check if we’ve reached the max number of players
        if (joinedPlayers >= maxPlayers)
        {
            Debug.Log("Max players reached. Cannot join more players.");
            return;
        }

        foreach (var slot in playerSlots)
        {
            if (!slot.isOccupied)
            {
                // Update slot state
                slot.unjoinedIndicator.SetActive(false);
                slot.joinedIndicator.SetActive(true);
                slot.isOccupied = true;
                slot.panelGamepad = gamepad;

                joinedPlayers++;
                PlayerManager.instance.JoinPlayer(gamepad);

                Debug.Log($"Gamepad {gamepad.deviceId} assigned to slot.");
                break;
            }
        }
    }

    // Unassign the gamepad from its current slot
    void UnassignGamepadSlot(Gamepad gamepad)
    {
        foreach (var slot in playerSlots)
        {
            if (slot.panelGamepad == gamepad)
            {
                // Update slot state
                slot.unjoinedIndicator.SetActive(true);
                slot.joinedIndicator.SetActive(false);
                slot.isOccupied = false;
                slot.panelGamepad = null;

                joinedPlayers--;
                PlayerManager.instance.UnjoinPlayer(gamepad);

                Debug.Log($"Gamepad {gamepad.deviceId} unassigned from slot.");
                break;
            }
        }
    }

    // Check if this gamepad has already been assigned to another slot
    bool IsGamepadAssigned(Gamepad gamepad)
    {
        foreach (var slot in playerSlots)
        {
            if (slot.panelGamepad == gamepad)
            {
                return true;
            }
        }
        return false;
    }

    void StartGame()
    {
        if (!canStartGame) return;
        
        SceneManager.LoadScene("GameScene");
    }

    //Disable XInput device since Unity has a bug where Switch pro controllers are recognised as two inputs
    private void OnControlsChanged(Gamepad gamepad)
    {
        if (gamepad is UnityEngine.InputSystem.Switch.SwitchProControllerHID)
        {
            foreach (var item in Gamepad.all)
            {
                if ((item is UnityEngine.InputSystem.XInput.XInputController) && (Math.Abs(item.lastUpdateTime - gamepad.lastUpdateTime) < 0.1))
                {
                    Debug.Log($"Switch Pro controller detected and a copy of XInput was active at almost the same time. Disabling XInput device. `{gamepad}`; `{item}`");
                    InputSystem.DisableDevice(item);
                }
            }
        }
    }
}