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
    public float knockbackStrength = 0.9f;
    public float knockbackTime = 0.1f;
    public bool friendlyFire;
    public bool charge = false;
    public bool canStrafe = false;
    public GameObject projectile;
    public string weaponSound;
    public bool isMelee = true;

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

    // Gets the value of charge
    public bool GetCharge() 
    {
        return charge;
    }

    public bool GetStrafe() 
    {
        return canStrafe;
    }

    protected virtual float GetDamageValue(HealthComponent healthComponent) 
    {
        return damageValue;
    }

    // Deals damage to a specified HealthComponent
    public void DealDamage(HealthComponent healthComponent)
    {
        float preDamageHealth = healthComponent.GetCurrentHealth(); 
        healthComponent.TakeDamage(Mathf.Ceil(GetDamageValue(healthComponent) * damageMod));
        if (GetSourceObject() && GetSourceObject().GetComponent<PlayerScore>()) {
            if (GetDamageValue(healthComponent) < 0) {
                GetSourceObject().GetComponent<PlayerScore>().AddHealing(healthComponent.GetCurrentHealth()-preDamageHealth);
                Debug.Log("Healing CHECK");
            }
            else {
                //GetSourceObject().GetComponent<PlayerScore>().AddDamageDealt(preDamageHealth-healthComponent.GetCurrentHealth());
                if (GetSourceObject().GetComponent<PlayerStats>().GetLifestealStat() > 0) {
                    GetSourceObject().GetComponent<HealthComponent>().TakeDamage(-((preDamageHealth-healthComponent.GetCurrentHealth()) 
                                                                               * GetSourceObject().GetComponent<PlayerStats>().GetLifestealStat()));
                }
                if (healthComponent.GetCurrentHealth() <= 0) {
                    GetSourceObject().GetComponent<PlayerScore>().IncrementKills();
                }
            }
        }


        if (healthComponent.gameObject.CompareTag("Player") && GetDamageValue(healthComponent) > 0)
        {
            if (healthComponent.gameObject.GetComponent<PlayerMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.parent.transform.position;
                if(GetSourceObject() != null && GetSourceObject().GetComponent<PlayerStats>() != null){
                    healthComponent.gameObject.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackStrength + GetSourceObject().GetComponent<PlayerStats>().GetKnockbackStat(), knockbackTime,knockbackDirection); 
                }
            }
        } else if (healthComponent.gameObject.CompareTag("Enemy"))
        {
            if (healthComponent.gameObject.GetComponent<EnemyMovement>() != null)
            {
                Vector3 knockbackDirection = healthComponent.gameObject.transform.position - transform.parent.transform.position;
                if (GetSourceObject() != null && GetSourceObject().GetComponent<PlayerStats>() != null)
                {
                    healthComponent.gameObject.GetComponent<EnemyMovement>().KnockbackEnemy(
                        knockbackStrength + GetSourceObject().GetComponent<PlayerStats>().GetKnockbackStat(), knockbackTime, knockbackDirection);
                }
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
