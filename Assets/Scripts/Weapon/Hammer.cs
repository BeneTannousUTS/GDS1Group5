using System.Collections;
using UnityEngine;

public class Hammer : WeaponStats
{
    public float friendlyKnockbackRadius = 2f;

    protected override void TriggerAttack()
    {
        GetSourceObject().GetComponent<PlayerMovement>().KnockbackPlayer(0, weaponLifetime, Vector3.zero);
        StartCoroutine(FriendlyKnockback());
        base.TriggerAttack();
    }

    IEnumerator FriendlyKnockback()
    {
        yield return new WaitForSeconds(weaponLifetime * 0.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, friendlyKnockbackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (collider.gameObject != transform.parent.gameObject)
                {
                    Vector3 knockbackDirection = collider.transform.position - transform.position;
                    collider.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackStrength, knockbackTime, knockbackDirection);
                }
            }
        }
    }
}
