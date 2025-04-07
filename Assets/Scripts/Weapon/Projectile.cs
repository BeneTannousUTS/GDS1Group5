// AUTHOR: Alistair
// Handles projectile movement and dealing damage

using UnityEditor.Callbacks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damageValue;
    private string sourceType;
    private bool friendlyFire;
    public float moveSpeed;
    public float knockbackMultiplier = 0.7f;
    public float knockbackTime = 0.1f;

    private Vector3 shotDirection = Vector3.zero;

    // Sets the projectile's shot direction. Normalizes the direction vector.
    public void SetShotDirection(Vector3 direction)
    {
        shotDirection = direction.normalized;
        transform.up = shotDirection;
    }

    // Sets the value of sourceType
    public void SetSourceType(string type) 
    {
        sourceType = type;
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

    // Deals damage to a specified HealthComponent
    public void DealDamage(HealthComponent healthComponent)
    {
        healthComponent.TakeDamage(damageValue);

        if (healthComponent.gameObject.CompareTag("Player") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.position;
                healthComponent.gameObject.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackMultiplier, knockbackTime,knockbackDirection);
            }
        } else if (healthComponent.gameObject.CompareTag("Enemy") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<EnemyMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.position;
                healthComponent.gameObject.GetComponent<EnemyMovement>().KnockbackEnemy(knockbackMultiplier, knockbackTime,knockbackDirection);
            }
        }
    }

    // Moves projectile
    void Update()
    {   
        transform.position += shotDirection * moveSpeed * Time.deltaTime;
    }

    bool ColliderType(Collider2D otherCollider) 
    {
        return otherCollider.gameObject.CompareTag("Enemy") == false && otherCollider.gameObject.CompareTag("Weapon") == false && otherCollider.gameObject.CompareTag("TempBuff") == false && otherCollider.gameObject.CompareTag("Player") == false && otherCollider.gameObject.CompareTag("Projectile") == false;
    }

    void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if (ColliderType(otherCollider)) 
        {
            Destroy(gameObject);
        }
    }
}
