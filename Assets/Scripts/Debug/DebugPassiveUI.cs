using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPassiveUI : MonoBehaviour
{
    GameObject selectedPlayer;
    GameManager gameManager;
    [SerializeField] TMP_Text giveBtn;
    public GameObject passive;
    List<GameObject> players = new List<GameObject>();
    public TMP_Dropdown dropdown;
    [SerializeField] GameObject[] passives;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SelectPlayer1()
    {
        selectedPlayer = gameManager.GetPlayerList()[0];
        giveBtn.text = "Give Player 1";
    }

    public void SelectPlayer2()
    {
        selectedPlayer = gameManager.GetPlayerList()[1];
        giveBtn.text = "Give Player 2";
    }
    public void SelectPlayer3()
    {
        if (gameManager.GetPlayerList().Count > 2)
        {
            selectedPlayer = gameManager.GetPlayerList()[2];
            giveBtn.text = "Give Player 3";
        }
    }
    public void SelectPlayer4()
    {
        if (gameManager.GetPlayerList().Count > 3)
        {
            selectedPlayer = gameManager.GetPlayerList()[3];
            giveBtn.text = "Give Player 4";
        }
    }

    public void GivePassive()
    {
        if (selectedPlayer != null)
        {
            selectedPlayer.GetComponent<PlayerStats>().SetPassive(passive.GetComponent<Card>().abilityObject);
            selectedPlayer.GetComponent<PlayerHUD>().UpdateStatsDisplay();
            selectedPlayer.GetComponent<HealthComponent>().UpdateHUDHealthBar();
        }
    }

    public void ResetPassive()
    {
        if (selectedPlayer != null)
        {
            selectedPlayer.GetComponent<PlayerStats>().ResetPassives();
            selectedPlayer.GetComponent<PlayerHUD>().UpdateStatsDisplay();
            selectedPlayer.GetComponent<HealthComponent>().UpdateHUDHealthBar();
        }
    }

    public void PassiveDropdown()
    {
        passive = passives[dropdown.value];
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        foreach (GameObject sec in passives)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(sec.name));
        }
        dropdown.value = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
