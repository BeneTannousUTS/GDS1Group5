// AUTHOR: BENEDICT
// This script ensures all spawn points have been loaded in the scene before spawning players

using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    
    GameSceneManager gameSceneManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();

        //gameSceneManager.SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
