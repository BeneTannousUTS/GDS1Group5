using System.Collections.Generic;
using UnityEngine;

public class PVPTraitor : BaseTraitor
{

    GameObject passive;

    public override void TraitorAbility()
    {
        base.TraitorAbility();
        gameObject.GetComponent<HealthComponent>().TakeDamage(15);
        gameObject.GetComponent<PlayerStats>().SetPassive(passive);
        gameObject.GetComponent<PlayerStats>().public_RemoveTempBuff(6, passive);
    }

    public override void TraitorSetup()
    {
        gameObject.tag = "Traitor";
        FindAnyObjectByType<EnemyPathfinder>().RemovePlayer(gameObject);
        gameObject.GetComponent<PlayerCollision>().SetPlayerPVP(true);
    }

    public override void LoseCondition()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Traitor");
        if (temp.Length == 2)
        {
            DestroyDoor();
        }
        gameObject.SetActive(false);
    }

    void ChangeAnim()
    {
        List<GameObject> playerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList();
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].gameObject.GetComponent<Animator>().runtimeAnimatorController = traitorManager.GetAnim(i);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revive();
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 10;
        passive = traitorManager.GetObjectRef(1);
        ChangeAnim();
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
