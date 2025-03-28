using UnityEngine;

public class HealingPotion : MonoBehaviour, ISecondary
{
    public void DoSecondary() 
    {
        GameObject tempProjectile = Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position, Quaternion.identity);
        tempProjectile.GetComponent<WeaponStats>().SetSourceType("Enemy");
    }
}
