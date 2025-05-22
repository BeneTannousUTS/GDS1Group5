using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedsterTraitor : BaseTraitor
{
    List<GameObject> playerList = new List<GameObject>();
    public GameObject passive;
    public override void TraitorAbility()
    {
        base.TraitorAbility();
        foreach (GameObject player in playerList)
        {
            if (player != gameObject)
            {
                player.GetComponent<PlayerStats>().SetPassive(passive);
                Debug.Log("AAA");
            }
        }
    }

    public override void TraitorSetup()
    {
        base.TraitorSetup();
    }


    public override void LoseCondition()
    {
        DestroyDoor();
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revive();
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 12;
        traitorSprite = traitorManager.GetCardRef(0);
        passive = traitorManager.GetObjectRef(4);
        playerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = traitorManager.GetAnim(7);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
