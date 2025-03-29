// AUTHOR: Alistair
// Handles collision for the players

using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Returns true if friendlyFire is on or if the attack is from an enemy
    bool FriendlyFire(bool friendlyFire, string sourceType) 
    {
        return friendlyFire || gameObject.CompareTag(sourceType) == false;
    }

    // On collision with a weapon or projectile take damage
    void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if (otherCollider.gameObject.CompareTag("Weapon") && FriendlyFire(otherCollider.GetComponent<WeaponStats>().GetFriendlyFire(), otherCollider.GetComponent<WeaponStats>().GetSourceType()))
        {
            otherCollider.GetComponent<WeaponStats>().DealDamage(gameObject.GetComponent<HealthComponent>());
        }
        else if (otherCollider.gameObject.CompareTag("Projectile") && FriendlyFire(otherCollider.GetComponent<Projectile>().GetFriendlyFire(), otherCollider.GetComponent<Projectile>().GetSourceType()))
        {
            otherCollider.GetComponent<Projectile>().DealDamage(gameObject.GetComponent<HealthComponent>());
            Destroy(otherCollider.gameObject);
        }
        else if (otherCollider.gameObject.CompareTag("TempBuff")) 
        {
            gameObject.GetComponent<PlayerStats>().SetPassive(otherCollider.GetComponent<TempBuff>().GetPassive());
        }
    }
}
