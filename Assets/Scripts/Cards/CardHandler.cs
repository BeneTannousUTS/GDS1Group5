// AUTHOR: Alistair/Zac
// Handles flipping the cards

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class CardHandler : MonoBehaviour
{
    [SerializeField]
    Card card;
    [SerializeField]
    Image playerIcon;
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

    public void SwapCard(Sprite newSprite, GameObject newAbility)
    {
        card.cardFrontSprite = newSprite;
        gameObject.GetComponent<Image>().sprite = card.cardFrontSprite;

        card.abilityObject = newAbility;
    }

    public void showPlayerIcon(Sprite playerSprite)
    {
        playerIcon.color = new Vector4(1,1,1,1);
        playerIcon.sprite = playerSprite;
    }

    public void showNameAsType()
    {
        nameText.GetComponent<TMP_Text>().text = card.cardType.ToString() + " Card";
        nameText.SetActive(true);
    }

    public void showNameAsCard()
    {
        nameText.GetComponent<TMP_Text>().text = card.cardName;
        nameText.SetActive(true);
    }

    public void showDesc()
    {
        descriptionText.GetComponent<TMP_Text>().text = card.cardDescription;
        descriptionText.SetActive(true);
    }

    public void setTraitorText()
    {
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
}
