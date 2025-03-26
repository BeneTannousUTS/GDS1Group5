// AUTHOR: Alistair
// Handles projectile movement and dealing damage

using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damageValue;
    private string sourceType;
    private bool friendlyFire;
    public float moveSpeed;

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
    }

    // Moves projectile
    void Update()
    {   
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
