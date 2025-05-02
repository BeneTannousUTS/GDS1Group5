// AUTHOR: ZAC
// Mostly a storage object to create card prefabs

using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    public CardType cardType;
    [SerializeField]
    public string cardName;
    [SerializeField]
    public string cardDescription;
    [SerializeField]
    public CardRarity cardRarity;
    // [SerializeField]
    // public int cardLevel = 1;
    [SerializeField]
    public Sprite cardFrontSprite;
    [SerializeField]
    Sprite cardBackSprite;
    [SerializeField]
    public GameObject abilityObject;
}
