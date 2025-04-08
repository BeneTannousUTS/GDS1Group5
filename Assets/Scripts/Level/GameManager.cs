using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    public DungeonManager dungeonManager;
    public CardManager cardManager;
    public TraitorManager traitorManager;
    [SerializeField] private List<ITraitor> traitorTypeList = new List<ITraitor>();
    private ITraitor currentTraitorType;

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

    void AddTraitorTypes() 
    {
        traitorTypeList.Add(new CloneTraitor());
        traitorTypeList.Add(new PVPTraitor());
    }

    void DecideTraitor() 
    {
        AddTraitorTypes();

        currentTraitorType = traitorTypeList[Random.Range(1, traitorTypeList.Count)];
        traitorManager.SetTraitorType(currentTraitorType);
        cardManager.SetTraitorType(currentTraitorType);
    }

    public void ShowCardSelection(DungeonCamera lastDunCam)
    {
        Debug.Log($"traitorNum: {traitorManager.GetTraitorAmount()}");
        if (traitorManager.CheckTraitorAppear())
        {
            cardManager.ShowCardSelection(lastDunCam, traitorManager.GetTraitorAmount());
        }
        else 
        {
            cardManager.ShowCardSelection(lastDunCam, 0);
        }
    }

    void Start() 
    {
        playerList = GameObject.FindWithTag("EnemyAISystem").GetComponent<EnemyPathfinder>().GetPlayers();

        DecideTraitor();
    }

    public void CheckGameState() 
    {
        bool allDead = true;
        bool traitor = false;

        foreach (GameObject player in playerList)
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
