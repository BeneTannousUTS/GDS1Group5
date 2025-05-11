using System.Collections;
using UnityEngine;

public class Shield : WeaponStats
{
    public float detectionRadius = 2f;
    public float deflectionDuration = 0.5f;
    public float projectilePushDistance = 2f;
    public float deflectCheckInterval = 0.005f;
    private Coroutine activeShieldRoutine;

    protected override void TriggerAttack()
    {
        if (activeShieldRoutine != null)
        {
            StopCoroutine(activeShieldRoutine);
        }

        activeShieldRoutine = StartCoroutine(ShieldDeflectWindow());
        
        base.TriggerAttack();
    }

    IEnumerator ShieldDeflectWindow()
    {
        float timer = 0f;

        Vector3 facingDirection = Vector3.up;
        if (transform.parent.CompareTag("Player") || transform.parent.CompareTag("Traitor"))
        {
            facingDirection = transform.parent.GetComponent<PlayerMovement>().GetFacingDirection();
        }
        else
        {
            facingDirection = transform.parent.GetComponent<EnemyMovement>().GetFacingDirection();
        }

        while (timer < deflectionDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
            foreach (Collider2D hit in hits)
            {
                Projectile projectile = hit.GetComponent<Projectile>();
                if (projectile != null)
                {
                    StartCoroutine(ProjNoCollisionTimer(hit));
                    Debug.Log($"Shield trying to deflect {hit.name}");
                    projectile.SetShotDirection(facingDirection);
                    projectile.transform.position += facingDirection * projectilePushDistance;
                    projectile.SetSourceType("Player");
                }
            }

            timer += deflectCheckInterval;
            yield return new WaitForSeconds(deflectCheckInterval);
        }

        activeShieldRoutine = null;
    }

    IEnumerator ProjNoCollisionTimer(Collider2D collider)
{
    collider.enabled = false;
        yield return new WaitForSeconds(0.125f);
    collider.enabled = true;
}
}


