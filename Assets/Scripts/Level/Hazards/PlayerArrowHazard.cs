using UnityEngine;

public class PlayerArrowHazard : MonoBehaviour
{
    private float waitTime = 0f;
    public GameObject projectile;
    public float lifetime = 5f;
    private float lifeTimer = 0f;

    void FireArrow()
    {
        if (projectile != null)
        {
            Vector3 baseDirection = transform.up;
            Vector3 perpendicular = new Vector3(-baseDirection.y, baseDirection.x, 0f);

            Vector3 shotDirection = (baseDirection).normalized;

            GameObject currentProjectile = Instantiate(projectile, transform.position + baseDirection, Quaternion.identity);
            Projectile proj = currentProjectile.GetComponent<Projectile>();

            gameObject.GetComponent<Animator>().SetTrigger("shoot");

            proj.SetShotDirection(shotDirection);
            proj.SetDamageValue(7);
            proj.SetFriendlyFire(false);
            proj.SetSourceType("Player");
            proj.SetSourceObject(gameObject);
        }
    }

    void FireTimer()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
        else {
            waitTime += Time.deltaTime;
            if (waitTime >= 0.5f)
            {
                FireArrow();
                waitTime = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        FireTimer();
    }
}
