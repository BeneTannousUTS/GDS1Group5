using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DebugWeaponUI : MonoBehaviour
{
    GameObject selectedPlayer;
    GameManager gameManager;
    public GameObject weapon;
    public List<GameObject> players = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SelectPlayer1()
    {
        selectedPlayer = gameManager.GetPlayerList()[0];
    }

    public void SelectPlayer2()
    {
        selectedPlayer = gameManager.GetPlayerList()[1];
    }

    public void GiveWeapon()
    {
        selectedPlayer.GetComponent<PlayerAttack>().currentWeapon = weapon;
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
