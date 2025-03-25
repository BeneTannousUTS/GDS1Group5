// AUTHOR: Alistair
// Handles collision for the players

using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // On collision with a weapon take damage
    void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if (otherCollider.gameObject.CompareTag("Weapon"))
        {
            otherCollider.GetComponent<WeaponStats>().DealDamage(gameObject.GetComponent<HealthComponent>());
        }
    }
}
