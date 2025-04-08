using UnityEngine;

public class Shield : WeaponStats
{
    public float detectionRadius = 2f;

    protected override void TriggerAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.parent.up * 0.5f, detectionRadius);
        foreach (Collider2D hit in hits)
        {
            // Check if the collider has a Projectile component.
            Projectile projectile = hit.GetComponent<Projectile>();
            if (projectile != null)
            {
                // Calculate the deflection direction from the shield's center.
                Vector3 deflectionDirection = (projectile.transform.position - transform.position).normalized;
                projectile.SetShotDirection(deflectionDirection);
                projectile.SetFriendlyFire(true);
            }
        }
        
        base.TriggerAttack();
    }
}
