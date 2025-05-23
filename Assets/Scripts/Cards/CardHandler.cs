// AUTHOR: Alistair/Zac
// Handles flipping the cards

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class CardHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    Card card;
    [SerializeField]
    Image playerIcon;
    [SerializeField]
    Image playerArrowIcon;
    [SerializeField]
    GameObject nameText;
    [SerializeField]
    GameObject descriptionText;
    Color playerColour = Color.white;

    public IEnumerator ChangeSprite() 
    {
        gameObject.GetComponent<Animator>().SetTrigger("Flip");
        yield return new WaitForSeconds(0.8f);
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Image>().sprite = card.cardFrontSprite;
    }

    public void showPlayerIcon(Sprite playerSprite)
    {
        playerIcon.color = new Vector4(1,1,1,1);
        playerIcon.sprite = playerSprite;  
    }

    public void setArrowIcon(Color playerColour)
    {
        this.playerColour = playerColour;
    }

    public void showNameAsType()
    {
        nameText.GetComponent<TMP_Text>().color = FindAnyObjectByType<CardSelection>().GetColourFromRarity(card.cardRarity) * new Vector4(1f,1f,1f,0.7f);
        nameText.GetComponent<TMP_Text>().text = card.cardType.ToString();
        nameText.SetActive(true);
    }

    public void showNameAsCard()
    {
        nameText.GetComponent<TMP_Text>().color = FindAnyObjectByType<CardSelection>().GetColourFromRarity(card.cardRarity);
        nameText.GetComponent<TMP_Text>().text = card.cardName;
        nameText.SetActive(true);
    }

    public void showDesc()
    {
        descriptionText.GetComponent<TMP_Text>().color = FindAnyObjectByType<CardSelection>().GetColourFromRarity(card.cardRarity) * new Vector4(1f,1f,1f,0.7f);
        descriptionText.GetComponent<TMP_Text>().text = card.cardDescription;
        descriptionText.SetActive(true);
    }

    public void setTraitorCard(Sprite traitorSprite)
    {
        card.cardFrontSprite = traitorSprite;
        gameObject.GetComponent<Image>().sprite = card.cardFrontSprite;
        gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        card.cardType = CardType.Passive;
        card.cardRarity = CardRarity.Traitor;
        card.abilityObject = null;
        card.cardName = "Traitor";
        card.cardDescription = "You are a traitor...";
    }

    public void ReplaceCard(Card replacementCard)
    {
        card.cardName = replacementCard.cardName;
        card.cardType = replacementCard.cardType;
        card.cardRarity = replacementCard.cardRarity;
        card.cardDescription = replacementCard.cardDescription;
        card.cardFrontSprite = replacementCard.cardFrontSprite;
        card.abilityObject = replacementCard.abilityObject;
    }

    public void OnSelect(BaseEventData eventData)
    {
        playerArrowIcon.color = playerColour;
        nameText.GetComponent<TMP_Text>().color = FindAnyObjectByType<CardSelection>().GetColourFromRarity(card.cardRarity) * new Vector4(1,1,1,1);
        playerArrowIcon.gameObject.GetComponent<Animator>().enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        playerArrowIcon.color = new Vector4(1,1,1,0);
        nameText.GetComponent<TMP_Text>().color = FindAnyObjectByType<CardSelection>().GetColourFromRarity(card.cardRarity) * new Vector4(0.5f,0.5f,0.5f,1);
        playerArrowIcon.gameObject.GetComponent<Animator>().enabled = false;
    }
}
