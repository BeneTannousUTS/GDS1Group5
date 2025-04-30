using UnityEngine;

public class Rock : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
        Vector3 spawnDir = gameObject.GetComponent<SecondaryStats>().GetSourceObject().GetComponent<PlayerMovement>().GetFacingDirection();
        GameObject currentProjectile = Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position + spawnDir.normalized * 1.5f, gameObject.GetComponent<SecondaryStats>().CalculateQuaternion(spawnDir));
        currentProjectile.GetComponent<Projectile>().SetShotDirection(currentProjectile.transform.up);
        currentProjectile.GetComponent<Projectile>().SetDamageValue(2f);
        currentProjectile.GetComponent<Projectile>().SetFriendlyFire(false);
        currentProjectile.GetComponent<Projectile>().SetSourceType(gameObject.GetComponent<SecondaryStats>().GetSourceType());
        currentProjectile.GetComponent<Projectile>().SetSourceObject(gameObject.GetComponent<SecondaryStats>().GetSourceObject());
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
