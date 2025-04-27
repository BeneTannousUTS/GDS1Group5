using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<GameObject> passives = new List<GameObject>();

    public void public_RemoveTempBuff(float time, GameObject passive)
    {
        StartCoroutine(RemoveTempBuff(time, passive));
    }

    private IEnumerator RemoveTempBuff(float time, GameObject passive) 
    {
        yield return new WaitForSeconds(time);
        passives.Remove(passive);
        Debug.Log("Temp buff time up");
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
        float modifier = 100f;

        foreach (GameObject passive in passives) 
        {
            modifier += passive.GetComponent<PassiveStats>().GetMoveMod();
        }

        return modifier/100;
    }

    public float GetStrengthStat() 
    {
        float modifier = 100f;

        foreach (GameObject passive in passives) 
        {
            modifier += passive.GetComponent<PassiveStats>().GetStrengthMod();
        }

        return modifier/100;
    }

    public float GetHealthStat() 
    {
        float baseHealth = 100f;
        float healthMods = 0;

        foreach (GameObject passive in passives) 
        {
            healthMods += passive.GetComponent<PassiveStats>().GetHealthMod();
        }

        return baseHealth + healthMods;
    }
    
    public float GetCooldownStat() 
    {
        float modifier = 100f;

        foreach (GameObject passive in passives) 
        {
            modifier -= passive.GetComponent<PassiveStats>().GetCooldownMod();
        }

        return modifier/100;
    }

    public float GetLifestealStat()
    {
        float modifier = 0f;

        foreach (GameObject passive in passives)
        {
            modifier += passive.GetComponent<PassiveStats>().GetLifestealMod();
        }

        return modifier/100;
    }

    //Removes all the passives that the player has collected
    public void ResetPassives()
    {
        passives.Clear();
        gameObject.GetComponent<HealthComponent>().ResetPlayerHealth();
    }
    
    public float GetKnockbackStat() 
    {
        float modifier = 100f;

        foreach (GameObject passive in passives) 
        {
            modifier += passive.GetComponent<PassiveStats>().GetKnockbackMod();
        }

        return modifier/100;
    }
}
