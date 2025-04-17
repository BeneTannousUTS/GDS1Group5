using UnityEngine;
using TMPro;

public class PopupHandler : MonoBehaviour
{
    public TMP_Text popupText;

    public void SetText(string popupText, Color colour)
    {
        this.popupText.text = popupText;
        this.popupText.color = colour;
    }
}
