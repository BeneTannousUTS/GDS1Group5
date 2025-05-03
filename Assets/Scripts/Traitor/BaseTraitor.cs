using System.Collections.Generic;
using UnityEngine;

public class BaseTraitor : MonoBehaviour
{
    protected float cooldownLength = 10;
    [SerializeField] protected float traitorRoom = 1;
    protected Sprite traitorSprite;
    [SerializeField] protected string amountOfTraitors = "";
    protected TraitorManager traitorManager;
    [SerializeField] protected string traitorName = "";
    [SerializeField] protected string traitorDesc = "";
    [SerializeField] protected string traitorAbilityName = "";
    [SerializeField] protected string traitorAbilityDesc = "";
    private GameObject healthBoost;
    [SerializeField] int traitorID;

    public float GetTraitorRoom()
    {
        return traitorRoom;
    }

    public string GetTraitorName()
    {
        return traitorName;
    }

    public string GetTraitorDesc()
    {
        return traitorDesc;
    }

    public string GetAbilityName()
    {
        return traitorAbilityName;
    }

    public string GetAbilityDesc()
    {
        return traitorAbilityDesc;
    }

    public virtual void TraitorAbility()
    {

    }

    protected void DestroyDoor()
    {
        GameObject finalDoor = GameObject.FindGameObjectWithTag("EscapeDoor");
        if (finalDoor != null)
        {
           GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlaySoundJingle("RoomClear");
            finalDoor.GetComponent<Animator>().SetTrigger("open");
            Destroy(finalDoor, 1.2f);
        }
    }
    public float GetCooldownLength()
    {
        return cooldownLength;
    }

    Sprite GetSprite()
    {
        return traitorSprite;
    }

    public virtual void LoseCondition()
    {

    }

    public int GetTraitorID()
    {
        return traitorID;
    }
    protected void Revive()
    {
        HealthComponent health = gameObject.GetComponent<HealthComponent>();
        if (health.GetIsDead())
        {
            health.Revive();
        }
    }
    void WinCondition()
    {

    }

    public virtual void TraitorSetup()
    {
        gameObject.tag = "Traitor";
        gameObject.transform.position += Vector3.up*13;
        gameObject.GetComponent<PlayerHUD>().SetSecondarySprite(traitorSprite);
        FindAnyObjectByType<EnemyPathfinder>().RemovePlayer(gameObject);
        gameObject.GetComponent<PlayerSecondary>().SetTraitorAbility();
        healthBoost = FindAnyObjectByType<TraitorManager>().GetHealthBoost();
        for (int i = 0; i < GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetPlayerList().Count-1; i++)
        {
            gameObject.GetComponent<PlayerStats>().SetPassive(healthBoost);
        }
        gameObject.GetComponent<HealthComponent>().UpdateHUDHealthBar();
    }

    public string GetAmountOfTraitors()
    {
        return amountOfTraitors;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
