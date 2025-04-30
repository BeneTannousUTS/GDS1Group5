// AUTHOR: BENEDICT
// This script handles player joining and logic to update the UI as players join and leave


using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] players;
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
            else if (gamepad.startButton.wasPressedThisFrame && gamepad == players[0].GetComponent<PlayerInput>().GetDevice<Gamepad>())
            {
                StartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (PlayerManager.instance.players[0].isJoined)
            {
                AssignGamepadSlot(PlayerManager.instance.players[0].gamepad);
            }
        }

        if (players[0].GetComponent<PlayerIndex>().isOccupied && (players[1].GetComponent<PlayerIndex>().isOccupied || players[2].GetComponent<PlayerIndex>().isOccupied || players[3].GetComponent<PlayerIndex>().isOccupied))
        {
            canStartGame = true;
        }
        else
        {
            canStartGame = false;
        }
    }

    void CheckControllersForJoinInput()
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
            else if (gamepad.startButton.wasPressedThisFrame && gamepad == players[0].GetComponent<PlayerInput>().GetDevice<Gamepad>())
            {
                StartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (PlayerManager.instance.players[0].isJoined)
            {
                AssignGamepadSlot(PlayerManager.instance.players[0].gamepad);
            }
        }

        if (players[0].GetComponent<PlayerIndex>().isOccupied && (players[1].GetComponent<PlayerIndex>().isOccupied || players[2].GetComponent<PlayerIndex>().isOccupied || players[3].GetComponent<PlayerIndex>().isOccupied))
        {
            canStartGame = true;
        }
        else
        {
            canStartGame = false;
        }
    }

    // Assign the gamepad to the first available player
    void AssignGamepadSlot(Gamepad gamepad)
    {
        // Check if we’ve reached the max number of players
        if (joinedPlayers >= maxPlayers)
        {
            Debug.Log("Max players reached. Cannot join more players.");
            return;
        }

        foreach (var playerPrefab in players)
        {
            if (!playerPrefab.GetComponent<PlayerIndex>().isOccupied)
            {
                // Update slot state
                var playerUser = InputUser.PerformPairingWithDevice(gamepad);;
                playerPrefab.GetComponent<PlayerIndex>().isOccupied = true;
                InputUser.PerformPairingWithDevice(gamepad);
                
                //playerPrefab.GetComponent<PlayerInput>(). = playerUser;

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
        foreach (var playerPrefab in players)
        {
            if (playerPrefab.GetComponent<PlayerInput>().GetDevice<Gamepad>() == gamepad)
            {
                // Update slot state
                playerPrefab.GetComponent<PlayerIndex>().isOccupied = false;
                //playerPrefab.panelGamepad = null;

                joinedPlayers--;
                PlayerManager.instance.UnjoinPlayer(gamepad);

                Debug.Log($"Gamepad {gamepad.deviceId} unassigned from player " + (playerPrefab.GetComponent<PlayerIndex>().playerIndex+1));
                break;
            }
        }
    }

    // Check if this gamepad has already been assigned to another slot
    bool IsGamepadAssigned(Gamepad gamepad)
    {
        foreach (var playerPrefab in players)
        {
            if (playerPrefab.GetComponent<PlayerInput>().GetDevice<Gamepad>() == gamepad)
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