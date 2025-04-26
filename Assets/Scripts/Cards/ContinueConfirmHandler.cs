// AUTHOR: Zac
// Child for continue ui object

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContinueConfirmHandler : BaseConfirmHandler
{
    public TMP_Text promptText;
    public Image playerIcon;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(string prompt, Sprite icon)
    {
        promptText.text = prompt;
        playerIcon.sprite = icon;
    }
}

