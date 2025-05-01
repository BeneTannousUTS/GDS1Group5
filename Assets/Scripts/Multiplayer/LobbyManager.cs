// AUTHOR: BENEDICT
// This script handles player joining and logic to update the UI as players join and leave

//TODO: REFACTOR THIS SCRIPT SO THAT PLAYER HUD ELEMENTS ARE LOADED IN THE LOBBY AND ARE NOT DESTROYED WHEN LOADING A NEW SCENE

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] playerSpawns;
    public Color[] playerColours;
    public AnimatorOverrideController[] playerAnimators;
    
    private int joinedPlayers = 0;
    private int maxPlayers = 4;
    private bool canStartGame = false;

 void Update()
 {
     CheckControllerInput();
 }

    void CheckControllerInput()
    {
        // Loop through all connected gamepads
        foreach (Gamepad gamepad in Gamepad.all)
        {
            // Check if the player wants to join
            if (gamepad.buttonEast.wasPressedThisFrame && !IsGamepadAssigned(gamepad))
            {
                OnControlsChanged(gamepad);
                SpawnLobbyPlayer(gamepad);
            }
            // Check if the player wants to leave
            else if (gamepad.buttonSouth.wasPressedThisFrame && IsGamepadAssigned(gamepad))
            {
                OnControlsChanged(gamepad);
                UnassignGamepadSlot(gamepad);
            }
            // Check if the first player wants to start the game
            else if (gamepad.startButton.wasPressedThisFrame && gamepad == playerSpawns[0].GetComponent<PlayerInput>().GetDevice<Gamepad>())
            {
                StartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (PlayerManager.instance.players[0].isJoined)
            {
                SpawnLobbyPlayer(PlayerManager.instance.players[0].gamepad);
            }
        }

        if (playerSpawns[0].GetComponent<PlayerIndex>().isOccupied && (playerSpawns[1].GetComponent<PlayerIndex>().isOccupied || playerSpawns[2].GetComponent<PlayerIndex>().isOccupied || playerSpawns[3].GetComponent<PlayerIndex>().isOccupied))
        {
            canStartGame = true;
        }
        else
        {
            canStartGame = false;
        }
    }

    // Assign the gamepad to the first available player
    void SpawnLobbyPlayer(Gamepad gamepad)
    {
        // Check if we’ve reached the max number of players
        if (joinedPlayers >= maxPlayers)
        {
            Debug.Log("Max players reached. Cannot join more players.");
            return;
        }

        foreach (var spawn in playerSpawns)
        {
            if (!spawn.GetComponent<PlayerIndex>().isOccupied)
            {
                var spawnIndex = spawn.GetComponent<PlayerIndex>().playerIndex;
                // Spawn player prefab linked to gamepad
                PlayerInput newPlayer = PlayerInputManager.instance.JoinPlayer(pairWithDevice: gamepad);
                newPlayer.transform.position = spawn.transform.position;
                
                // Ensure player setup remains consistent between lobby scene and main gameplay scene
                PlayerManager.instance.players[spawnIndex].playerInput = newPlayer;
                PlayerManager.instance.players[spawnIndex].playerColour = playerColours[spawnIndex];

                // Add newPlayer to GameManager playerList
                FindAnyObjectByType<GameManager>().AddPlayer(newPlayer.gameObject);

                // Set Spawn point as occupied so other players don't spawn here
                spawn.GetComponent<PlayerIndex>().isOccupied = true;
                newPlayer.GetComponent<PlayerIndex>().playerIndex = spawnIndex;
                
                //Assign player colour & setup HUD
                newPlayer.GetComponent<PlayerHUD>().SetPlayerNum(spawnIndex);
                newPlayer.GetComponent<PlayerHUD>().SetHUDColour(playerColours[spawnIndex]);
                newPlayer.GetComponent<PlayerIndex>().playerIndex = spawnIndex;

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
        foreach (var playerPrefab in playerSpawns)
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
        foreach (var playerPrefab in playerSpawns)
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