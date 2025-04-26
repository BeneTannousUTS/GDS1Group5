using UnityEngine;

public class PassiveStats : MonoBehaviour
{
    [Range(0, 100)]
    [Tooltip("Strength buff amount as a percentage of base weapon damage.")]
    public float strengthMod = 0f; 
    
    [Range(0, 100)] 
    [Tooltip("Speed buff amount as a percentage of base movement speed.")]
    public float moveMod = 0f; 
    
    [Range(0, 100)]
    [Tooltip("Health multiplier as a percentage of base health.")]
    public float healthMod = 0f;
    
    //float used for cooldown multipliers
    [Range(0, 100)]
    [Tooltip("Cooldown reduction amount as a percentage of base cooldown.")]
    public float cooldownMultiplier = 0f;
    
    //float used for cooldown multipliers
    [Range(0, 100)] 
    [Tooltip("Lifesteal amount as a percentage of base weapon damage")]
    public float lifestealMulitplier = 0;
    
    //float used for cooldown multipliers
    [Range(0, 100)]
    [Tooltip("Knockback buff amount as a percentage of base weapon knockback")]
    public float knockbackMult = 0; 
    
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
