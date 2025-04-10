using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<GameObject> passives = new List<GameObject>();

    public IEnumerator RemoveTempBuff(float time, GameObject passive) 
    {
        yield return new WaitForSeconds(time);
        passives.Remove(passive);
    }

    public void SetPassive(GameObject passive) 
    {
        //check if card gotten is to revive player, otherwise normal passive effect occurs
        if (passive.GetComponent<PassiveStats>().GetRevivePlayer())
        {
            gameObject.GetComponent<HealthComponent>().Revive();
        }
        else
        {
            passives.Add(passive);
            gameObject.GetComponent<HealthComponent>().SetMaxHealth(GetHealthStat());
        }
    }

    public float GetMoveStat() 
    {
        float modifier = 1f;

        foreach (GameObject passive in passives) 
        {
            modifier *= passive.GetComponent<PassiveStats>().GetMoveMod();
        }

        return modifier;
    }

    public float GetStrengthStat() 
    {
        float modifier = 1f;

        foreach (GameObject passive in passives) 
        {
            modifier *= passive.GetComponent<PassiveStats>().GetStrengthMod();
        }

        return modifier;
    }

    public float GetHealthStat() 
    {
        float modifier = 100f;

        foreach (GameObject passive in passives) 
        {
            modifier += passive.GetComponent<PassiveStats>().GetHealthMod();
        }

        return modifier;
    }
    
    public float GetCooldownStat() 
    {
        float modifier = 1f;

        foreach (GameObject passive in passives) 
        {
            modifier *= passive.GetComponent<PassiveStats>().GetCooldownMod();
        }

        return modifier;
    }
    
    public float GetLifestealStat() 
    {
        float modifier = 1f;

        foreach (GameObject passive in passives) 
        {
            modifier *= passive.GetComponent<PassiveStats>().GetLifestealMod();
        }

        return modifier;
    }
}
