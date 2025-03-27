// AUTHOR: BENEDICT
// This script handles player joining and logic to update the UI as players join and leave

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
        public bool isOccupied;
    }
    
    public PlayerSlot[] playerSlots;
    private int joinedPlayers = 0;
    private int maxPlayers = 4;

    public void TryJoinGame(int controllerID)
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                PlayerManager.instance.JoinPlayer(gamepad);
            }
        }
    }
}
