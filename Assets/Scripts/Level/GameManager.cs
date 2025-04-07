using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    public void Win() 
    {
        SceneManager.LoadScene("WinScreen");
    }

    public List<GameObject> GetPlayerList()
    {
        return playerList;
    }

    void Lose()
    {
        SceneManager.LoadScene("LoseScreen");
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
        bool allDead = true;
        bool traitor = false;

        foreach (GameObject player in GameObject.FindWithTag("EnemyAISystem").GetComponent<EnemyPathfinder>().GetPlayers())
        {
            traitor = player.CompareTag("Traitor");
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
