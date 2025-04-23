using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        int numOfTraitors = gameManager.traitorManager.GetTraitorAmount();

        if (!(collision.tag.Equals("Player") || collision.tag.Equals("Traitor")))
        {
            return;
        }

        if (collision.GetComponent<HealthComponent>().GetIsDead())
        {
            return;
        }

        if (numOfTraitors == 1 && collision.tag.Equals("Player"))
        {
            List<GameObject> playerObjects = gameManager.GetPlayerList();

            foreach (GameObject playerObject in playerObjects)
            {
                if (playerObject.tag.Equals("Player"))
                {
                    playerObject.GetComponent<PlayerScore>().SetWonGame();
                }
            }
        } 
        else if (numOfTraitors > 1 && collision.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerScore>().SetWonGame();
        }
        else if (collision.tag.Equals("Traitor"))
        {
            collision.gameObject.GetComponent<PlayerScore>().SetWonGame();
        }

        gameManager.Win();
    }
}
