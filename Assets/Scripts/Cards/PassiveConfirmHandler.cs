using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveConfirmHandler : MonoBehaviour
{
    public TMP_Text playerText;
    public Image playerIcon;
    public TMP_Text passiveNameText;
    public Image passiveCard;
    public TMP_Text prevMultText;
    public TMP_Text newMultText;

    public void SetupCard(string playerLabel, Color playerColour, Sprite icon, string passiveName, Sprite passiveCard, string prevMult, string newMult)
    {
        playerText.text = playerLabel;
        playerText.color = playerColour;
        playerIcon.sprite = icon;

        passiveNameText.text = passiveName;
        this.passiveCard.sprite = passiveCard;
        prevMultText.text = prevMult;
        newMultText.text = newMult;
    }
}
