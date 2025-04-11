// AUTHOR: James
// Handles the weapons debug menu
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugWeaponUI : MonoBehaviour
{
    List<GameObject> selectedPlayer = new List<GameObject>(4);
    [SerializeField] List<GameObject> playerList;
    GameManager gameManager;
    [SerializeField] TMP_Text playerTxt;
    public GameObject weapon;
    List<GameObject> players = new List<GameObject>();
    public TMP_Dropdown dropdown;
    [SerializeField] GameObject[] weapons;
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
            selectPlay = selectPlay + "P" + (playerList.IndexOf(player) + 1) + " ";
        }
        playerTxt.text = selectPlay;

    }

    public void GiveWeapon()
    {
        foreach (GameObject player in selectedPlayer)
        {
            player.GetComponent<PlayerAttack>().currentWeapon = weapon.GetComponent<Card>().abilityObject;
            player.GetComponent<PlayerHUD>().SetPrimarySprite(weapon.GetComponent<Card>().cardFrontSprite);
        }
    }

    public void WeaponDropdown()
    {
        weapon = weapons[dropdown.value];
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        foreach (GameObject weapon in weapons)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(weapon.name));
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
