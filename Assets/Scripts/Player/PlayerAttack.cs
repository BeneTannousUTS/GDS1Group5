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
    public void Attack(GameObject weapon) 
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag.Equals("Weapon") && child.GetComponent<DualWield>() == null)
            {
                Destroy(child.gameObject);
            }
        }

        GetComponent<PlayerScore>().IncrementWeaponActivated();
        attackCooldownWindow = weapon.GetComponent<WeaponStats>().attackCooldownWindow * GetComponent<PlayerStats>().GetCooldownStat();
        attackBufferWindow *= GetComponent<PlayerStats>().GetCooldownStat();
        Vector3 attackDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection().normalized;
        bool isMelee = weapon.GetComponent<WeaponStats>().projectile == null;

        if (!isMelee) GetComponent<PlayerScore>().IncrementProjectilesShot();

        float weaponTypeMod = isMelee ? 1.5f : 0.7f;
        GameObject tempWeapon = Instantiate(weapon, transform.position + attackDirection * weaponTypeMod, CalculateQuaternion(attackDirection), transform);
        if (attackDirection.x < 0 && !isMelee && tempWeapon.transform.childCount != 0) {
            tempWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
        }

        if (attackDirection.y < 0) {
            if (tempWeapon.transform.childCount != 0)
            {
                tempWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
            } else
            {
                tempWeapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            
            if (isMelee)
            {
                tempWeapon.transform.position = tempWeapon.transform.position + attackDirection * 0.3f;
            } else
            {
                tempWeapon.transform.position = tempWeapon.transform.position + attackDirection * 0.3f;
            }
        }

        tempWeapon.GetComponent<WeaponStats>().SetSourceType(gameObject.tag);
        if (tempWeapon.transform.childCount != 0 && tempWeapon.transform.GetChild(0).GetComponent<WeaponStats>()) {
            tempWeapon.transform.GetChild(0).GetComponent<WeaponStats>().SetSourceType(gameObject.tag);
        }
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

        if (tempWeapon.GetComponent<WeaponStats>().GetCharge()) {
            gameObject.GetComponent<PlayerMovement>().StartDash();
        }
        
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
        else {
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
