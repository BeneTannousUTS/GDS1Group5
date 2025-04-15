using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ConfirmCardHandler : MonoBehaviour
{
    public bool hasConfirmed = false;
    public bool? confirmedChoice = null;
    public TMP_Text playerText;
    public Image playerIcon;
    public Image prevCard;
    public Image newCard;
    public TMP_Text yesText;
    public TMP_Text noText;
    public int playerIndex;
    public PlayerInput assignedInput = null;
}
