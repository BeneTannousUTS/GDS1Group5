// AUTHOR: Alistair
// Handles projectile movement and dealing damage

using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damageValue;
    private string sourceType;
    protected GameObject sourceObject;
    private bool friendlyFire;
    public float moveSpeed;
    public float knockbackStrength = 0.7f;
    public float knockbackTime = 0.1f;
    private float comeBackTimer = 0f;
    public bool comeBack = false;
    public float comeBackWindow = 0.5f;
    private float frozenTimer = 3f;

    private Vector3 shotDirection = Vector3.zero;

    // Sets the projectile's shot direction. Normalizes the direction vector.
    public void SetShotDirection(Vector3 direction)
    {
        shotDirection = direction.normalized;
        transform.up = shotDirection;
    }

    public Vector3 GetShotDirection()
    {
        return shotDirection;
    }

    // Sets the value of sourceType
    public void SetSourceType(string type)
    {
        sourceType = type;
    }

    public void SetSourceObject(GameObject source)
    {
        sourceObject = source;
    }

    public GameObject GetSourceObject()
    {
        return sourceObject;
    }

    // Sets the value of sourceType
    public void SetFriendlyFire(bool friend)
    {
        friendlyFire = friend;
    }

    // Gets the value of sourceType
    public string GetSourceType()
    {
        return sourceType;
    }

    // Gets the value of sourceType
    public bool GetFriendlyFire()
    {
        return friendlyFire;
    }

    // Sets the value of damageValue
    public void SetDamageValue(float damage)
    {
        damageValue = damage;
    }

    // Gets the value of comeback
    public bool GetComeBack()
    {
        return comeBack;
    }

    // Deals damage to a specified HealthComponent & Applies Knockback
    public void DealDamage(HealthComponent healthComponent)
    {
        float preDamageHealth = healthComponent.GetCurrentHealth();
        healthComponent.TakeDamage(damageValue);
        if (GetSourceObject() && GetSourceObject().GetComponent<PlayerScore>())
        {
            if (damageValue < 0)
            {
                GetSourceObject().GetComponent<PlayerScore>().AddHealing(healthComponent.GetCurrentHealth() - preDamageHealth);
                Debug.Log("Healing CHECK");
            }
            else
            {
                //GetSourceObject().GetComponent<PlayerScore>().AddDamageDealt(preDamageHealth-healthComponent.GetCurrentHealth());
                GetSourceObject().GetComponent<PlayerScore>().IncrementProjectilesHit();
                if (GetSourceObject().GetComponent<PlayerStats>().GetLifestealStat() > 0)
                {
                    GetSourceObject().GetComponent<HealthComponent>().TakeDamage(-((preDamageHealth - healthComponent.GetCurrentHealth())
                                                                               * GetSourceObject().GetComponent<PlayerStats>().GetLifestealStat()));
                }
                if (healthComponent.GetCurrentHealth() <= 0)
                {
                    GetSourceObject().GetComponent<PlayerScore>().IncrementKills();
                }
            }
        }

        if (healthComponent.gameObject.CompareTag("Player") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.position;
                if (GetSourceObject() != null && GetSourceObject().GetComponent<PlayerStats>() != null)
                {
                    healthComponent.gameObject.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackStrength + GetSourceObject().GetComponent<PlayerStats>().GetKnockbackStat(), knockbackTime, knockbackDirection);
                }
            }
        }
        else if (healthComponent.gameObject.CompareTag("Enemy") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<EnemyMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.position;
                if (GetSourceObject() != null && GetSourceObject().GetComponent<PlayerStats>() != null)
                {
                    healthComponent.gameObject.GetComponent<EnemyMovement>().KnockbackEnemy(
                        knockbackStrength + GetSourceObject().GetComponent<PlayerStats>().GetKnockbackStat(), knockbackTime, knockbackDirection);
                }
            }
        }
    }

    // Moves projectile
    void Update()
    {
        frozenTimer += Time.deltaTime;
        if (frozenTimer >= 3f)
        {
            if (gameObject.GetComponent<SpriteRenderer>().material.HasInt("_Invert"))
            {
                if (gameObject.GetComponent<SpriteRenderer>().material.GetInt("_Invert") == 1) // bad solution but better than setting it every frame
                {
                    gameObject.GetComponent<SpriteRenderer>().material.SetInt("_Invert", 0);
                    if (gameObject.GetComponent<Animator>() != null)
                    {
                        gameObject.GetComponent<Animator>().speed = 1;
                    }
                }
            }

            comeBackTimer += Time.deltaTime;
            if (comeBack == false || comeBackTimer < comeBackWindow)
            {
                transform.position += shotDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                // If projectile is meant to return to the player it will so after a period of time otherwise it will not destroy and can hit enemies multiple times
                if (Vector3.Distance(transform.position, sourceObject.transform.position) <= 0.1f)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, sourceObject.transform.position, moveSpeed * 3f * Time.deltaTime);
                }
            }
        }
    }

    bool ColliderType(Collider2D otherCollider)
    {
        return otherCollider.gameObject.CompareTag("Enemy") == false && otherCollider.gameObject.CompareTag("Weapon") == false && otherCollider.gameObject.CompareTag("TempBuff") == false && otherCollider.gameObject.CompareTag("Player") == false && otherCollider.gameObject.CompareTag("Projectile") == false && otherCollider.gameObject.CompareTag("Hazard") == false && otherCollider.gameObject.CompareTag("PressurePlate") == false && otherCollider.gameObject.CompareTag("Coin") == false && otherCollider.gameObject.CompareTag("Destructible") == false;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (ColliderType(otherCollider) && comeBack == false)
        {
            if (gameObject.GetComponent<Explosion>() != null)
            {
                gameObject.GetComponent<Explosion>().SpawnExplosion();
            }
            Destroy(gameObject);
        }
    }


    public void SetFrozen()
    {
        frozenTimer = 0f;
        gameObject.GetComponent<SpriteRenderer>().material.SetInt("_Invert", 1);
        if (gameObject.GetComponent<Animator>() != null)
        {
            gameObject.GetComponent<Animator>().speed = 0;
        }
    }
}
