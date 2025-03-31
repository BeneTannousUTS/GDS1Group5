// AUTHOR: Alistair
// Handles flipping the cards

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CardHandler : MonoBehaviour
{
    [SerializeField]
    Card card;

    // Changes the sprite of 
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
}
