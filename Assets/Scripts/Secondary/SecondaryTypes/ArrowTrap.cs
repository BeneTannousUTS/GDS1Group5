using UnityEngine;

public class ArrowTrap : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
        Vector3 spawnDir = gameObject.GetComponent<SecondaryStats>().GetSourceObject().GetComponent<PlayerMovement>().GetFacingDirection();
        Instantiate(gameObject.GetComponent<SecondaryStats>().GetProjectile(), transform.position + spawnDir.normalized * 1.5f, gameObject.GetComponent<SecondaryStats>().CalculateQuaternion(spawnDir));
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
