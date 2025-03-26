// AUTHOR: Alistair
// Handles storing a player/enemy's health and checking whether death has occured

using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth;
    public float invicibilityFrameTime;
    private bool invincible = false;
    private float currentHealth;
    private bool isDead = false;

    public bool GetIsDead() 
    {
        return isDead;
    }

    IEnumerator DamageFlash() 
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator DoInvincibilityFrames(float time) 
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    IEnumerator Die() 
    {
        isDead = true;
        yield return new WaitForSeconds(0.5f);
        if (gameObject.CompareTag("Player") == false) {
            Destroy(gameObject);
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    // Takes damage equal to the damageValue then checks if is dead
    public void TakeDamage(float damageValue) 
    {
        if (invincible == false) 
        {
            currentHealth -= damageValue;

            StartCoroutine(DamageFlash());
            StartCoroutine(DoInvincibilityFrames(invicibilityFrameTime));

            if (currentHealth <= 0f) 
            {
                currentHealth = 0f;
                StartCoroutine(Die());
            }
        }
    }

    // Sets currentHealth to maxHealth
    void Start()
    {
       currentHealth = maxHealth; 
    }
}
