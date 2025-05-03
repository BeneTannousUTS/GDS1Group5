// AUTHOR: James
// Handles building the dungeon

using System.Collections.Generic;
using System.Reflection;
using Unity.Multiplayer.Tools.NetStats;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] GameObject[] EarlyRooms;
    [SerializeField] GameObject[] MiddleRooms;
    [SerializeField] GameObject[] LateRooms;
    [SerializeField] GameObject[] FinalRooms;
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject finalRoom;
    private int numberRooms;
    List<GameObject> allRooms = new List<GameObject>();
    [SerializeField] private int currentRoom = 1;
    private GameObject[] spawnedRooms;
    private DungeonManager dManager;
    public int roomsCleared = 0;

    public int GetNumRooms() 
    {
        return numberRooms;
    }

    public GameObject[] GetSpawnedRooms()
    {
        return spawnedRooms;
    }
    public int GetCurrentRoom()
    {
        return currentRoom;
    }

    public List<GameObject> getRooms()
    {
        return allRooms;
    }

    public void ReplaceRoom(int roomPos, GameObject room)
    {
        Debug.Log(roomPos);
        Destroy(spawnedRooms[roomPos]);
        spawnedRooms[roomPos] = Instantiate(room);  
        spawnedRooms[roomPos].transform.position += new Vector3(0, 18 * (roomPos), 0);
        if (roomPos != (currentRoom))
        {
            spawnedRooms[roomPos].SetActive(false);
        }
    }

    public void ChangeDungeonLength(int cRoom, int newLength)
    {
        GameObject[] tempArray = new GameObject[newLength+1];
        for (int i = 0; i < spawnedRooms.Length; i++)
        {
            if (i <= cRoom)
            {
                tempArray[i] = spawnedRooms[i];
            }
            else
            {
                Destroy (spawnedRooms[i]);
            }
        }
        cRoom++;
        spawnedRooms = tempArray;
        int middle = Random.Range(6, 7);
        GameObject previousRoom = startRoom;
        while (cRoom < (newLength - currentRoom-1))
        {
            if (cRoom <= 3)
            {
                previousRoom = SpawnRooms(EarlyRooms, cRoom, previousRoom);
            }
            else if (cRoom <= middle)
            {
                previousRoom = SpawnRooms(MiddleRooms, cRoom, previousRoom);
            }
            else
            {
                previousRoom = SpawnRooms(LateRooms, cRoom, previousRoom);
            }
            cRoom += 1;
        }
        spawnedRooms[cRoom] = Instantiate(finalRoom);
        spawnedRooms[cRoom].transform.position += new Vector3(0, 18 * cRoom, 0);
        spawnedRooms[cRoom].SetActive(false);
        dManager.SetDungeonLength(newLength);

    }

    GameObject SpawnRooms(GameObject[] rooms, int pos, GameObject previousRoom)
    {
        GameObject newRoom;
        newRoom = rooms[Random.Range(0, rooms.Length)];
        while (newRoom == previousRoom)
        {
            newRoom = rooms[Random.Range(0, rooms.Length)];
        }
        spawnedRooms[pos] = Instantiate(newRoom);
        spawnedRooms[pos].transform.position += new Vector3(0, 18 * pos, 0);
        spawnedRooms[pos].SetActive(false);
        return newRoom;
    }

    public void UpdateRoomCount(int count)
    {
        dManager.SetRoomCount(count + 1);
        currentRoom++;
    }

    //Instantiates rooms based on the set roomCount, always ending with the final room
    void GenerateRooms()
    {
        spawnedRooms[0] = Instantiate(startRoom);
        int middle = Random.Range(6, 7);
        GameObject previousRoom = startRoom;
        while (currentRoom < numberRooms - 1)
        {
            if (currentRoom <= 3)
            {
                previousRoom = SpawnRooms(EarlyRooms, currentRoom, previousRoom);
            }
            else if (currentRoom <= middle)
            {
                previousRoom = SpawnRooms(MiddleRooms, currentRoom, previousRoom);
            }
            else
            {
                previousRoom = SpawnRooms(LateRooms, currentRoom, previousRoom);
            }
            currentRoom += 1;
        }
        spawnedRooms[currentRoom] = Instantiate(finalRoom);
        spawnedRooms[currentRoom].transform.position += new Vector3(0, 18 * currentRoom, 0);
        spawnedRooms[currentRoom].SetActive(false);
        currentRoom = 0;
    }
    //Activates rooms when the player reaches the end of the prior room (called upon by DungeonCamera)
    public void ActivateRooms(int position)
    {
        if (position+1<numberRooms && spawnedRooms[position + 1] != null)
        {
            spawnedRooms[position + 1].SetActive(true);
            if (position + 2 < numberRooms && spawnedRooms[position + 2] != null)
            {
                
                spawnedRooms[position + 2].SetActive(false);
            }
        }
        if (position-1>0 && spawnedRooms[position - 1] != null)
        {
            spawnedRooms[position - 1].SetActive(true);
            if (position - 2 < numberRooms && spawnedRooms[position - 2] != null)
            {

                spawnedRooms[position - 2].SetActive(false);
            }
        }
    }

    void AllRooms()
    {
        foreach (GameObject room in EarlyRooms)
        {
            allRooms.Add(room);
        }
        foreach (GameObject room in MiddleRooms)
        {
            allRooms.Add(room);
        }
        foreach (GameObject room in LateRooms)
        {
            allRooms.Add(room);
        }
    }

    public void FinalRoomSelect(int i)
    {
        dManager = gameObject.GetComponent<DungeonManager>();
        numberRooms = dManager.GetDungeonLength();
        spawnedRooms = new GameObject[numberRooms + 1];
        finalRoom = FinalRooms[i];
        GenerateRooms();
        AllRooms();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
