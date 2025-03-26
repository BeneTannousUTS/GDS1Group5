// AUTHOR: James
// Handles moving the camera when moving between dungeon rooms

using System;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    private DungeonBuilder dungeonBuild;
    // Moves the camera to the position of the room that the player has just entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Camera.main.transform.position = transform.parent.transform.position + new Vector3(0,0,-10);
            Debug.Log((int)transform.position.y / 25);
            dungeonBuild.ActivateRooms((int)transform.position.y / 25);
            
        }
    }
    void Start()
    {
        dungeonBuild = FindAnyObjectByType<DungeonBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
