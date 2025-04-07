using UnityEngine;

public class Crossbow : WeaponStats
{
    public int numOfArrows = 3;
    public float spacing = 0.5f;
    protected override void TriggerAttack()
    {
        if (projectile != null)
        {
            float baseAngle = transform.eulerAngles.z;
            for (int i = 0; i < numOfArrows; ++i)
            {
                float angleOffset = (i - ((numOfArrows - 1) / 2f)) * spacing;
                GameObject currentProjectile = Instantiate(projectile, transform.position + transform.up, Quaternion.identity);

                currentProjectile.transform.eulerAngles = new Vector3(0, 0, baseAngle + angleOffset);

                Projectile proj = currentProjectile.GetComponent<Projectile>();
                proj.SetDamageValue(damageValue * damageMod);
                proj.SetFriendlyFire(friendlyFire);
                proj.SetSourceType(sourceType);
            }
        }

        base.TriggerAttack();
    }
}
