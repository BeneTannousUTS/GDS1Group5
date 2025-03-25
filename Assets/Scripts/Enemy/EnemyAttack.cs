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
    
    // Starts the DoAttack coroutine
    void Attack() 
    {
        StartCoroutine(DoAttack());
    }

    // First shows a warning sign then spawns the weapon attack 0.5 seconds later
    IEnumerator DoAttack() 
    {
        attackCooldownTimer = 0f;
        Vector3 attackDirection = gameObject.GetComponent<EnemyMovement>().GetFacingDirection();
        Instantiate(warningUI, transform.position + attackDirection, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(currentWeapon, transform.position + attackDirection.normalized, CalculateQuaternion(attackDirection), transform);
    }

    // Calculates a quaternion which is the rotation needed for the weapon based on direction
    Quaternion CalculateQuaternion(Vector3 direction) 
    {
        float angle;

        if (direction.y == 1)
        {
            if (direction.x == 1) 
            {
                angle = 315f;
            }
            else if (direction.x == -1) 
            {
                angle = 45f;
            }
            else 
            {
                angle = 0f;
            }
        }

        else if (direction.y == -1) 
        {
            if (direction.x == 1) 
            {
                angle = 225f;
            }
            else if (direction.x == -1) 
            {
                angle = 135f;
            }
            else 
            {
                angle = 180f;
            }
        }

        else {
            if (direction.x == 1) 
            {
                angle = 270f;
            }
            else if (direction.x == -1) 
            {
                angle = 90f;
            }
            else 
            {
                angle = 0f;
            }
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
        if (attackCooldownTimer >= attackCooldownWindow) 
        {
            Attack();
        }

        UpdateTimers(Time.deltaTime);
    }
}
