// AUTHOR: James
// Handels the clone traitor type

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class OLD_CloneTraitor : MonoBehaviour, ITraitor
{
    private Vector3 spawnPos;
    private bool realTraitor = true;
    private List<GameObject> cloneList = new List<GameObject>();
    private GameObject cloneObject;
    private Sprite abilitySprite;
    private float cooldownLength = 10;
    private float traitorRoom = 1;
    private TraitorManager traitorManager;
    public float getTraitorRoom()
    {
        return traitorRoom;
    }
    public float GetCooldownLength()
    {
        return cooldownLength;
    }
    public int GetMaxHealth()
    {
        throw new System.NotImplementedException();
    }

    public Sprite GetSprite()
    {
        throw new System.NotImplementedException();
    }

    public void TraitorAbility()
    {
        SpawnClones();
    }

    private void SpawnClones()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject clone = Instantiate(cloneObject);
            clone.tag = "Traitor";
            cloneList.Add(clone);
            ClonePosition(clone);
        }
    }

    public void TraitorSetup()
    {
        if (realTraitor)
        {
            gameObject.GetComponent<PlayerHUD>().SetSecondarySprite(abilitySprite);
            FindAnyObjectByType<EnemyPathfinder>().RemovePlayer(gameObject);
            spawnPos = FindAnyObjectByType<DungeonManager>().GetRoomPos();
            ClonePosition(gameObject);
            SpawnClones();
        }
    }

    public void LoseCondition()
    {
        if (realTraitor)
        {
            foreach (GameObject clone in cloneList)
            {
                if (gameObject != clone)
                {
                    Destroy(clone);
                }
            }
        }
    }
    private void OnDestroy()
    {
        LoseCondition();
        Destroy(GameObject.FindGameObjectWithTag("EscapeDoor"));
    }
    private void ClonePosition(GameObject clone)
    {
        bool validPos = false;
        while (!validPos)
        {
            Vector3 checkPos = new Vector3(Random.Range(-14, 14), Random.Range(-6, 5), 0) + spawnPos;
            Collider2D[] hit = Physics2D.OverlapCircleAll(checkPos, 1.5f);
            if (hit.Length == 0)
            {
                validPos = true;
                clone.transform.position = checkPos;
            }
        }
    }

    public void CloneSetup()
    {
        realTraitor = false;
        gameObject.GetComponent<HealthComponent>().SetCurrentHealth(20);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        traitorManager = FindAnyObjectByType<TraitorManager>();
        cloneObject = traitorManager.GetObjectRef(0);
        abilitySprite = traitorManager.GetCardRef(0);
        if (gameObject.GetComponent<HealthComponent>().GetIsDead())
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.tag = "Traitor";
            gameObject.GetComponent<PlayerSecondary>().SetTraitorAbility();
            TraitorSetup();
        }
    }
    //Returns the amount of traitor cards there should be
    public string GetAmountOfTraitors()
    {
        return "single";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void WinCondition()
    {
            
    }
}
