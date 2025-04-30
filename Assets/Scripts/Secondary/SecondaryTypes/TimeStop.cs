using UnityEngine;

public class TimeStop : MonoBehaviour, ISecondary
{
    [SerializeField] float cooldownLength;

    public void DoSecondary()
    {
        // Time stop time stop time stop time stop time
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies) 
        {
            enemy.GetComponent<EnemyMovement>().SetFrozen();
        }

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach(GameObject projectile in projectiles) 
        {
            if (projectile.GetComponent<Projectile>().GetSourceType().Equals("Player") == false) {
                projectile.GetComponent<Projectile>().SetFrozen();
            }
        }
    }

    public float GetCooldownLength()
    {
        return cooldownLength;
    }
}
