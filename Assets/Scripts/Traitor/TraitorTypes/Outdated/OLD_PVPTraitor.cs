// AUTHOR: James
// Handles the pvp traitor type

using System;
using UnityEngine;

public class OLD_PVPTraitor : MonoBehaviour, ITraitor
{
    private float cooldownLength = 10;
    private float traitorRoom = 1;
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

    public float getTraitorRoom()
    {
        return traitorRoom;
    }

    public void LoseCondition()
    {
        throw new NotImplementedException();
    }

    public void TraitorAbility()
    {
        
    }

    public void TraitorSetup()
    {
        gameObject.tag = "Traitor";
        gameObject.GetComponent<PlayerCollision>().SetPlayerPVP(true);
        //gameObject.GetComponent<PlayerSecondary>().SetTraitorAbility();
        //insert code for moving players into the corners of the rooms
    }

    public void WinCondition()
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Traitor");
        if (temp.Length == 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("EscapeDoor"));
        }
    }
    //Returns the amount of traitor cards there should be
    public string GetAmountOfTraitors()
    {
        return "everyone";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead())
        {
            Destroy(gameObject);
        }
        else
        {
            TraitorSetup();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
