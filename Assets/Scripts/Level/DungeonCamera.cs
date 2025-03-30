// AUTHOR: James
// Handles moving the camera when moving between dungeon rooms

using System;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    private DungeonBuilder dungeonBuild;
    private float roomChangeTimer;
    private bool roomChange = false;
    private EnemyPathfinder ePath;
    // Moves the camera to the position of the room that the player has just entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            roomChange = true;
        }
    }

    void MoveCamera(Camera cam)
    {
        cam.transform.position = transform.parent.transform.position + new Vector3(0, 25, -10);
        dungeonBuild.ActivateRooms((int)(transform.position.y / 25));
    }

    public void RoomChangeTime(Camera cam)
    {
        foreach (GameObject player in ePath.GetPlayers())
        {
            player.SetActive(true);
            player.transform.position = gameObject.transform.position + Vector3.up;
        }
        MoveCamera(cam);
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
                player.SetActive(false);
            }

            roomChange = false;

            FindAnyObjectByType<CardManager>().CardSceneCoroutine(this);
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
            if (dungeonBuild.GetNumRooms() == (int)(transform.position.y / 25))
            {
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Win();
            }
            else 
            {
                LoadCardScene();
            }
        }
    }
}
