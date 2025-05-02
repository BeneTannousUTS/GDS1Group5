// AUTHOR: Benedict
// Handles lobby room functionality

using System.Collections.Generic;
using UnityEngine;

public class StartRoomManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject doorPressurePlates;
    
    public float doorDelayTime = 3;

    private float doorTimer;
    private bool openDoor;
    private bool doorBeingDestroyed = false;
    private float checkTimer;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (doorPressurePlates.GetComponent<DoorPressurePlate>().CountActivePlates() ==
            CountActivePlayers() && CountActivePlayers() >= 2)
        {
            Debug.Log("Door countdown active " + CountActivePlayers());
            DoorOpenDelayTimer(true);
        }
        else
        {
            Debug.Log("Door countdown inactive " + CountActivePlayers());
            DoorOpenDelayTimer(false);
        };
    }

    void DoorOpenDelayTimer(bool shouldDoorOpen)
    {
        if (shouldDoorOpen && doorTimer < doorDelayTime)
        {
            doorTimer += Time.deltaTime;

            if (doorTimer >= doorDelayTime)
            {
                door.GetComponent<Door>().OpenDoor();
            }
        }else if (!shouldDoorOpen && doorTimer > 0)
        {
            doorTimer -= Time.deltaTime;
            if (doorTimer < 0)
            {
                doorTimer = 0;
            }
        };
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
}
