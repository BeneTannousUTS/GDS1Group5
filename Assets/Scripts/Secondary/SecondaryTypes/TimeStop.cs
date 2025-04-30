using UnityEngine;

public class TimeStop : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
        // Time stop time stop time stop time stop time
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
