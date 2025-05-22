using UnityEngine;

public class Boomerang : WeaponStats
{
    protected override void TriggerAttack()
    {
        if (projectile != null)
        {
            GameObject currentProjectile = Instantiate(projectile, transform.position + transform.up, transform.rotation);
            currentProjectile.GetComponent<Projectile>().SetShotDirection(currentProjectile.transform.up);
            currentProjectile.GetComponent<Projectile>().SetDamageValue(Mathf.Ceil(damageValue * damageMod));
            currentProjectile.GetComponent<Projectile>().SetFriendlyFire(friendlyFire);
            currentProjectile.GetComponent<Projectile>().SetSourceType(sourceType);
            currentProjectile.GetComponent<Projectile>().SetSourceObject(sourceObject);
            currentProjectile.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        }

        base.TriggerAttack();
    }
}
