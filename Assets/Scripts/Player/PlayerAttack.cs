// AUTHOR: Alistair
// Handles player attack input and the spawning of a weapon to hurt enemies

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject currentWeapon;

    private float attackCooldownWindow;
    private float attackCooldownTimer = 10f;

    public float attackBufferWindow;
    private float attackBufferTimer = 10f;

    private bool attackButtonPressed;

    private AudioManager audioManager;

    // Gets audio manager by tag
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Instantiates a weapon in front of the players current facing direction
    void Attack() 
    {
        attackCooldownWindow = currentWeapon.GetComponent<WeaponStats>().attackCooldownWindow;
        Vector3 attackDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        GameObject tempWeapon = Instantiate(currentWeapon, transform.position + attackDirection, CalculateQuaternion(attackDirection), transform);
        if (attackDirection.x < 0 && tempWeapon.transform.childCount != 0) {
            tempWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
        }
        tempWeapon.GetComponent<WeaponStats>().SetSourceType(gameObject.tag);
        tempWeapon.GetComponent<WeaponStats>().SetDamageMod(gameObject.GetComponent<PlayerStats>().GetStrengthStat());

        // this NEEDS to be changed but atm there are no other ways to determine which weapon is being used
        string audioAttackType = currentWeapon.name.Equals("Sword") ? "PlayerMeleeAttack" : "PlayerRangedAttack"; 
        audioManager.PlaySoundEffect(audioAttackType);

        attackCooldownTimer = 0f;
        
        // Call HUD component function for cooldown animation.
        GetComponent<PlayerHUD>().StartPrimaryCooldownAnim(attackCooldownWindow);
    }

    // Calculates a quaternion which is the rotation needed for the weapon based on direction
    Quaternion CalculateQuaternion(Vector3 direction) 
    {
        float angle = Mathf.Abs((Mathf.Acos(direction.x) * 180)/Mathf.PI);

        if (direction.y >= 0)
        {
            angle -= 90;
        }
        else if (direction.y < 0)
        {
            angle = 270 - angle;
        }

        return Quaternion.Euler(0f, 0f, angle);
    }

    // Updates timers by deltaTime
    void UpdateTimers(float timeIncrease)
    {
        attackBufferTimer += timeIncrease;
        attackCooldownTimer += timeIncrease;
    }

    // Check for unique controller input
    public void OnPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        attackButtonPressed = context.ReadValueAsButton();
    }

    // Gets player input acts on it if it can
    void Update()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            if (attackButtonPressed) 
            {
                attackBufferTimer = 0f;
            }

            if (attackCooldownTimer >= attackCooldownWindow && attackBufferTimer <= attackBufferWindow) 
            {
                Attack();
            }

            UpdateTimers(Time.deltaTime);
        }
    }
}
