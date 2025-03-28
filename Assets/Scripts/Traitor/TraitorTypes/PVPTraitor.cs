using System;
using UnityEngine;

public class PVPTraitor : MonoBehaviour, ITraitor
{
    public int GetMaxHealth()
    {
        throw new System.NotImplementedException();
    }

    public Sprite GetSprite()
    {
        throw new System.NotImplementedException();
    }

    public void LoseCondition()
    {
        throw new NotImplementedException();
    }

    public void TraitorAbility()
    {
        throw new System.NotImplementedException();
    }

    public void TraitorSetup()
    {
        gameObject.tag = "Traitor";
        GetComponent<PlayerAttack>().currentWeapon.GetComponent<WeaponStats>().friendlyFire = true;
        //insert code for moving players into the corners of the rooms
    }

    private void OnDestroy()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Traitor");
        if (temp.Length == 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("EscapeDoor"));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TraitorSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
