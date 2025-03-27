// AUTHOR: BENEDICT
// This script handles players joining and carrying unique controller identities into the main gameplay

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [System.Serializable]
    public class PlayerData
    {
        public int playerIndex;
        public Gamepad gamepad;
        public bool isJoined;
    }

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

    //Get player data
    public PlayerData[] GetPlayers()
    {
        return players;
    }
}
