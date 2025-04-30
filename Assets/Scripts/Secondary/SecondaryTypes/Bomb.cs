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
        gameObject.GetComponent<Explosion>().SpawnExplosion();
    }
}
