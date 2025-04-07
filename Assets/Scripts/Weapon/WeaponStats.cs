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
    public float attackCooldownWindow;
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
        healthComponent.TakeDamage(damageValue * damageMod);
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
