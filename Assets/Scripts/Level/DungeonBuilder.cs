// AUTHOR: James
// Handles building the dungeon

using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject finalRoom;
    public int roomCount;
    private int currentRoom = 1;

    //Instantiates rooms based on the set roomCount, always ending with the final room
    void GenerateRooms()
    {
        while (currentRoom < roomCount - 1)
        {
            Instantiate(rooms[Random.Range(0, rooms.Length)]).transform.position += new Vector3(0,25*currentRoom,0);
            currentRoom += 1;
        }
        Instantiate(finalRoom).transform.position += new Vector3(0, 25 * currentRoom, 0);
    }

    void Start()
    {
        GenerateRooms();
    }

    void Update()
    {
        
    }

}
