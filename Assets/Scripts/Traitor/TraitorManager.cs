// AUTHOR: James
// Manages when a traitor card should appear

using System;
using UnityEngine;

public class TraitorManager : MonoBehaviour
{
    private BaseTraitor traitorType;
    private float traitorRoom;
    private bool traitorActive = false;
    public DungeonManager dungeonManager;
    [SerializeField] GameObject[] objectRef;
    [SerializeField] Sprite[] cardRef;
    [SerializeField] RuntimeAnimatorController[] traitorAnims;
    [SerializeField] GameObject healthBoost;
    [SerializeField] GameObject[] weapons;
    [SerializeField] Sprite[] icons;
    [SerializeField] GameObject[] extras;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Called by game manager when the traitor is decided
    public void SetTraitorType(BaseTraitor traitor)
    {
        traitorType = traitor;
        traitorRoom = dungeonManager.GetDungeonLength() - traitorType.GetTraitorRoom();
    }

    public RuntimeAnimatorController GetAnim(int i)
    {
        return traitorAnims[i];
    }
    public BaseTraitor GetTraitorType()
    {
        return traitorType;
    }

    public GameObject GetObjectRef(int pos)
    {
        return objectRef[pos];
    }

    public Sprite GetCardRef(int pos)
    {
        return cardRef[pos];
    }

    //called by the card manager when cards are being decided to tell if the traitor card should appear
    public bool CheckTraitorAppear()
    {
        traitorRoom = dungeonManager.GetDungeonLength() - traitorType.GetTraitorRoom();
        Debug.Log($"currentRoom: {dungeonManager.GetRoomCount()}, traitorRoom: {traitorRoom}, isFinal: {dungeonManager.GetRoomCount() == traitorRoom}, traitorType: {traitorType}");
        if (dungeonManager.GetRoomCount() == traitorRoom)
        {
            traitorActive = true;
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

    public bool IsTraitorActive() { return traitorActive; }

    void Start()
    {
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject GetHealthBoost()
    {
        return healthBoost;
    }

    internal GameObject[] GetWeapons()
    {
        return weapons;
    }

    public Sprite GetIcon(int pos)
    {
        return icons[pos];
    }

    public GameObject GetExtras(int pos)
    {
        return extras[pos];
    }
}
