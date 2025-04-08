// AUTHOR: James
// Handles building the dungeon

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject finalRoom;
    private int numberRooms;
    private int currentRoom = 1;
    private GameObject[] spawnedRooms;
    private DungeonManager dManager;
    public int roomsCleared = 0;

    public int GetNumRooms() 
    {
        return numberRooms;
    }

    public int GetCurrentRoom()
    {
        return currentRoom;
    }

    public void UpdateRoomCount(int count)
    {
        dManager.SetRoomCount(count + 1);
    }

    //Instantiates rooms based on the set roomCount, always ending with the final room
    void GenerateRooms()
    {
        while (currentRoom < numberRooms - 1)
        {
            spawnedRooms[currentRoom] = Instantiate(rooms[Random.Range(0, rooms.Length)]);
            spawnedRooms[currentRoom].transform.position += new Vector3(0, 18 * currentRoom, 0);
            spawnedRooms[currentRoom].SetActive(false);
            currentRoom += 1;
        }
        spawnedRooms[currentRoom] = Instantiate(finalRoom);
        spawnedRooms[currentRoom].transform.position += new Vector3(0, 18 * currentRoom, 0);
        spawnedRooms[currentRoom].SetActive(false);
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

    void Start()
    {
        dManager = gameObject.GetComponent<DungeonManager>();
        numberRooms = dManager.GetDungeonLength();
        spawnedRooms = new GameObject[numberRooms + 1];
        GenerateRooms();
    }

    void Update()
    {
        
    }
}
