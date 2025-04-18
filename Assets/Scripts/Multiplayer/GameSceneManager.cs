// AUTHOR: BENEDICT
// This script uses the PlayerInputManager to join new players with unique controller identities

using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public Color[] playerColours;
    public AnimatorOverrideController[] playerAnimators;
        
    void Start()
    {
        
    }

    // Iterate over each player in the PlayerManager and Join each player into the game, setting HUD identifiers
    public void SpawnPlayers()
    {
        PlayerData[] players = PlayerManager.instance.players;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isJoined)
            {
                // Use PlayerInputManager to spawn a new player and pair it with the gamepad
                PlayerInput newPlayer = PlayerInputManager.instance.JoinPlayer(pairWithDevice: players[i].gamepad);
                PlayerManager.instance.players[i].playerInput = newPlayer;

                // Add newPlayer to GameManager playerList
                FindAnyObjectByType<GameManager>().AddPlayer(newPlayer.gameObject);

                // Position the new player at the corresponding spawn point
                newPlayer.transform.position = spawnPoints[i].transform.position;
                newPlayer.GetComponent<PlayerHUD>().SetPlayerNum(i);
                newPlayer.GetComponent<PlayerHUD>().SetHUDColour(playerColours[i]);
                newPlayer.gameObject.GetComponent<PlayerColour>().playerColour = playerColours[i];
                newPlayer.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = playerColours[i];
                
                if (i != 0)
                {
                    newPlayer.GetComponent<Animator>().runtimeAnimatorController = playerAnimators[i - 1];
                }

                Debug.Log($"Spawned Player {i + 1} with Gamepad: {players[i].gamepad.deviceId} at {transform.position}");
            }
        }
    }
}