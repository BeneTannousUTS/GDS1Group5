// AUTHOR: Alistair
// Handles player secondary input and the use of that secondary

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSecondary : MonoBehaviour
{
    public GameObject currentSecondary;
    private GameObject previousSecondary;

    private float secondaryCooldownWindow;
    private float secondaryCooldownTimer = 10f;

    public float secondaryBufferWindow;
    private float secondaryBufferTimer = 10f;

    private bool secondaryButtonPressed;

    private bool hasTraitorAbility = false;

    // Instantiates a secondary at the player's position
    void Secondary() 
    {
        if (hasTraitorAbility)
        {
            gameObject.GetComponent<BaseTraitor>().TraitorAbility();
        }
        else
        {
            Vector3 secondaryDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
            GameObject tempSecondary = Instantiate(currentSecondary, transform.position, Quaternion.identity);
            tempSecondary.GetComponent<SecondaryStats>().SetSourceType(gameObject.tag);
            tempSecondary.GetComponent<SecondaryStats>().SetSourceObject(gameObject);
        }
        secondaryCooldownTimer = 0f;
        
        // Call HUD component function for cooldown animation.
        GetComponent<PlayerHUD>().StartSecondaryCooldownAnim(secondaryCooldownWindow);
    }

    // Updates timers by deltaTime
    void UpdateTimers(float timeIncrease)
    {
        secondaryBufferTimer += timeIncrease;
        secondaryCooldownTimer += timeIncrease;
    }
    
    // Check for unique controller input
    public void OnSecondaryButtonPressed(InputAction.CallbackContext context)
    {
        secondaryButtonPressed = context.ReadValueAsButton();
    }

    public void SetTraitorAbility()
    {
        hasTraitorAbility = true;
        secondaryCooldownWindow = gameObject.GetComponent<BaseTraitor>().GetCooldownLength();
    }

    private void Start()
    {
        previousSecondary = currentSecondary;
        secondaryCooldownWindow = currentSecondary.GetComponent<ISecondary>().GetCooldownLength();
    }

    // Gets player input acts on it if it can
    void Update()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            if (previousSecondary != currentSecondary)
            {
                previousSecondary = currentSecondary;
                secondaryCooldownWindow = currentSecondary.GetComponent<ISecondary>().GetCooldownLength();
            }
            if (secondaryButtonPressed)
            {
                secondaryBufferTimer = 0f;
            }

            if (secondaryCooldownTimer >= secondaryCooldownWindow && secondaryBufferTimer <= secondaryBufferWindow) 
            {
                Secondary();
            }

            UpdateTimers(Time.deltaTime);
        }
    }
}
