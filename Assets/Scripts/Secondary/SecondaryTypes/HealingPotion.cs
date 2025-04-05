using UnityEngine;

public class HealingPotion : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public float GetCooldownLength()
    {
        return cooldownLength;
    }

    public void DoSecondary()
    {
            GameObject tempProjectile = Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position, Quaternion.identity);
            tempProjectile.GetComponent<WeaponStats>().SetSourceType("Enemy");
    }
}
