using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    public DungeonManager dungeonManager;
    public CardManager cardManager;
    public TraitorManager traitorManager;
    [SerializeField] private List<BaseTraitor> traitorTypeList = new List<BaseTraitor>();
    private BaseTraitor currentTraitorType;
    public ResultsManager resultsManager;

    public void Win(GameObject winner) 
    {
        resultsManager.GetPlayerScores();
        if (winner.CompareTag("Traitor")) 
        {
            TraitorWin();
        }
        else {
            SceneManager.LoadScene("WinScreen");
        }
    }

    public List<GameObject> GetPlayerList()
    {
        return playerList;
    }

    public List<BaseTraitor> GetTraitorList()
    {
        return traitorTypeList;
    }

    public void AddPlayer(GameObject player) 
    {
        playerList.Add(player);
    }

    void Lose()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    void TraitorWin() 
    {
        SceneManager.LoadScene("TraitorWinScreen");
    }

    void AddTraitorTypes() 
    {
        //traitorTypeList.Add(new PVPTest());
        //traitorTypeList.Add(new PVPTest());
        //traitorTypeList.Add(new PVPTraitor());
    }

    void DecideTraitor() 
    {
        AddTraitorTypes();

        currentTraitorType = traitorTypeList[Random.Range(1, traitorTypeList.Count)];
        traitorManager.SetTraitorType(currentTraitorType);
        cardManager.SetTraitorType(currentTraitorType);
    }

    public void DebugSetTraitor(BaseTraitor traitor)
    {
        currentTraitorType = traitor;
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
        DecideTraitor();
    }

    public void CheckGameState() 
    {
        bool allDead = true;

        foreach (GameObject player in playerList)
        {
            if (player.GetComponent<HealthComponent>().GetIsDead() == false) 
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            resultsManager.GetPlayerScores();
            Lose();
        }
    }
}
