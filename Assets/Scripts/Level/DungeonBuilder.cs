// AUTHOR: James
// Handles building the dungeon

using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject finalRoom;
    public int numberOfRooms;
    private int currentRoom = 1;
    private GameObject[] spawnedRooms;

    //Instantiates rooms based on the set roomCount, always ending with the final room
    void GenerateRooms()
    {
        while (currentRoom < numberOfRooms - 1)
        {
            spawnedRooms[currentRoom] = Instantiate(rooms[Random.Range(0, rooms.Length)]);
            spawnedRooms[currentRoom].transform.position += new Vector3(0, 25 * currentRoom, 0);
            spawnedRooms[currentRoom].SetActive(false);
            currentRoom += 1;
        }
        spawnedRooms[currentRoom] = Instantiate(finalRoom);
        spawnedRooms[currentRoom].transform.position += new Vector3(0, 25 * currentRoom, 0);
        spawnedRooms[currentRoom].SetActive(false);
    }
    //Activates rooms when the player reaches the end of the prior room (called upon by DungeonCamera)
    public void ActivateRooms(int position)
    {
<<<<<<< Updated upstream
=======
<<<<<<< Updated upstream
        dManager.SetRoomCount(position+1);
=======
<<<<<<< Updated upstream
>>>>>>> Stashed changes
>>>>>>> Stashed changes
        if (position+1<roomCount && spawnedRooms[position + 1] != null)
=======
        dManager.SetRoomCount(position+1);
        if (position+1<numberOfRooms && spawnedRooms[position + 1] != null)
>>>>>>> Stashed changes
        {
            spawnedRooms[position + 1].SetActive(true);
            if (position + 2 < numberOfRooms && spawnedRooms[position + 2] != null)
            {

                spawnedRooms[position + 2].SetActive(false);
            }
        }
        if (position-1>0 && spawnedRooms[position - 1] != null)
        {
            spawnedRooms[position - 1].SetActive(true);
            if (position - 2 < numberOfRooms && spawnedRooms[position - 2] != null)
            {

                spawnedRooms[position - 2].SetActive(false);
            }
        }
    }

    void Start()
    {
        spawnedRooms = new GameObject[numberOfRooms];
        GenerateRooms();
         
    }

    void Update()
    {
        
    }

}
