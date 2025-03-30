// AUTHOR: James
// Handels the clone traitor type

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CloneTraitor : MonoBehaviour, ITraitor
{
    private Vector3 spawnPos;
    private bool realTraitor = true;
    private List<GameObject> cloneList = new List<GameObject>();
    [SerializeField] GameObject cloneObject;
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
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
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
        int test = 0;
        while (!validPos)
        {
            test++;
            Vector3 checkPos = new Vector3(Random.Range(-19, 19), Random.Range(-9, 9), 0) + spawnPos;
            Collider2D[] hit = Physics2D.OverlapCircleAll(checkPos, 1f);
            if (hit.Length == 0)
            {
                validPos = true;
                clone.transform.position = checkPos;
            }
            if (test > 10)
            {
                validPos = true;
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
        gameObject.tag = "Traitor";
        //TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TraitorSetup();
        }
        if (realTraitor && Input.GetKeyDown(KeyCode.P))
        {
            Destroy(gameObject);
        }
    }
}
