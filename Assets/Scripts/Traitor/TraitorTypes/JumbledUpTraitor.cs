using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumbledUpTraitor : BaseTraitor
{
    private Vector3 summonPos;
    private List<GameObject> summonList = new List<GameObject>();
    private GameObject summonObject;
    List<GameObject> weaponStats = new List<GameObject>();
    List<RuntimeAnimatorController> animCon = new List<RuntimeAnimatorController>();
    List<GameObject> playerList = new List<GameObject>();
    public override void TraitorAbility()
    {
        SummonPosition(gameObject);
        foreach (GameObject player in playerList)
        {
            SummonPosition(player);
        }
        SummonEnemies();
    }

    public override void TraitorSetup()
    {
        summonPos = FindAnyObjectByType<DungeonManager>().GetRoomPos();
        foreach (GameObject player in GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList())
        {
            playerList.Add(player);
            animCon.Add(player.GetComponent<Animator>().runtimeAnimatorController);
            weaponStats.Add(player.GetComponent<PlayerAttack>().currentWeapon);
            player.GetComponent<PlayerInfo>().playerLocateArrow.SetActive(false);
            SummonPosition(player);
        }
        SummonEnemies();
        base.TraitorSetup();
    }

    private void SummonPosition(GameObject summon)
    {
        bool validPos = false;
        float exitTime = 0;
        while (!validPos)
        {
            exitTime++;
            Vector3 checkPos = new Vector3(Random.Range(-14f, 14f), Random.Range(-6, 5), 0) + summonPos;
            Collider2D[] hit = Physics2D.OverlapCircleAll(checkPos, 1.5f, LayerMask.GetMask("Default"));
            if (hit.Length == 0 || exitTime == 100)
            {
                validPos = true;
                summon.transform.position = checkPos;
                summon.GetComponent<Animator>().runtimeAnimatorController = gameObject.GetComponent<Animator>().runtimeAnimatorController;
            }
        }
    }

    private void SummonEnemies()
    {
        for (int i = 0; i < weaponStats.Count*3; i++)
        {
            GameObject summon = Instantiate(summonObject);
            summon.GetComponent<EnemyAttack>().currentWeapon = weaponStats[Random.Range(0, (weaponStats.Count))];
            summonList.Add(summon);
            SummonPosition(summon);
        }
    }

    public override void LoseCondition()
    {
        foreach (GameObject sum in summonList)
        {
            if (gameObject != sum)
            {
                Destroy(sum);
            }
        }
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] != null)
            {
                playerList[i].GetComponent<Animator>().runtimeAnimatorController = animCon[i];
            }
        }
        DestroyDoor();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revive();
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 10;
        traitorSprite = traitorManager.GetCardRef(0);
        summonObject = traitorManager.GetObjectRef(2);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
