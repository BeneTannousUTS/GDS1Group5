// AUTHOR: Alistair
// Handles collision for the players

using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool isPlayerPVP = false;
    // Returns true if friendlyFire is on or if the attack is from an enemy
    private HashSet<int> processedColliderIDs = new HashSet<int>();

    public void SetPlayerPVP(bool isPVP) { isPlayerPVP=isPVP; }
    bool FriendlyFire(bool friendlyFire, string sourceType) 
    {
        if (sourceType.Equals("Enemy") && gameObject.CompareTag("Traitor"))
        {
            return false;
        }
        return friendlyFire || isPlayerPVP || gameObject.CompareTag(sourceType) == false;
    }

    // On collision with a weapon or projectile take damage
    void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        int colliderID = otherCollider.GetInstanceID();
        if (processedColliderIDs.Contains(colliderID)) return;
        if (!otherCollider.CompareTag("Hazard") && !(otherCollider.GetComponent<Projectile>() != null && otherCollider.GetComponent<Projectile>().GetComeBack()))
        {
            processedColliderIDs.Add(colliderID);
        }
        if (gameObject.GetComponent<HealthComponent>().GetIsDead() == false) 
        {
            if (otherCollider.gameObject.CompareTag("Weapon") && FriendlyFire(otherCollider.GetComponent<WeaponStats>().GetFriendlyFire(), otherCollider.GetComponent<WeaponStats>().GetSourceType()))
            {
                otherCollider.GetComponent<WeaponStats>().DealDamage(gameObject.GetComponent<HealthComponent>());
            }
            else if (otherCollider.gameObject.CompareTag("Projectile") && FriendlyFire(otherCollider.GetComponent<Projectile>().GetFriendlyFire(), otherCollider.GetComponent<Projectile>().GetSourceType()))
            {
                otherCollider.GetComponent<Projectile>().DealDamage(gameObject.GetComponent<HealthComponent>());
                if (otherCollider.GetComponent<Explosion>() != null)
                {
                    otherCollider.GetComponent<Explosion>().SpawnExplosion();
                }
                else if (otherCollider.GetComponent<PolyProjectile>())
                {
                    otherCollider.GetComponent<PolyProjectile>().ApplyMorph(gameObject);
                }
                if (otherCollider.gameObject.GetComponent<Projectile>().GetComeBack() == false) {
                    Destroy(otherCollider.gameObject);
                }
                Destroy(otherCollider.gameObject);
            }
            else if (otherCollider.gameObject.CompareTag("Hazard") && FriendlyFire(otherCollider.GetComponentInParent<Hazards>().GetFriendlyFire(), otherCollider.GetComponentInParent<Hazards>().GetSourceType()))
            {
                otherCollider.GetComponentInParent<Hazards>().DealDamage(gameObject.GetComponent<HealthComponent>());
            }
            else if (otherCollider.gameObject.CompareTag("TempBuff")) 
            {
                GameObject passive = otherCollider.GetComponent<TempBuff>().GetPassive();
                gameObject.GetComponent<PlayerStats>().SetPassive(passive);
                gameObject.GetComponent<PlayerStats>().public_RemoveTempBuff(otherCollider.GetComponent<TempBuff>().GetBuffTime(), passive);
            }
        }
    }

    public void ClearColliderIDs()
    {
        processedColliderIDs.Clear();
    }
}
