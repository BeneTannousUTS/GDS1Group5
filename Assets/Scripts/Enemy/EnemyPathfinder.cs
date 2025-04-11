// AUTHOR: Alistair
// This handles interfacing between the enemy AI and the players

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    // Fill playerList with all players in the scene
    void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerList.Add(player);
        }
    }

    public List<GameObject> GetPlayers()
    {
        return playerList;
    }

    public void RemovePlayer(GameObject player)
    {
        if (playerList.Contains(player))
        {
            playerList.Remove(player);
        }
    }

    // Returns the position of the closest player
    public GameObject ClosestPlayer(Vector3 enemyPosition)
    {
        float closestDistance = 1000f;
        GameObject closestPlayer = null;

        foreach (GameObject player in playerList)
        {
            if (Vector3.Distance(player.transform.position, enemyPosition) < closestDistance && player.GetComponent<HealthComponent>().GetIsDead() == false && CheckLineOfSight(player.transform.position, player.transform.position - transform.position) == false)
            {
                closestDistance = Vector3.Distance(player.transform.position, enemyPosition);
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    // Returns the position of the furthest player
    public GameObject FurthestPlayer(Vector3 enemyPosition)
    {
        float furthestDistance = 0f;
        GameObject furthestPlayer = null;

        foreach (GameObject player in playerList)
        {
            if (Vector3.Distance(player.transform.position, enemyPosition) > furthestDistance)
            {
                furthestDistance = Vector3.Distance(player.transform.position, enemyPosition);
                furthestPlayer = player;
            }
        }

        return furthestPlayer;
    }

    bool CheckLineOfSight(Vector3 position, Vector3 direction) 
    {
        return Physics2D.Raycast(position, direction, 2f);
    }
}