using UnityEngine;

public class CloneTraitor : MonoBehaviour, ITraitor
{
    private Vector3 spawnPos;
    private bool realTraitor = true;
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
        foreach (GameObject clone in GameObject.FindGameObjectsWithTag("Traitor"))
        {
            ClonePosition(clone);
        }
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            ClonePosition(player);
        }
    }

    public void TraitorSetup()
    {
        if (realTraitor)
        {
            spawnPos = FindAnyObjectByType<DungeonManager>().GetRoomPos();
            ClonePosition(gameObject);
            for (int i = 0; i < 10; i++)
            {
                GameObject clone = Instantiate(gameObject);
                clone.GetComponent<CloneTraitor>().CloneSetup();
                ClonePosition(clone);
            }
        }
    }

    public void LoseCondition()
    {
        if (realTraitor)
        {
            foreach (GameObject clone in GameObject.FindGameObjectsWithTag("Traitor"))
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
