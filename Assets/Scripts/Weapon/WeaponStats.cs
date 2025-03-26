// AUTHOR: Alistair
// Handles the storing of weapon stats and destroys the weapon
// after its lifetime is over

using UnityEngine;
using System.Collections;

public class WeaponStats : MonoBehaviour
{
    public float damageValue;
    public float weaponLifetime;
    private string sourceType;
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
        if (projectile != null) 
        {
            GameObject currentProjectile = Instantiate(projectile, transform.position + transform.up, transform.rotation);
            currentProjectile.GetComponent<Projectile>().SetDamageValue(damageValue);
            currentProjectile.GetComponent<Projectile>().SetFriendlyFire(friendlyFire);
            currentProjectile.GetComponent<Projectile>().SetSourceType(sourceType);
        }
        StartCoroutine(DestroyWeapon(weaponLifetime));
        
        transform.position += 0.001f * transform.up;
    }
}
