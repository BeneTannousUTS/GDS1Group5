using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
    public TMP_Text popupText;
    public Image popupImage;
    public TMP_Text popupEndText;

    public void SetText(string popupText, Color colour)
    {
        this.popupText.text = popupText;
        this.popupText.color = colour;
    }

    public void SetTextTwo(string popupTextTwo, Color colour)
    {
        this.popupEndText.text = popupTextTwo;
        this.popupText.color = colour;
    }

    public void SetImage(Sprite image)
    {
        this.popupImage.sprite = image;
    }
}
