// AUTHOR: Zac
// Child obj for ready check UI handler (only seen in traitor canvas)

using UnityEngine;
using UnityEngine.UI;

public class ReadyCheckHandler : BaseConfirmHandler
{
    public Image playerIcon;
    public Toggle toggle;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Setup(Sprite icon)
    {
        playerIcon.sprite = icon;
        toggle.isOn = false;
    }
}