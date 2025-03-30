// AUTHOR: Alistair
// Handles player secondary input and the use of that secondary

using UnityEngine;

public class PlayerSecondary : MonoBehaviour
{
    public GameObject currentSecondary;

    public float secondaryCooldownWindow;
    private float secondaryCooldownTimer = 10f;

    public float secondaryBufferWindow;
    private float secondaryBufferTimer = 10f;

    // Instantiates a secondary at the player's position
    void Secondary() 
    {
        Vector3 secondaryDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        GameObject tempSecondary = Instantiate(currentSecondary, transform.position, Quaternion.identity);
        tempSecondary.GetComponent<SecondaryStats>().SetSourceType(gameObject.tag);
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

    // Gets player input acts on it if it can
    void Update()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            if (Input.GetButtonDown("Fire2"))
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
