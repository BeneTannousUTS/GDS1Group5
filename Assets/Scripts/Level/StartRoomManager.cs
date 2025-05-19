// AUTHOR: Benedict
// Handles lobby room functionality

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartRoomManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject doorPressurePlates;
    [SerializeField] GameObject doorProgressBar;
    [SerializeField] LobbyManager lobby;
    
    public float doorDelayTime = 3;

    private float doorTimer;
    private bool openDoor;
    private float checkTimer;
    public bool roomChange = false;
    private float roomChangeTimer;
    private bool shouldBarUpdate = true;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        if (CountActivePlayers() < 2 && shouldBarUpdate)
        {
            doorProgressBar.GetComponent<ProgressBarHelper>().SetProgressBarText("Waiting for enough players to join...");
        }
        else if (doorPressurePlates.GetComponent<DoorPressurePlate>().CountActivePlates() ==
                 CountActivePlayers() && CountActivePlayers() >= 2 && shouldBarUpdate)
        {
            //Debug.Log("Door countdown active " + CountActivePlayers());
            doorProgressBar.SetActive(true);
            DoorOpenDelayTimer(true);
            doorProgressBar.GetComponent<ProgressBarHelper>().SetProgressBarText("Starting game with " + CountActivePlayers() + " players!");
        }
        else if (shouldBarUpdate)
        {
            //Debug.Log("Door countdown inactive " + CountActivePlayers());
            doorProgressBar.GetComponent<ProgressBarHelper>().SetProgressBarText("Step on the plates to start game!");
            DoorOpenDelayTimer(false);
        }
        
        if (roomChange)
        {
            LoadGameScene();
        }
    }
     void LoadGameScene()
    {
        roomChangeTimer += Time.deltaTime;
        if (roomChangeTimer > 2)
        {
            lobby.StartGame();
            
            Debug.Log("Wario");
        }
    }

    void DoorOpenDelayTimer(bool shouldDoorOpen)
    {
        if (shouldDoorOpen && doorTimer < doorDelayTime && shouldBarUpdate)
        {
            doorTimer += Time.deltaTime;
            doorProgressBar.GetComponent<ProgressBarHelper>().SetProgressBarFill(doorTimer/doorDelayTime);

            if (doorTimer >= doorDelayTime)
            {
                door.GetComponent<Door>().OpenDoor();

                int maxHealthAmount = 200 - (CountActivePlayers() - 2) * 50;

                PlayerData[] players = PlayerManager.instance.GetPlayers();

                foreach(PlayerData player in players)
                {
                    if (!player.isJoined) continue;

                    player.playerInput.gameObject.GetComponent<HealthComponent>().SetMaxHealth(maxHealthAmount);
                    player.playerInput.gameObject.GetComponent<HealthComponent>().UpdateHUDHealthBar();
                }

                shouldBarUpdate = false;
                
                LobbyHudHelper hud = GameObject.FindGameObjectWithTag("PlayerHUDContainer").transform.GetComponent<LobbyHudHelper>();
                foreach (var panel in hud.joinPanels)
                {
                    if (panel.activeSelf)
                    {
                        Destroy(panel);
                    }
                }
                
                doorProgressBar.GetComponent<ProgressBarHelper>().SetProgressBarText("Good luck...");
                hud.DestroyLeavePrompt();
                lobby.SetLobbyUnlocked(false);
            }
        }else if (!shouldDoorOpen && doorTimer > 0 && shouldBarUpdate)
        {
            doorTimer -= Time.deltaTime;
            doorProgressBar.GetComponent<ProgressBarHelper>().SetProgressBarFill(doorTimer/doorDelayTime);
            if (doorTimer < 0)
            {
                doorTimer = 0;
            }
        }
    }

    int CountActivePlayers()
    {
        int activePlayers = 0;
        foreach (var player in PlayerManager.instance.GetComponent<PlayerManager>().GetPlayers())
        {
            if (player.isJoined)
            {
                activePlayers++;
            }
        }
        return activePlayers;
    }
    
    public void HidePlayer(GameObject player)
    {
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Locked");

        foreach (Transform child in player.transform)
        {
            if (child.GetComponent<WeaponStats>())
            {
                Destroy(child.gameObject);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
