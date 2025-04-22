// AUTHOR: James
// Handles each room functions such as opening door

using System.Collections.Generic;
using UnityEngine;

public class StartRoomManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    private bool openDoor;
    private bool doorBeingDestroyed = false;
    private float checkTimer;
    private AudioManager audioManager;
    private DungeonManager dungeonManager;
    
    
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
