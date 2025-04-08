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

    public void setArrowIcon(Sprite playerArrowSprite)
    {
        if (playerArrowSprite != null)
        {
            playerArrowIcon.sprite = playerArrowSprite;
        }
    }

    public void showNameAsType()
    {
        nameText.GetComponent<TMP_Text>().text = "New " + "\n" + card.cardType.ToString();
        nameText.SetActive(true);
    }

    public void showNameAsCard()
    {
        nameText.GetComponent<TMP_Text>().color = new Vector4(1,1,1,1);
        nameText.GetComponent<TMP_Text>().text = card.cardName;
        nameText.SetActive(true);
    }

    public void showDesc()
    {
        descriptionText.GetComponent<TMP_Text>().text = card.cardDescription;
        descriptionText.SetActive(true);
    }

    public void setTraitorCard(Sprite traitorSprite)
    {
        card.cardFrontSprite = traitorSprite;
        gameObject.GetComponent<Image>().sprite = card.cardFrontSprite;
        card.abilityObject = null;
        card.cardName = "Traitor";
        card.cardDescription = "You are a traitor... Fight your once friends.";
    }

    public void ReplaceCard(Card replacementCard)
    {
        card.cardName = replacementCard.cardName;
        card.cardDescription = replacementCard.cardDescription;
        card.cardFrontSprite = replacementCard.cardFrontSprite;
        card.abilityObject = replacementCard.abilityObject;
    }

    public void OnSelect(BaseEventData eventData)
    {
        playerArrowIcon.color = new Vector4(1,1,1,1);
        nameText.GetComponent<TMP_Text>().color = new Vector4(1,1,1,1);
        playerArrowIcon.gameObject.GetComponent<Animator>().enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        playerArrowIcon.color = new Vector4(1,1,1,0);
        nameText.GetComponent<TMP_Text>().color = new Vector4(0.5f,0.5f,0.5f,1);
        playerArrowIcon.gameObject.GetComponent<Animator>().enabled = false;
    }
}
