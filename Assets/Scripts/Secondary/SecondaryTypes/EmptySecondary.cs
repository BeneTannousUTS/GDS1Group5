using UnityEngine;

public class EmptySecondary : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
        // do nothing :)
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
