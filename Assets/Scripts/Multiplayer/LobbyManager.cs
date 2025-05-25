// AUTHOR: BENEDICT
// This script handles player joining and logic to update the UI as players join and leave

using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;
    [SerializeField] Color[] playerColours;
    [SerializeField] AnimatorOverrideController[] playerAnimators;


    private GameObject[] spawnedPlayers;
    private int joinedPlayers;
    private readonly int maxPlayers = 4;
    private bool lobbyUnlocked = true;

    private void Update()
    {
        if (lobbyUnlocked)
        {
            CheckControllerInput();
        }
    }

    private void CheckControllerInput()
    {
        // Loop through all connected gamepads
        foreach (var gamepad in Gamepad.all)
            // Check if the player wants to join
            if (gamepad.leftShoulder.isPressed && gamepad.rightShoulder.isPressed && !IsGamepadAssigned(gamepad))
            {
                OnControlsChanged(gamepad);
                SpawnLobbyPlayer(gamepad);
            }
            // Check if the player wants to leave
            else if (gamepad.selectButton.wasPressedThisFrame && IsGamepadAssigned(gamepad))
            {
                OnControlsChanged(gamepad);
                DespawnLobbyPlayer(gamepad);
            }
        // Check if the first player wants to start the game
        // else if (gamepad.startButton.wasPressedThisFrame &&
        //          gamepad == playerSpawns[0].GetComponent<PlayerInput>().GetDevice<Gamepad>())
        // {
        //     StartGame();
        // }

        /*
        if (Input.GetKeyDown(KeyCode.Q)) // change to true if you want for testing
        {
            if (PlayerManager.instance.players[0].isJoined)
                SpawnLobbyPlayer(PlayerManager.instance.players[0].gamepad);
        }
        */
    }

    // Assign the gamepad to the first available player
    private void SpawnLobbyPlayer(Gamepad gamepad)
    {
        // Check if we’ve reached the max number of players
        if (joinedPlayers >= maxPlayers)
        {
            Debug.Log("Max players reached. Cannot join more players.");
            return;
        }

        foreach (var spawn in playerSpawns)
            if (spawn.GetComponent<PlayerIndex>().isOccupied == false)
            {
                var spawnIndex = spawn.GetComponent<PlayerIndex>().playerIndex;
                PlayerManager.instance.players[spawnIndex].playerColour = playerColours[spawnIndex];
                
                var particle = spawn.GetComponent<ParticleSystem>();
                var main = particle.main;
                main.startColor = playerColours[spawnIndex];
                
                particle.Play();
                
                // Spawn player prefab linked to gamepad
                var newPlayer = PlayerInputManager.instance.JoinPlayer(pairWithDevice: gamepad);
                newPlayer.transform.position = spawn.transform.position;
                newPlayer.actions.Enable();
                
                // Ensure player setup remains consistent between lobby scene and main gameplay scene
                PlayerManager.instance.players[spawnIndex].playerInput = newPlayer;
                
                // Add newPlayer to GameManager playerList
                // FindAnyObjectByType<GameManager>().AddPlayer(newPlayer.gameObject);
                
                // Set Spawn point as occupied so other players don't spawn here
                spawn.GetComponent<PlayerIndex>().SetOccupied(true);
                newPlayer.GetComponent<PlayerIndex>().playerIndex = spawnIndex;
                
                // Hide Spawn point sprite so it looks like player has risen
                spawn.GetComponent<SpriteRenderer>().enabled = false;
                //SetActive(false);
                
                //Assign player colour & setup HUD element
                newPlayer.GetComponent<PlayerHUD>().SetPlayerNum(spawnIndex);
                newPlayer.GetComponent<PlayerHUD>().SetHUDColour(playerColours[spawnIndex]);
                newPlayer.GetComponent<PlayerIndex>().playerIndex = spawnIndex;
                
                newPlayer.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = playerColours[spawnIndex];
                
                if (spawnIndex != 0)
                {
                    newPlayer.GetComponent<Animator>().runtimeAnimatorController = playerAnimators[spawnIndex - 1];
                }
                
                joinedPlayers++;
                PlayerManager.instance.JoinPlayer(gamepad);
                AudioManager.instance.PlaySoundEffect("UIConfirm", 2.0f);
                
                Debug.Log($"Gamepad {gamepad.deviceId} assigned to slot.");
                break;
            }
    }

    // // Unassign the gamepad from its current slot
    private void DespawnLobbyPlayer(Gamepad gamepad)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
            if (gamepad == PlayerManager.instance.players[player.GetComponent<PlayerIndex>().playerIndex].gamepad)
            {
                // Update slot state
                player.GetComponent<PlayerHUD>().DestroyHUD();
                GameObject.FindGameObjectWithTag("PlayerHUDContainer").transform.GetComponent<LobbyHudHelper>()
                    .ReactivateJoinPanel(player.GetComponent<PlayerIndex>().playerIndex);
                foreach (var spawn in playerSpawns)
                {
                    if (player.GetComponent<PlayerIndex>().playerIndex == spawn.GetComponent<PlayerIndex>().playerIndex)
                    {
                        spawn.transform.position = player.transform.position;
                        spawn.GetComponent<PlayerIndex>().isOccupied = false;
                        spawn.GetComponent<ParticleSystem>().Play();
                        spawn.GetComponent<SpriteRenderer>().enabled = true;
                        // spawn.SetActive(true);
                    }
                }

                Destroy(player);

                //playerPrefab.panelGamepad = null;
                joinedPlayers--;
                PlayerManager.instance.UnjoinPlayer(gamepad);
                AudioManager.instance.PlaySoundEffect("UIReject", 2.0f);

                Debug.Log($"Gamepad {gamepad.deviceId} left the lobby.");
                break;
            }
    }

    // Check if this gamepad has already been assigned to another slot
    private bool IsGamepadAssigned(Gamepad gamepad)
    {
        foreach (var player in PlayerManager.instance.players)
        {
            if (player.isJoined && player.gamepad == gamepad)
                return true;
        }

        return false;
    }

    public void StartGame()
    {
        if (lobbyUnlocked) return;

        SceneManager.LoadScene("GameScene");
    }

    public bool GetLobbyUnlocked()
    {
        return lobbyUnlocked;
    }

    public void SetLobbyUnlocked(bool isGameJoinable)
    {
        lobbyUnlocked = isGameJoinable;
    }

    //Disable XInput device since Unity has a bug where Switch pro controllers are recognised as two inputs
    private void OnControlsChanged(Gamepad gamepad)
    {
        if (gamepad is SwitchProControllerHID)
            foreach (var item in Gamepad.all)
                if (item is XInputController && Math.Abs(item.lastUpdateTime - gamepad.lastUpdateTime) < 0.01 &&
                    !IsGamepadAssigned(gamepad))
                {
                    Debug.Log(
                        $"Switch Pro controller detected and a copy of XInput was active at almost the same time. Disabling XInput device. `{gamepad}`; `{item}`");
                    InputSystem.DisableDevice(item);
                }
    }
}