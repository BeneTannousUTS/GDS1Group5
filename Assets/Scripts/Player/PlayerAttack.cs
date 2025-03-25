// AUTHOR: Alistair
// Handles player attack input and the spawning of a weapon to hurt enemies

using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject currentWeapon;

    public float attackCooldownWindow;
    private float attackCooldownTimer = 10f;

    public float attackBufferWindow;
    private float attackBufferTimer = 10f;

    // Instantiates a weapon in front of the players current facing direction
    void Attack() 
    {
        Vector3 attackDirection = gameObject.GetComponent<PlayerMovement>().GetFacingDirection();
        Instantiate(currentWeapon, transform.position + attackDirection.normalized, CalculateQuaternion(attackDirection), transform);
        attackCooldownTimer = 0f;
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
        attackBufferTimer += timeIncrease;
        attackCooldownTimer += timeIncrease;
    }

    // Gets player input acts on it if it can
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
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
