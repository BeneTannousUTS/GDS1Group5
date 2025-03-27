using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneManager : MonoBehaviour
{
    public Transform[] spawnPoints; // Assign spawn points for players

    void Start()
    {
        PlayerManager.PlayerData[] players = PlayerManager.instance.players;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isJoined)
            {
                // Use PlayerInputManager to spawn a new player and pair it with the gamepad
                PlayerInput newPlayer = PlayerInputManager.instance.JoinPlayer(pairWithDevice: players[i].gamepad);

                // Position the new player at the corresponding spawn point
                newPlayer.transform.position = spawnPoints[i].position;

                Debug.Log($"Spawned Player {i + 1} with Gamepad: {players[i].gamepad.deviceId}");
            }
        }
    }
}