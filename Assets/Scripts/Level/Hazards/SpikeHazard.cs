using UnityEngine;

public class SpikeHazard : Hazards
{
    [SerializeField] GameObject hitbox;
    [SerializeField] bool switchActivated;
    [SerializeField] float spikeOffset;
    [SerializeField] Animator animator;
    [SerializeField] float waitTime = 0;
    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSourceType(gameObject.tag);
        SetDamageValue(10);
        waitTime -= spikeOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void ActivateSpikes()
    {
        if (!active)
        {
            hitbox.SetActive(true);
            animator.SetTrigger("activate");
            active = true;
        }
    }

    public void DeactivateSpikes()
    {
        if (active)
        {
            hitbox.SetActive(false);
            animator.SetTrigger("deactivate");
            active = false;
        }
    }

    void Attack()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= 4 + spikeOffset)
        {
            if (!active)
            {
                hitbox.SetActive(true);
                animator.SetTrigger("activate");
                active = true;
            }
        }
        if (waitTime >= 7 + spikeOffset)
        {
            hitbox.SetActive(false);
            animator.SetTrigger("deactivate");
            animator.SetTrigger("deactivate");
            waitTime = 0;
            active = false;
        }
    }
}
