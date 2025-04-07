using UnityEngine;

public class Hammer : WeaponStats
{
    public float knockbackRadius = 3f;
    public float knockbackForce = 10f;

    protected override void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, knockbackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Knockback player
            } else if (collider.CompareTag("Enemy"))
            {
                // Knockback Enemy
            }
        }
        base.TriggerAttack();
    }
}
