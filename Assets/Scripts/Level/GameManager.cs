using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    public void Win() 
    {
        Debug.Log("WIN");
    }

    void Lose()
    {
        Debug.Log("LOSE");
    }

    void TraitorWin() 
    {
        Debug.Log("WIN");
    }

    void Start() 
    {
        Debug.Log("AddingPlayers");
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerList.Add(player);
        }
    }

    public void CheckGameState() 
    {
        Debug.Log("CheckGameState");

        bool allDead = true;
        bool traitor = false;

        Debug.Log($"playerListLen: {playerList.Count}");

        foreach (GameObject player in GameObject.FindWithTag("EnemyAISystem").GetComponent<EnemyPathfinder>().GetPlayers())
        {
            traitor = player.CompareTag("Traitor");
            Debug.Log($"PlayerDead: {player.GetComponent<HealthComponent>().GetIsDead()}");
            if (player.GetComponent<HealthComponent>().GetIsDead() == false && player.CompareTag("Traitor") == false) 
            {
                allDead = false;
                break;
            }
        }

        if (allDead && traitor) 
        {
            TraitorWin();
        }
        else if (allDead)
        {
            Lose();
        }
    }
}
