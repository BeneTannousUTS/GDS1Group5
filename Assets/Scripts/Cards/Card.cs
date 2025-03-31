using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    CardType cardType;
    [SerializeField]
    string cardName;
    [SerializeField]
    string cardDescription;
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
}
