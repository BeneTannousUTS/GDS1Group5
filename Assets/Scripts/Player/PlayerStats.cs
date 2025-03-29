using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    private List<GameObject> passives = new List<GameObject>();

    public IEnumerator removeTempBuff(float time, GameObject passive) 
    {
        yield return new WaitForSeconds(time);
        passives.Remove(passive);
    }

    public void SetPassive(GameObject passive) 
    {
        passives.Add(passive);
        gameObject.GetComponent<HealthComponent>().SetMaxHealth(GetHealthStat());
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
}
