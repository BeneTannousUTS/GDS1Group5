using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartRoomCamera : MonoBehaviour
{
    [SerializeField] DungeonCamera lastDunCam;
    
    private float roomChangeTimer;
    bool shouldCheckForRoomChange = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject StartRoomManager = FindAnyObjectByType<StartRoomManager>().gameObject;
        if (collision.gameObject.CompareTag("Player") && !StartRoomManager.GetComponent<StartRoomManager>().roomChange)
        {
            GameObject player = collision.gameObject;
            StartRoomManager.GetComponent<StartRoomManager>().HidePlayer(player);
            StartRoomManager.GetComponent<StartRoomManager>().roomChange = true;
            
        }else if(collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            FindAnyObjectByType<StartRoomManager>().HidePlayer(player);
        }
    }
    
    void LoadGameScene()
    {
        roomChangeTimer += Time.deltaTime;
        if (roomChangeTimer > 2)
        {
            SceneManager.LoadScene("GameScene");

            Debug.Log("Scene Load Requested");
        }
    }

    private void Update()
    {
        if (shouldCheckForRoomChange && SceneManager.GetActiveScene().name == "GameScene")
        {
            Debug.Log("Game Scene Loaded!");

            lastDunCam.enabled = true;
            lastDunCam.ReinitializeComponents();
            
            shouldCheckForRoomChange = false;
            ShowCardSelect();
        }
    }

    private void ShowCardSelect()
    {
        foreach (GameObject player in lastDunCam.GetPathfinder().GetPlayers())
        {
            FindAnyObjectByType<CardManager>().HidePlayer(player);
        }

        FindAnyObjectByType<GameManager>().ShowCardSelection(lastDunCam);
        Debug.Log("WARIO");
    }
}
