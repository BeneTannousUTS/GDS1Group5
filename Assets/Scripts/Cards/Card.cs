// AUTHOR: ZAC
// Mostly a storage object to create card prefabs

using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    CardType cardType;
    [SerializeField]
    public string cardName;
    [SerializeField]
    public string cardDescription;
    [SerializeField]
    public Sprite cardFrontSprite;
    [SerializeField]
    Sprite cardBackSprite;
    [SerializeField]
    public GameObject abilityObject;

    public string GetCardName()
    {
        return cardName;
    }

    public string GetCardDesc()
    {
        return cardDescription;
    }

    public string GetCardType()
    {
        return cardType.ToString();
    }
}
