// AUTHOR: Alistair
// Handles collision for the players

using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool isPlayerPVP = false;
    // Returns true if friendlyFire is on or if the attack is from an enemy
    public void SetPlayerPVP(bool isPVP) { isPlayerPVP=isPVP; }
    bool FriendlyFire(bool friendlyFire, string sourceType) 
    {
        return friendlyFire || gameObject.CompareTag(sourceType) == false || isPlayerPVP;
    }

    // On collision with a weapon or projectile take damage
    void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false) 
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
                gameObject.GetComponent<PlayerStats>().RemoveTempBuff(otherCollider.GetComponent<TempBuff>().GetBuffTime(), otherCollider.GetComponent<TempBuff>().GetPassive());
            }
        }
    }
}
