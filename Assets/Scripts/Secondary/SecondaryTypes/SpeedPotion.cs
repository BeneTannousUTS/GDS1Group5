using UnityEngine;

public class SpeedPotion : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
            Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position, Quaternion.identity);
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
