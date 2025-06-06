using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<GameObject> passives = new List<GameObject>();
    [SerializeField] private List<string> temp_buffs = new List<string>();
    public GameObject strengthParticles;
    public GameObject speedParticles;

    public void public_RemoveTempBuff(float time, GameObject passive, string type = "None")
    {
        StartCoroutine(RemoveTempBuff(time, passive, type));
    }

    private IEnumerator RemoveTempBuff(float time, GameObject passive, string type) 
    {
        yield return new WaitForSeconds(time);
        passives.Remove(passive);
        if (temp_buffs.Contains(type)) {
            temp_buffs.Remove(type);
        }
        Debug.Log("Temp buff time up");
    }

    public void SetPassive(GameObject passive, string type = "None") 
    {
        //check if card gotten is to revive player, otherwise normal passive effect occurs
        if (passive.GetComponent<PassiveStats>().GetRevivePlayer())
        {
            gameObject.GetComponent<HealthComponent>().Revive();
        }
        else
        {
            if (type.Equals("None") || !temp_buffs.Contains(type)) {
                passives.Add(passive);
                if (type.Equals("Speed")) {
                    Instantiate(speedParticles, transform);
                    temp_buffs.Add(type);
                }
                else if (type.Equals("Strength")) {
                    Instantiate(strengthParticles, transform);
                    temp_buffs.Add(type);
                }
            }
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
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null) return 100f;

        int numberOfPlayers = gameManager.GetPlayerList().Count;

        float baseHealth = 200 - (numberOfPlayers - 2) * 50;
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
