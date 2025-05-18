using System.Collections;
using UnityEngine;

public class Hammer : WeaponStats
{
    public float friendlyKnockbackRadius = 2f;

    protected override void TriggerAttack()
    {
        
        if (GetSourceObject().TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.KnockbackPlayer(0, weaponLifetime, Vector3.zero);
        }

        StartCoroutine(FriendlyKnockback());
        base.TriggerAttack();
    }

    IEnumerator FriendlyKnockback()
    {
        yield return new WaitForSeconds(weaponLifetime * 0.25f);

        if (damageValue > 0) {
            FindAnyObjectByType<AudioManager>().PlaySoundEffect("Slam");
        }

        yield return new WaitForSeconds(weaponLifetime * 0.25f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, friendlyKnockbackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (collider.gameObject != transform.parent.gameObject)
                {
                    Vector3 knockbackDirection = collider.transform.position - GetSourceObject().transform.position;
                    collider.GetComponent<PlayerMovement>().KnockbackPlayer(knockbackStrength, knockbackTime, knockbackDirection);
                }
            }
        }
    }
}
