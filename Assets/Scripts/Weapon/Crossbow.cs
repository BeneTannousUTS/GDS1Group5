using UnityEngine;

public class Crossbow : WeaponStats
{
    private int numOfArrows = 3;
    private float deviationAmount = 0.3f;

    protected override void TriggerAttack()
    {
        if (projectile != null)
        {
            Vector3 baseDirection = transform.up;
            Vector3 perpendicular = new Vector3(-baseDirection.y, baseDirection.x, 0f);
            
            float middleIndex = (numOfArrows - 1) / 2f;
            
            for (int i = 0; i < numOfArrows; i++)
            {
                float offset = i - middleIndex;
                
                Vector3 deviation = perpendicular * offset * deviationAmount;
                
                Vector3 shotDirection = (baseDirection + deviation).normalized;
                
                GameObject currentProjectile = Instantiate(projectile, transform.position + baseDirection, Quaternion.identity);
                Projectile proj = currentProjectile.GetComponent<Projectile>();
                
                proj.SetShotDirection(shotDirection);
                proj.SetDamageValue(Mathf.Ceil(damageValue * damageMod));
                proj.SetFriendlyFire(friendlyFire);
                proj.SetSourceType(sourceType);
                proj.SetSourceObject(sourceObject);
            }
        }

        if (GetSourceObject().GetComponent<PlayerScore>() != null)
        {
            GetSourceObject().GetComponent<PlayerScore>().IncrementProjectilesShot();
            GetSourceObject().GetComponent<PlayerScore>().IncrementProjectilesShot();
        }

        base.TriggerAttack();
    }
}
