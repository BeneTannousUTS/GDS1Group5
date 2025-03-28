using UnityEngine;

public class Bomb : MonoBehaviour, ISecondary
{
    public void DoSecondary() 
    {
        GameObject tempProjectile = Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position, Quaternion.identity);
        tempProjectile.GetComponent<WeaponStats>().SetSourceType(gameObject.GetComponent<SecondaryStats>().GetSourceType());
    }
}
