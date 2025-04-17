// AUTHOR: Alistair
// Handles player attack input and the spawning of a weapon to hurt enemies

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject ghostWeapon;

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
    void Attack(GameObject weapon) 
    {
        attackCooldownWindow = weapon.GetComponent<WeaponStats>().attackCooldownWindow * GetComponent<PlayerStats>().GetCooldownStat();
        attackBufferWindow *= GetComponent<PlayerStats>().GetCooldownStat();
        Vector3 attackDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        GameObject tempWeapon = Instantiate(weapon, transform.position + attackDirection, CalculateQuaternion(attackDirection), transform);
        if (attackDirection.x < 0 && tempWeapon.transform.childCount != 0) {
            tempWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
        }
        tempWeapon.GetComponent<WeaponStats>().SetSourceType(gameObject.tag);
        tempWeapon.GetComponent<WeaponStats>().SetSourceObject(gameObject);
        tempWeapon.GetComponent<WeaponStats>().SetDamageMod(gameObject.GetComponent<PlayerStats>().GetStrengthStat());

        // this NEEDS to be changed but atm there are no other ways to determine which weapon is being used

        if (weapon.GetComponent<WeaponStats>().projectile == null) // is melee
        {
            audioManager.PlaySoundEffect("PlayerMeleeAttack");
            gameObject.GetComponent<Animator>().SetTrigger("attack");
        } else
        {
            audioManager.PlaySoundEffect("PlayerRangedAttack");
            gameObject.GetComponent<Animator>().SetTrigger("range");
        }
        
        attackCooldownTimer = 0f;

        
        
        // Call HUD component function for cooldown animation.
        GetComponent<PlayerHUD>().StartPrimaryCooldownAnim(attackCooldownWindow * gameObject.GetComponent<PlayerStats>().GetCooldownStat());
    }

    // Calculates a quaternion which is the rotation needed for the weapon based on direction
    public Quaternion CalculateQuaternion(Vector3 direction) 
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

            if (attackCooldownTimer >= attackCooldownWindow * gameObject.GetComponent<PlayerStats>().GetCooldownStat() && attackBufferTimer <= attackBufferWindow) 
            {
                Attack(currentWeapon);
            }

            UpdateTimers(Time.deltaTime);
        }
        else
        {
            if (attackButtonPressed) 
            {
                attackBufferTimer = 0f;
            }

            if (attackCooldownTimer >= attackCooldownWindow * gameObject.GetComponent<PlayerStats>().GetCooldownStat() && attackBufferTimer <= attackBufferWindow) 
            {
                Attack(ghostWeapon);
            }

            UpdateTimers(Time.deltaTime);
        }
    }
}
