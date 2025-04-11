using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugCheatsUI : MonoBehaviour
{
    List<GameObject> selectedPlayer = new List<GameObject>(4);
    [SerializeField] List<GameObject> playerList;
    GameManager gameManager;
    [SerializeField] TMP_Text playerTxt;
    List<GameObject> players = new List<GameObject>();
    public TMP_Dropdown dropdown;
    public TMP_Dropdown worldDropdown;
    [SerializeField] List<BaseTraitor> traitors;
    private int playerCheatSelection;
    private int worldCheatSelection;
    [SerializeField] GameObject[] instakills;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SelectPlayer1()
    {
        if (selectedPlayer.Contains(playerList[0]))
        {
            selectedPlayer.Remove(playerList[0]);
        }
        else
        {
            selectedPlayer.Add(playerList[0]);
        }
        UpdatePlayersTxt();
    }

    public void SelectPlayer2()
    {
        if (selectedPlayer.Contains(playerList[1]))
        {
            selectedPlayer.Remove(playerList[1]);
        }
        else
        {
            selectedPlayer.Add(playerList[1]);
        }
        UpdatePlayersTxt();
    }
    public void SelectPlayer3()
    {
        if (playerList.Count > 2)
        {
            if (selectedPlayer.Contains(playerList[2]))
            {
                selectedPlayer.Remove(playerList[2]);
            }
            else
            {
                selectedPlayer.Add(playerList[2]);
            }
        }
        UpdatePlayersTxt();
    }
    public void SelectPlayer4()
    {
        if (playerList.Count > 3)
        {
            if (selectedPlayer.Contains(playerList[3]))
            {
                selectedPlayer.Remove(playerList[3]);
            }
            else
            {
                selectedPlayer.Add(playerList[3]);
            }
        }
        UpdatePlayersTxt();
    }

    private void UpdatePlayersTxt()
    {
        string selectPlay = "Selected Players: ";
        foreach (GameObject player in selectedPlayer)
        {
            selectPlay = selectPlay + "P" + (playerList.IndexOf(player)+1) + " ";
        }
        playerTxt.text = selectPlay;

    }

    public void CheatDropdown()
    {
        playerCheatSelection = dropdown.value;
    }

    public void WorldCheatDropdown()
    {
        worldCheatSelection = worldDropdown.value;
    }

    public void ApplyPlayerCheat()
    {
        foreach (GameObject player in selectedPlayer)
        {
            if (playerCheatSelection == 0)
            {
                player.GetComponent<HealthComponent>().ToggleInvincible();
            }
            else if (playerCheatSelection == 1)
            {
                if (player.GetComponent<PlayerStats>().GetStrengthStat() >= 1000)
                {
                    player.GetComponent<PlayerStats>().SetPassive(instakills[1]);
                }
                else
                {
                    player.GetComponent<PlayerStats>().SetPassive(instakills[0]);
                }
                
            }
        }
    }

    public void ApplyWorldCheats()
    {
        if (worldCheatSelection == 0)
        {
            gameManager.dungeonManager.SetAutoUnlock();
        }
        if (worldCheatSelection == 1)
        {
            gameManager.dungeonManager.SetAutoDefeat();
        }
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        dropdown.value = -1;
        foreach (GameObject pla in gameManager.GetPlayerList())
        {
            playerList.Add(pla);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
