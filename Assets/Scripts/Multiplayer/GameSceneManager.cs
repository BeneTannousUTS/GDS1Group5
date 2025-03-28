using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public Color[] playerColours;
        
    void Start()
    {
        
    }

    public void SpawnPlayers()
    {
        PlayerManager.PlayerData[] players = PlayerManager.instance.players;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isJoined)
            {
                // Use PlayerInputManager to spawn a new player and pair it with the gamepad
                PlayerInput newPlayer = PlayerInputManager.instance.JoinPlayer(pairWithDevice: players[i].gamepad);

                // Position the new player at the corresponding spawn point
                newPlayer.transform.position = spawnPoints[i].transform.position;
                newPlayer.GetComponent<SpriteRenderer>().color = playerColours[i];

                Debug.Log($"Spawned Player {i + 1} with Gamepad: {players[i].gamepad.deviceId} at {transform.position}");
            }
        }
    }
}