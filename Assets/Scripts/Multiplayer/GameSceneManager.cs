// AUTHOR: BENEDICT
// This script uses the PlayerInputManager to join new players with unique controller identities

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneManager : MonoBehaviour
{
    // public GameObject[] spawnPoints;
    // public Color[] playerColours;
    // public AnimatorOverrideController[] playerAnimators;
    public GameObject dungeonManager;
    public GameObject[] inactivePlayerPrefabs;
    public GameObject[] activePlayerPrefabs;
    
    private int joinedPlayers = 0;
    private int maxPlayers = 4;
    
    void Start()
    {
    }

    private void Update()
    {
        if (dungeonManager != null && dungeonManager.GetComponent<DungeonBuilder>().GetCurrentRoom() == 1)
        {
            if(dungeonManager.GetComponent<DungeonBuilder>())
            // Loop through all connected gamepads
            foreach (Gamepad gamepad in Gamepad.all)
            {
                // Check if the player wants to join
                if (gamepad.leftShoulder.wasPressedThisFrame && gamepad.rightShoulder.wasPressedThisFrame && !IsGamepadAssigned(gamepad))
                {
                    OnControlsChanged(gamepad);
                    AssignGamepad(gamepad);
                }
                // Check if the player wants to leave
                else if (gamepad.startButton.wasPressedThisFrame && gamepad.selectButton.wasPressedThisFrame && IsGamepadAssigned(gamepad))
                {
                    OnControlsChanged(gamepad);
                    UnassignGamepad(gamepad);
                }
            }
        }
    }

    public void AssignPlayerPrefabs(GameObject[] playerPrefabs)
    {
        activePlayerPrefabs = playerPrefabs;
    }
    
    // Assign the gamepad to the first available player
    void AssignGamepad(Gamepad gamepad)
    {
        // Check if we’ve reached the max number of players
        if (joinedPlayers >= maxPlayers)
        {
            Debug.Log("Max players reached. Cannot join more players.");
            return;
        }

        foreach (var player in inactivePlayerPrefabs)
        {
            if (player.GetComponent<PlayerInput>().GetDevice<Gamepad>() != gamepad)
            {
                //Assign gamepad to player
                PlayerInput newPlayer = PlayerInputManager.instance.JoinPlayer(pairWithDevice: gamepad);

                joinedPlayers++;
                PlayerManager.instance.JoinPlayer(gamepad);

                Debug.Log($"Gamepad {gamepad.deviceId} assigned to slot.");
                break;
            }
        }
    }

    // Unassign the gamepad from its current slot
    void UnassignGamepad(Gamepad gamepad)
    {
        foreach (var player in activePlayerPrefabs)
        {
            if (player.GetComponent<PlayerInput>().GetDevice<Gamepad>() == gamepad)
            {
                // Update slot state
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
        foreach (var player in activePlayerPrefabs)
        {
            if (player.GetComponent<PlayerInput>().GetDevice<Gamepad>() == gamepad)
            {
                return true;
            }
        }
        return false;
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