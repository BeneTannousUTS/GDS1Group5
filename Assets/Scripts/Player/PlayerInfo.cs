// AUTHOR: BENEDICT
// This script lets the player find their character sprite by enlarging the arrow above their character's head when
// the player presses the left bumper.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfo : MonoBehaviour
{
    public GameObject playerLocateArrow;
    
    PlayerHUD playerHud;
    VibrationManager vibrationManager;
    
    bool infoButtonPressed;
    bool isArrowLarge;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHud = GetComponent<PlayerHUD>();
        vibrationManager = GetComponent<VibrationManager>();
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

        ActivateHeartbeatCheck();
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
        playerHud.SetStatsFilling();
        playerLocateArrow.transform.localScale *= 2;

    }

    //Shrink arrow above player & turn off attack and speed modifier display
    void HidePlayerInfo()
    {
        playerHud.SetStatsUnfilling();
        playerLocateArrow.transform.localScale /= 2;
    }
    
    //Check to see if player is less than 50% HP and activate heartbeat if so
    void ActivateHeartbeatCheck()
    {
        float current = gameObject.GetComponent<HealthComponent>().GetCurrentHealth();
        float max = gameObject.GetComponent<HealthComponent>().maxHealth;

        if (current <= max / 2)
        {
            vibrationManager.StartHeartbeatVibration(GetComponent<PlayerInput>().GetDevice<Gamepad>(), 1-current/(max/2));
        }
        else if (current > max / 2 || current == 0)
        {
            vibrationManager.StopHeartbeatVibration();
        }
    }
}
