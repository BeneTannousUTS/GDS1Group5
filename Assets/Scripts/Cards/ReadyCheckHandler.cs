using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReadyCheckHandler : MonoBehaviour
{
    public bool hasConfirmed = false;
    public Image playerIcon;
    public Toggle toggle;
    public PlayerInput assignedInput;
    public int playerIndex;
}
