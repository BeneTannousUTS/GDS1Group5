// AUTHOR: James
// Manages when a traitor card should appear

using System;
using UnityEngine;

public class TraitorManager : MonoBehaviour
{
    private ITraitor traitorType;
    private float traitorRoom;
    private bool traitorCardAppear;
    public DungeonManager dungeonManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Called by game manager when the traitor is decided
    public void SetTraitorType(ITraitor traitor)
    {
        traitorType = traitor;
        traitorRoom = dungeonManager.GetDungeonLength() - traitorType.getTraitorRoom();
    }

    //called by the card manager when cards are being decided to tell if the traitor card should appear
    public bool CheckTraitorAppear()
    {
        Debug.Log($"currentRoom: {dungeonManager.GetRoomCount()}, traitorRoom: {traitorRoom - 1}, isFinal: {dungeonManager.GetRoomCount() == traitorRoom - 1}");
        if (dungeonManager.GetRoomCount() == traitorRoom - 1)
        {
            return true;
        }
        return false;
    }

    public int GetTraitorAmount()
    {
        string amount = traitorType.GetAmountOfTraitors();
        int playerAmount = FindAnyObjectByType<GameManager>().GetPlayerList().Count;
        if (amount == "single")
        {
            return 1;
        }
        else if (amount == "everyone")
        {
            return playerAmount;
        }
        else if (amount == "teams")
        {
            return 2;
        }
        else if (amount == "majority")
        {
            return playerAmount - 1;
        }
        return 0;
    }
    
    void Start()
    {
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
