using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ArrowHazard : Hazards, IPressed
{
    [SerializeField] GameObject projectile;
    [SerializeField] bool switchActivated;
    [SerializeField] bool activeStart;
    [SerializeField] float arrowOffset;
    [SerializeField] Animator animator;
    [SerializeField] float waitTime = 0;
    private float amountPressed;
    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitTime -= arrowOffset;
        if (!switchActivated)
        {
            active = true;
        }
    }


    void FireArrow()
    {
        if (!roomCleared)
        {
            if (projectile != null)
            {
                Vector3 baseDirection = transform.up;
                Vector3 perpendicular = new Vector3(-baseDirection.y, baseDirection.x, 0f);

                Vector3 shotDirection = (baseDirection).normalized;

                GameObject currentProjectile = Instantiate(projectile, transform.position + baseDirection, Quaternion.identity);
                Projectile proj = currentProjectile.GetComponent<Projectile>();

                proj.SetShotDirection(shotDirection);
                proj.SetDamageValue(10);
                proj.SetFriendlyFire(true);
                proj.SetSourceType(gameObject.tag);
                proj.SetSourceObject(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            FireTimer();
        }
    }

    void FireTimer()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= 2)
        {
            FireArrow();
            waitTime = 0;
        }
    }

    public void Pressed()
    {
        amountPressed += 1;
        waitTime = 2;
        active = true;
    }

    public void Unpressed()
    {
        amountPressed -= 1;
        if (amountPressed == 0)
        {
            active = false;
        }
    }
}
