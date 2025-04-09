using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugDungeonUI : MonoBehaviour
{
    GameObject dungeonManager;
    DungeonManager dm;
    DungeonBuilder db;
    GameObject selectedRoom;
    GameManager gameManager;
    int roomNumber = 0;
    [SerializeField] int newDungeonLength = 5;
    public TMP_Dropdown dropdown;
    public TMP_Dropdown dropdownRoomLength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SelectCurrentRoom()
    {
        roomNumber = -1;
    }

    public void SelectNextRoom()
    {
        roomNumber = 0;
    }

    public void RoomCountDropdown()
    {
        newDungeonLength = dropdownRoomLength.value+1;
    }

    public void UpdateRoom()
    {
        db.ReplaceRoom((db.GetCurrentRoom() + roomNumber), selectedRoom);
    }

    public void ChangeDungeonLength()
    {
        db.ChangeDungeonLength(db.GetCurrentRoom()-1, newDungeonLength);
    }

    public void DungeonDropdown()
    {
        int pickedEntry = dropdown.value;
        selectedRoom = db.getRooms()[pickedEntry];
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        dungeonManager = FindAnyObjectByType<DungeonManager>().gameObject;
        dm = dungeonManager.GetComponent<DungeonManager>();
        db = dungeonManager.GetComponent<DungeonBuilder>();
        foreach (GameObject room in db.getRooms())
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(room.name));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
