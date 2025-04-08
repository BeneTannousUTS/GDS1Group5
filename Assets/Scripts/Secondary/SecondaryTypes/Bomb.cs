using UnityEngine;

public class Bomb : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public float GetCooldownLength()
    {
        return cooldownLength;
    }

    public void DoSecondary()
    {
            GameObject tempProjectile = Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position, Quaternion.identity);
            tempProjectile.GetComponent<WeaponStats>().SetSourceType(gameObject.GetComponent<SecondaryStats>().GetSourceType());
            tempProjectile.GetComponent<WeaponStats>().SetSourceObject(gameObject.GetComponent<SecondaryStats>().GetSourceObject());
    }
}
