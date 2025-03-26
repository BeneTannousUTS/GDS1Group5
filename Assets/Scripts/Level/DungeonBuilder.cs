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
    public GameObject[] spawnedRooms;

    //Instantiates rooms based on the set roomCount, always ending with the final room
    void GenerateRooms()
    {
        while (currentRoom < roomCount - 1)
        {
            spawnedRooms[currentRoom] = Instantiate(rooms[Random.Range(0, rooms.Length)]);
            spawnedRooms[currentRoom].transform.position += new Vector3(0, 25 * currentRoom, 0);
            spawnedRooms[currentRoom].SetActive(false);
            currentRoom += 1;
        }
        Instantiate(finalRoom).transform.position += new Vector3(0, 25 * currentRoom, 0);
    }
    //Activates rooms when the player reaches the end of the prior room (called upon by DungeonCamera)
    public void ActivateRooms(int position)
    {
        if (spawnedRooms[position + 1] != null)
        {
            spawnedRooms[position + 1].SetActive(true);
            if (spawnedRooms[position + 2] != null)
            {

                spawnedRooms[position + 2].SetActive(false);
            }
        }
        if (spawnedRooms[position - 1] != null)
        {
            spawnedRooms[position - 1].SetActive(true);
            if (spawnedRooms[position - 2] != null)
            {

                spawnedRooms[position - 2].SetActive(false);
            }
        }
    }

    void Start()
    {
        spawnedRooms = new GameObject[roomCount];
        GenerateRooms();
         
    }

    void Update()
    {
        
    }

}
