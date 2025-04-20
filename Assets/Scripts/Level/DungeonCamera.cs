// AUTHOR: James
// Handles moving the camera when moving between dungeon rooms

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonCamera : MonoBehaviour
{
    private DungeonBuilder dungeonBuild;
    private float roomChangeTimer;
    private bool roomChange = false;
    private EnemyPathfinder ePath;
    bool roomCleared;
    // Moves the camera to the position of the room that the player has just entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !roomCleared)
        {
            roomCleared = true;
            gameObject.GetComponentInParent<RoomManager>().ClearRoom();
            GameObject player = collision.gameObject;
            FindAnyObjectByType<CardManager>().HidePlayer(player);
            roomChange = true;
            dungeonBuild.UpdateRoomCount((int)((transform.position.y - 9.5) / 18));
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            FindAnyObjectByType<CardManager>().HidePlayer(player);
        }
    }

    void MoveCamera()
    {
        Camera.main.transform.position = transform.parent.transform.position + new Vector3(0,18, -10);
        Debug.Log("Transform y: " + transform.position.y);
        dungeonBuild.ActivateRooms((int)((transform.position.y-9.5) / 18));
    }

    public void RoomChangeTime()
    {
        foreach (GameObject player in ePath.GetPlayers())
        {
            FindAnyObjectByType<CardManager>().ShowPlayers();
        }
        MoveCamera();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        roomChange = false;
    }

    void LoadCardScene()
    {
        roomChangeTimer += Time.deltaTime;
        if (roomChangeTimer > 2)
        {
            dungeonBuild.roomsCleared++;
            foreach (GameObject player in ePath.GetPlayers())
            {
                FindAnyObjectByType<CardManager>().HidePlayer(player);
            }

            roomChange = false;

            FindAnyObjectByType<GameManager>().ShowCardSelection(this);
            Debug.Log("Wario");
        }
    }
    void Start()
    {
        dungeonBuild = FindAnyObjectByType<DungeonBuilder>();
        ePath = FindAnyObjectByType<EnemyPathfinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomChange)
        {
            LoadCardScene();
        }
    }
}
