// AUTHOR: Alistair
// Handles collision for the enemies

using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    // Returns true if friendlyFire is on or if the attack is from an enemy
    private HashSet<int> processedColliderIDs = new HashSet<int>();
    bool FriendlyFire(bool friendlyFire, string sourceType) 
    {
        return friendlyFire || (gameObject.CompareTag(sourceType) == false && !sourceType.Equals("Traitor"));
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

            if (otherCollider.gameObject.GetComponent<Projectile>().GetComeBack() == false) {
                Destroy(otherCollider.gameObject);
            }
        }
        // Enemy collision with hazards turned off for now
        //else if (otherCollider.gameObject.CompareTag("Hazard") && FriendlyFire(otherCollider.GetComponentInParent<Hazards>().GetFriendlyFire(), otherCollider.GetComponentInParent<Hazards>().GetSourceType()))
        //{
          //  otherCollider.GetComponentInParent<Hazards>().DealDamage(gameObject.GetComponent<HealthComponent>());
        //}
    }

    public void ClearColliderIDs()
    {
        processedColliderIDs.Clear();
    }
}
