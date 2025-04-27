// AUTHOR: Benedict
// Handles the unique logic needed for the lobby room

using System.Collections.Generic;
using UnityEngine;

public class StartRoomManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    public GameObject[] playerPrefabs;
    private bool openDoor;
    private bool doorBeingDestroyed = false;
    private float checkTimer;
    private AudioManager audioManager;
    private DungeonManager dungeonManager;
    
    
    
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        dungeonManager = FindAnyObjectByType<DungeonManager>();
        FindAnyObjectByType<GameSceneManager>().AssignPlayerPrefabs(playerPrefabs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
