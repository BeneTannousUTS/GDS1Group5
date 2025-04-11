using UnityEngine;

public class SpeedBoost : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;
    [SerializeField] TempBuff buff;

    public float GetCooldownLength()
    {
        return cooldownLength;
    }

    public void DoSecondary() // current issue: adds the buff like 30 times
    {
            GameObject player = gameObject.GetComponent<SecondaryStats>().GetSourceObject();
            player.GetComponent<PlayerStats>().SetPassive(buff.GetPassive());
            gameObject.GetComponent<PlayerStats>().public_RemoveTempBuff(buff.GetBuffTime(), buff.GetPassive());
    }
}
