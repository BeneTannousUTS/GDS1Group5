// AUTHOR: BENEDICT
// This script stores identifiers for players joining and passes unique controller identities into the main gameplay

using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerData[] players = new PlayerData[4];

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Add player's controller id to their PlayerData when they join
    public bool JoinPlayer(Gamepad gamepad)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isJoined)
            {
                players[i].playerIndex = i;
                players[i].gamepad = gamepad;
                players[i].isJoined = true;

                Debug.Log($"Player {i + 1} joined with Gamepad: {gamepad.deviceId}");
                return true;
            }
        }

        Debug.Log("All player slots are full!");
        return false;
    }

    // Remove player index that matches the controller that unjoined & remove controller ID
    public bool UnjoinPlayer(Gamepad gamepad)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].gamepad == gamepad)
            {
                players[i].isJoined = false;
                players[i].gamepad = null;
                players[i].playerIndex = i;
                
                Debug.Log($"Player {i + 1} left with Gamepad: {gamepad.deviceId}");
                return true;
            }
        }
        
        Debug.Log("All player slots are empty!");
        return false;
    }

    public void ResetPlayers()
    {
        if (players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].isJoined = false;
                players[i].gamepad = null;
                players[i].playerIndex = i;
            }
        }
        
        Debug.Log("Players reset!");
    }

    //Get player data
    public PlayerData[] GetPlayers()
    {
        return players;
    }
}
