using UnityEngine;

public class PassiveStats : MonoBehaviour
{
    public float strengthMod = 0f; [Range(0, 100)]
    public float moveMod = 0f; [Range(0, 100)]
    public float healthMod = 0f; [Range(0, 100)]
    
    //float used for cooldown multipliers
    public float cooldownMultiplier = 0f; [Range(0, 100)]
    
    //float used for cooldown multipliers
    public float lifestealMulitplier = 0; [Range(0, 100)]
    
    //float used for cooldown multipliers
    public float knockbackMult = 0; [Range(0, 100)]
    
    //bool used for revive player
    public bool revivePlayer;

    public float GetStrengthMod()
    {
        return strengthMod;
    }

    public float GetMoveMod()
    {
        return moveMod;
    }

    public float GetHealthMod()
    {
        return healthMod;
    }
    public bool GetRevivePlayer()
    {
        return revivePlayer;
    }
    
    public float GetCooldownMod()
    {
        return cooldownMultiplier;
    }

    public float GetLifestealMod()
    {
        return lifestealMulitplier;
    }

    public float GetKnockbackMod()
    {
        return knockbackMult;
    }
}
