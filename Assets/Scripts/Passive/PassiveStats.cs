using UnityEngine;

public class PassiveStats : MonoBehaviour
{
    public float strengthMod = 1f;
    public float moveMod = 1f;
    public float healthMod = 0f;
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
}
