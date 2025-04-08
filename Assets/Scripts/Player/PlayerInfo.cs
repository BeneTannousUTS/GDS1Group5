// AUTHOR: BENEDICT
// This script lets the player find their character sprite by enlarging the arrow above their character's head when
// the player presses the left bumper.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfo : MonoBehaviour
{
    public GameObject playerLocateArrow;
    
    PlayerHUD playerHud;
    
    bool infoButtonPressed;
    bool isArrowLarge;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHud = GetComponent<PlayerHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        if (infoButtonPressed && isArrowLarge == false)
        {
            ShowPlayerInfo();
            isArrowLarge = true;
        }
        else if (infoButtonPressed == false && isArrowLarge == true)
        {
            HidePlayerInfo();
            isArrowLarge = false;
        }
    }
    
    //Callback function for button press
    public void OnInfoButtonPressed(InputAction.CallbackContext context)
    {
        infoButtonPressed = context.ReadValueAsButton();
        //Debug.unityLogger.Log(infoButtonPressed);
    }
    
    //Enlarge arrow above player & turn on attack and speed modifier display
    void ShowPlayerInfo()
    {
        playerHud.SetStatsAlpha(255);
        playerLocateArrow.transform.localScale *= 2;
        Gamepad.current.SetMotorSpeeds(1f, 1f);
    }

    //Shrink arrow above player & turn off attack and speed modifier display
    void HidePlayerInfo()
    {
        playerHud.SetStatsAlpha(0);
        playerLocateArrow.transform.localScale /= 2;
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
