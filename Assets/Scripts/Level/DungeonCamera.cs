// AUTHOR: James
// Handles moving the camera when moving between dungeon rooms

using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    // Moves the camera to the position of the room that the player has just entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Camera.main.transform.position = transform.parent.transform.position + new Vector3(0,0,-10);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
