// AUTHOR: Alistair
// Handles enemy attacks based on a cooldown

using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject warningUI;

    public float attackCooldownWindow;
    private float attackCooldownTimer = 0f;

    private bool canAttack = false;
    private AudioManager audioManager;

    // Gets audio manager by tag
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void SetCanAttack(bool inRange) 
    {
        canAttack = inRange;
    }

    // Starts the DoAttack coroutine
    void Attack() 
    {
        StartCoroutine(DoAttack());
    }

    // First shows a warning sign then spawns the weapon attack 0.5 seconds later
    IEnumerator DoAttack() 
    {
        attackCooldownTimer = 0f;
        Vector3 attackDirection = gameObject.GetComponent<EnemyMovement>().GetFacingDirection().normalized;
        Instantiate(warningUI, transform.position + attackDirection, Quaternion.identity, transform);
        gameObject.GetComponent<Animator>().SetTrigger("attack");

        audioManager.PlaySoundEffect("EnemyAttack");

        yield return new WaitForSeconds(0.5f);
        GameObject tempWeapon = Instantiate(currentWeapon, transform.position + attackDirection, CalculateQuaternion(attackDirection), transform);
        tempWeapon.GetComponent<WeaponStats>().SetSourceType(gameObject.tag);
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
        attackCooldownTimer += timeIncrease;
    }

    // Attacks when cooldown is over
    void Update()
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false)
        {
            if (attackCooldownTimer >= attackCooldownWindow && canAttack) 
            {
                Attack();
            }

            UpdateTimers(Time.deltaTime);
        }
    }
}
