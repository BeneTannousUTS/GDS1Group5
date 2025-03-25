// AUTHOR: Alistair
// Handles storing a player/enemy's health and checking whether death has occured

using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    private bool isDead = false;

    // Takes damage equal to the damageValue then checks if is dead
    public void TakeDamage(float damageValue) 
    {
        currentHealth -= damageValue;

        if (currentHealth <= 0f) 
        {
            currentHealth = 0f;
            isDead = true;
        }
    }

    // Sets currentHealth to maxHealth
    void Start()
    {
       currentHealth = maxHealth; 
    }
}
