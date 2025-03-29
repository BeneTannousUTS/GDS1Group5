using UnityEngine;

public class StrengthPotion : MonoBehaviour, ISecondary
{
    public void DoSecondary() 
    {
        Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position, Quaternion.identity);
    }
}
