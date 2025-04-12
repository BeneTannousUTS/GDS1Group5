// AUTHOR: James
// Handles the traitor debug menu

using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugTraitorUI : MonoBehaviour
{
    List<GameObject> selectedPlayer = new List<GameObject>(4);
    [SerializeField] List<GameObject> playerList;
    GameManager gameManager;
    [SerializeField] TMP_Text playerTxt;
    [SerializeField] TMP_Text countTxt;
    public BaseTraitor traitor;
    List<GameObject> players = new List<GameObject>();
    public TMP_Dropdown dropdown;
    [SerializeField] List<BaseTraitor> traitors;
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

    public void AssignTraitor()
    {
        foreach (GameObject pla in selectedPlayer)
        {
                pla.AddComponent(traitor.GetType());
        }
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

    public void TraitorDropdown()
    {
        traitor = traitors[dropdown.value];
        countTxt.text = "Amount of traitors: " + traitor.GetAmountOfTraitors();
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        foreach (BaseTraitor tra in gameManager.GetTraitorList())
        {
            traitors.Add(tra);
            dropdown.options.Add(new TMP_Dropdown.OptionData(tra.name));
        }
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
