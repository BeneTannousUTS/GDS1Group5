using UnityEngine;

public class Hammer : WeaponStats
{
    public float friendlyKnockbackRadius = 2f;

    protected override void TriggerAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, friendlyKnockbackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector3 knockbackDirection = collider.transform.position - transform.position;
                collider.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackMultiplier, knockbackTime,knockbackDirection);
            }
        }
        base.TriggerAttack();
    }
}
