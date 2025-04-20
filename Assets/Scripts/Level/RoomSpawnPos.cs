using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPos : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPos;
    List<GameObject> players;
    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void MovePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = spawnPos[i].transform.position;
        }
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        players = gameManager.GetPlayerList();
        MovePlayers();
    }

}
