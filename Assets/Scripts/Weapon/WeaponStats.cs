// AUTHOR: Alistair
// Handles the storing of weapon stats and destroys the weapon
// after its lifetime is over

using UnityEngine;
using System.Collections;

public class WeaponStats : MonoBehaviour
{
    public float damageValue;
    public float weaponLifetime;

    // Deals damage to a specified HealthComponent
    public void DealDamage(HealthComponent healthComponent)
    {
        healthComponent.TakeDamage(damageValue);
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
        StartCoroutine(DestroyWeapon(weaponLifetime));
    }
}
