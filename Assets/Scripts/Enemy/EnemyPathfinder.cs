// AUTHOR: Alistair
// This handles interfacing between the enemy AI and the players

using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    [SerializeField] private GameObject[] playerList;

    // Fill playerList with all players in the scene
    void Start()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player"); 
    }

    public GameObject[] GetPlayers()
    {
        return playerList;
    }

    // Returns the position of the closest player
    public GameObject ClosestPlayer(Vector3 enemyPosition) 
    {
        float closestDistance = 1000f;
        GameObject closestPlayer = null;

        foreach (GameObject player in playerList) 
        {
            if (Vector3.Distance(player.transform.position, enemyPosition) < closestDistance) 
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
}
