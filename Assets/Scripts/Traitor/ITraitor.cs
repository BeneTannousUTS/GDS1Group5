// AUTHOR: James
// Interface for all traitor scripts


using UnityEngine;

public interface ITraitor 
{
    float getTraitorRoom();

    void TraitorAbility();

    public float GetCooldownLength();

    Sprite GetSprite();

    int GetMaxHealth();

    void LoseCondition();

    void WinCondition();

    void TraitorSetup();
}
