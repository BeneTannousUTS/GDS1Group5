using Unity.Services.Lobbies.Models;
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
                if (collider.gameObject == transform.parent.gameObject)
                {
                    collider.GetComponent<PlayerMovement>().KnockbackPlayer(0, weaponLifetime + 0.05f, Vector3.zero);
                } else
                {
                    Vector3 knockbackDirection = collider.transform.position - transform.position;
                    collider.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackStrength, knockbackTime,knockbackDirection);
                }
            }
        }
        base.TriggerAttack();
    }
}
