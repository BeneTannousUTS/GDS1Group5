// AUTHOR: James
// Interface for all traitor scripts


using UnityEngine;

public interface ITraitor 
{
    void TraitorAbility();

    Sprite GetSprite();

    int GetMaxHealth();

    void LoseCondition();

    void TraitorSetup();
}
