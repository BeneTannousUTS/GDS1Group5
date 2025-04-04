// AUTHOR: James
// Manages dungeon variables that may need to be accessed elsewhere

using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private Vector3 roomPos = new Vector3(0,0,0);
    [SerializeField] int roomCount = 1;

    public int GetRoomCount() { 
        return roomCount; 
        }
    public void SetRoomCount(int count) {roomCount = count; roomPos = new Vector3(0, roomCount * 18, 0); }
    public Vector3 GetRoomPos() { return roomPos; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
