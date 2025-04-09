// AUTHOR: James
// Manages when a traitor card should appear

using System;
using UnityEngine;

public class TraitorManager : MonoBehaviour
{
    private BaseTraitor traitorType;
    private float traitorRoom;
    private bool traitorCardAppear;
    public DungeonManager dungeonManager;
    [SerializeField] GameObject[] summonRef;
    [SerializeField] Sprite[] cardRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Called by game manager when the traitor is decided
    public void SetTraitorType(BaseTraitor traitor)
    {
        traitorType = traitor;
        traitorRoom = dungeonManager.GetDungeonLength() - traitorType.getTraitorRoom();
    }

    public GameObject GetSummonRef(int pos)
    {
        return summonRef[pos];
    }

    public Sprite GetCardRef(int pos)
    {
        return cardRef[pos];
    }

    //called by the card manager when cards are being decided to tell if the traitor card should appear
    public bool CheckTraitorAppear()
    {
        Debug.Log($"currentRoom: {dungeonManager.GetRoomCount()}, traitorRoom: {traitorRoom}, isFinal: {dungeonManager.GetRoomCount() == traitorRoom}, traitorType: {traitorType}");
        if (dungeonManager.GetRoomCount() == traitorRoom)
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
