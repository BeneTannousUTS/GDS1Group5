using UnityEngine;

public class BarrelThrow : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
        Vector3 spawnDir = gameObject.GetComponent<SecondaryStats>().GetSourceObject().GetComponent<PlayerMovement>().GetFacingDirection();
        GameObject currentProjectile = Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position + spawnDir.normalized * 2.5f, gameObject.GetComponent<SecondaryStats>().CalculateQuaternion(spawnDir));
        currentProjectile.GetComponent<Projectile>().SetShotDirection(currentProjectile.transform.up);
        currentProjectile.GetComponent<Animator>().SetTrigger("vertical");
        currentProjectile.GetComponent<Projectile>().SetDamageValue(0);
        currentProjectile.GetComponent<Projectile>().SetFriendlyFire(true);
        currentProjectile.GetComponent<Projectile>().SetSourceType("Player");
        currentProjectile.GetComponent<Projectile>().SetSourceObject(gameObject);
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
