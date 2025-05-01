// AUTHOR: Zac
// Child card confirm handler, ui for the swapping of weapons/secondaries

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfirmCardHandler : BaseConfirmHandler
{
    public TMP_Text playerText;
    public Image playerIcon;
    public Image prevCard;
    public Image newCard;
    public TMP_Text yesText;
    public TMP_Text noText;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetupCard(string playerLabel, Sprite icon, Sprite oldAbility, Sprite newAbility)
    {
        playerText.text = playerLabel;
        playerIcon.sprite = icon;
        prevCard.sprite = oldAbility;
        newCard.sprite = newAbility;
    }
}