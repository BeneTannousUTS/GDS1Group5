// Author: Zac
// Handles enemy caster attack

using UnityEngine;

public class Caster : WeaponStats
{
    protected override void TriggerAttack()
    {
        if (projectile != null)
        {
            GameObject currentProjectile = Instantiate(projectile, transform.position + transform.up, transform.rotation);
            currentProjectile.GetComponent<Projectile>().SetShotDirection(currentProjectile.transform.up);
            currentProjectile.GetComponent<Projectile>().SetDamageValue(damageValue * damageMod);
            currentProjectile.GetComponent<Projectile>().SetFriendlyFire(friendlyFire);
            currentProjectile.GetComponent<Projectile>().SetSourceType(sourceType);
        }

        base.TriggerAttack();
    }
}
