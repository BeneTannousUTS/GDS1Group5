// AUTHOR: BENEDICT
// This script handles player joining and logic to update the UI as players join and leave

using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    public PlayerSlot[] playerSlots;
    private int joinedPlayers = 0;
    private int maxPlayers = 4;

    void Update()
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (IsGamepadAssigned(gamepad))
            {
                Debug.Log($"Gamepad {gamepad.deviceId} is already assigned to a slot.");
                UnassignGamepadSlot(gamepad);

                AssignGamepadSlot(gamepad);
            }
        }
    }

    // Assign the gamepad to the first available slot
    void AssignGamepadSlot(Gamepad gamepad)
    {
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            if (IsGamepadAssigned(gamepad))
            {
                Debug.Log($"Gamepad {gamepad.deviceId} is already assigned to a slot.");
                UnassignGamepadSlot(gamepad);
                return;
            }
            // Assign the gamepad to the first available slot
            foreach (var slot in playerSlots)
            {
                if (!slot.isOccupied)
                {
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
    }
    
    void UnassignGamepadSlot(Gamepad gamepad)
    {
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.panelGamepad == gamepad)
                {
                    slot.unjoinedIndicator.SetActive(true);
                    slot.joinedIndicator.SetActive(false);
                    slot.isOccupied = false;
                    slot.panelGamepad = null;
                    joinedPlayers--;

                    Debug.Log($"Gamepad {gamepad.deviceId} left the slot.");
                    break;
                }
            }
        }
    }
    
    //Check if this gamepad has already been assigned to another slot
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
}