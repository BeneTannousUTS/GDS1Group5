// AUTHOR: James
// Handles hazards such as spikes

using UnityEngine;

public class Hazards : MonoBehaviour
{
    private float damageValue;
    private string sourceType;
    protected GameObject sourceObject;
    private bool friendlyFire;
    protected bool roomCleared;
    public float moveSpeed;
    public float knockbackStrength = 0.7f;
    public float knockbackTime = 0.1f;

    // Sets the value of sourceType
    public void SetSourceType(string type) 
    {
        sourceType = type;
    }

    public void SetRoomCleared()
    {
        roomCleared = true;
    }

    public void SetSourceObject(GameObject source) {
        sourceObject = source;
    }

    public GameObject GetSourceObject() {
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

    // Deals damage to a specified HealthComponent & Applies Knockback
    public void DealDamage(HealthComponent healthComponent)
    {
        float preDamageHealth = healthComponent.GetCurrentHealth(); 
        healthComponent.TakeDamage(damageValue);
        if (GetSourceObject()) {
            if (damageValue < 0) {
                Debug.Log("Healing CHECK");
            }
            else {
                if (GetSourceObject().GetComponent<PlayerStats>().GetLifestealStat() > 0) {
                    GetSourceObject().GetComponent<HealthComponent>().TakeDamage(-((preDamageHealth-healthComponent.GetCurrentHealth()) 
                                                                               * GetSourceObject().GetComponent<PlayerStats>().GetLifestealStat()));
                }
                if (healthComponent.GetCurrentHealth() <= 0) {
                }
            }
        }

        if (healthComponent.gameObject.CompareTag("Player") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.position;
                if(GetSourceObject() != null && GetSourceObject().GetComponent<PlayerStats>() != null){
                    healthComponent.gameObject.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackStrength + GetSourceObject().GetComponent<PlayerStats>().GetKnockbackStat(), knockbackTime,knockbackDirection); 
                }

                healthComponent.gameObject.GetComponent<PlayerScore>().IncrementHazardsRanInto();
            }
        } else if (healthComponent.gameObject.CompareTag("Enemy") && damageValue > 0)
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
    }

    bool ColliderType(Collider2D otherCollider) 
    {
        return otherCollider.gameObject.CompareTag("Enemy") == false && otherCollider.gameObject.CompareTag("Weapon") == false && otherCollider.gameObject.CompareTag("TempBuff") == false && otherCollider.gameObject.CompareTag("Player") == false && otherCollider.gameObject.CompareTag("Projectile") == false;
    }
}
