using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugSecondaryUI : MonoBehaviour
{
    GameObject selectedPlayer;
    GameManager gameManager;
    [SerializeField] TMP_Text giveBtn;
    public GameObject secondary;
    List<GameObject> players = new List<GameObject>();
    public TMP_Dropdown dropdown;
    [SerializeField] GameObject[] secondaries;
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

    public void GiveWeapon()
    {
        if (selectedPlayer != null)
        {
            selectedPlayer.GetComponent<PlayerSecondary>().currentSecondary = secondary.GetComponent<Card>().abilityObject;
            selectedPlayer.GetComponent<PlayerHUD>().SetSecondarySprite(secondary.GetComponent<Card>().cardFrontSprite);
        }
    }

    public void WeaponDropdown()
    {
        int pickedEntry = dropdown.value;
        Debug.Log(pickedEntry);
        secondary = secondaries[pickedEntry];
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        foreach (GameObject sec in secondaries)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(sec.name));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
