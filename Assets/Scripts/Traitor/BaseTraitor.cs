using System.Collections.Generic;
using UnityEngine;

public class BaseTraitor : MonoBehaviour
{
    protected float cooldownLength = 10;
    [SerializeField] protected float traitorRoom = 1;
    protected Sprite traitorSprite;
    [SerializeField] protected string amountOfTraitors = "";
    protected TraitorManager traitorManager;

    public float getTraitorRoom()
    {
        return traitorRoom;
    }

    public virtual void TraitorAbility()
    {

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

    void WinCondition()
    {

    }

    public virtual void TraitorSetup()
    {
        gameObject.tag = "Traitor";
        gameObject.GetComponent<PlayerHUD>().SetSecondarySprite(traitorSprite);
        FindAnyObjectByType<EnemyPathfinder>().RemovePlayer(gameObject);
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
