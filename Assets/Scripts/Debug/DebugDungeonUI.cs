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
    public TMP_Dropdown roomDropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SelectRoom()
    {
        roomNumber = roomDropdown.value;
    }

    public void RoomCountDropdown()
    {
        newDungeonLength = dropdownRoomLength.value+1;
    }

    public void UpdateRoom()
    {
        if (selectedRoom != null)
        {
            db.ReplaceRoom(roomNumber, selectedRoom);
            roomDropdown.options.Clear();
            foreach (GameObject room in db.GetSpawnedRooms())
            {
                if (room != null)
                {
                    roomDropdown.options.Add(new TMP_Dropdown.OptionData(((room.transform.position.y / 18) + 1) + ": " + room.name));
                }

            }
        }
    }

    public void ChangeDungeonLength()
    {
        if (db.GetCurrentRoom() < newDungeonLength - 1)
        {
            db.ChangeDungeonLength(db.GetCurrentRoom(), newDungeonLength);
            roomDropdown.options.Clear();
            foreach (GameObject room in db.GetSpawnedRooms())
            {
                if (room != null)
                {
                    roomDropdown.options.Add(new TMP_Dropdown.OptionData(((room.transform.position.y / 18) + 1) + ": " + room.name));
                }

            }
        }
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
        foreach (GameObject room in db.GetSpawnedRooms())
        {
            if (room != null)
            {
                roomDropdown.options.Add(new TMP_Dropdown.OptionData(((room.transform.position.y / 18) + 1) + ": " + room.name));
            }
        }
        dropdown.value = -1;
        roomDropdown.value = -1;
        dropdownRoomLength.value = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
