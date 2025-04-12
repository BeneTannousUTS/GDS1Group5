using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class SummonerTraitor : BaseTraitor
{
    private Vector3 summonPos;
    private List<GameObject> summonList = new List<GameObject>();
    private GameObject summonObject;
    public override void TraitorAbility()
    {
        SummonEnemies();
    }

    public override void TraitorSetup()
    {
        base.TraitorSetup();
        gameObject.GetComponent<PlayerCollision>().SetPlayerPVP(true);
        summonPos = FindAnyObjectByType<DungeonManager>().GetRoomPos();
        SummonPosition(gameObject);
        SummonEnemies();
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
            }
        }
    }

    private void SummonEnemies()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject summon = Instantiate(summonObject);
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
        DestroyDoor();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Revive();
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cooldownLength = 10;
        traitorSprite = traitorManager.GetCardRef(0);
        summonObject = traitorManager.GetObjectRef(0);
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
