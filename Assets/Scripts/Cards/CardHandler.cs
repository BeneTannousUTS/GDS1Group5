// AUTHOR: Alistair
// Handles flipping the cards

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CardHandler : MonoBehaviour
{
    private Sprite frontSprite;

    // Changes the sprite of 
    public IEnumerator ChangeSprite() 
    {
        gameObject.GetComponent<Animator>().SetTrigger("Flip");
        yield return new WaitForSeconds(0.8f);
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Image>().sprite = frontSprite;
    }

    // Sets the value of frontSprite
    public void SetFrontSprite(Sprite sprite) 
    {
        frontSprite = sprite;
    }
}
