using System.Collections.Generic;
using UnityEngine;

public class BaseTraitor : MonoBehaviour
{
    [SerializeField] private float cooldownLength = 10;
    [SerializeField] private float traitorRoom = 1;
    [SerializeField] Sprite traitorSprite;
    [SerializeField] string amountOfTraitors = "";
    private TraitorManager traitorManager;

    float getTraitorRoom()
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
    }

    string GetAmountOfTraitors()
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
