// AUTHOR: Alistair
// Handles the storing of weapon stats and destroys the weapon
// after its lifetime is over

using UnityEngine;
using System.Collections;

public class WeaponStats : MonoBehaviour
{
    public float damageValue;
    protected float damageMod = 1f;
    public float weaponLifetime;
    protected string sourceType;
    protected GameObject sourceObject;
    public float attackCooldownWindow;
    public float knockbackMultiplier = 0.9f;
    public float knockbackTime = 0.1f;
    public bool friendlyFire;
    public GameObject projectile;

    // Sets the value of sourceType
    public void SetSourceType(string type)
    {
        sourceType = type;
    }

    // Gets the value of sourceType
    public string GetSourceType()
    {
        return sourceType;
    }

    public void SetSourceObject(GameObject source) {
        sourceObject = source;
    }

    public GameObject GetSourceObject() {
        return sourceObject;
    }

    // Gets the value of sourceType
    public bool GetFriendlyFire()
    {
        return friendlyFire;
    }

    // Sets the value of sourceType
    public void SetDamageMod(float modifier)
    {
        damageMod = modifier;
    }

    // Deals damage to a specified HealthComponent
    public void DealDamage(HealthComponent healthComponent)
    {
        float preDamageHealth = healthComponent.GetCurrentHealth(); 
        healthComponent.TakeDamage(damageValue * damageMod);
        if (GetSourceObject() && GetSourceObject().GetComponent<PlayerScore>()) {
            if (damageValue < 0) {
                GetSourceObject().GetComponent<PlayerScore>().AddHealing(healthComponent.GetCurrentHealth()-preDamageHealth);
                Debug.Log("Healing CHECK");
            }
            else {
                GetSourceObject().GetComponent<PlayerScore>().AddDamageDealt(preDamageHealth-healthComponent.GetCurrentHealth());
                if (healthComponent.GetCurrentHealth() <= 0) {
                    GetSourceObject().GetComponent<PlayerScore>().IncrementKills();
                }
            }
        }


        if (healthComponent.gameObject.CompareTag("Player") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.parent.transform.position;
                healthComponent.gameObject.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackMultiplier, knockbackTime,knockbackDirection);
            }
        } else if (healthComponent.gameObject.CompareTag("Enemy") && damageValue > 0)
        {
            if (healthComponent.gameObject.GetComponent<EnemyMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.parent.transform.position;
                healthComponent.gameObject.GetComponent<EnemyMovement>().KnockbackEnemy(knockbackMultiplier, knockbackTime,knockbackDirection);
            }
        }
    }

    // Destroys the weapon after its lifetime is up
    IEnumerator DestroyWeapon(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    // Starts the DestroyWeapon coroutine
    void Start()
    {
        TriggerAttack();
    }

    protected virtual void TriggerAttack()
    {
        Debug.Log($"sourceType: {sourceType}");

        StartCoroutine(DestroyWeapon(weaponLifetime));

        transform.position += 0.001f * transform.up;
    }
}
