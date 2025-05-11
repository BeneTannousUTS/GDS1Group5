using UnityEngine;

public class SpikeHazard : Hazards, IPressed
{
    [SerializeField] GameObject hitbox;
    [SerializeField] bool switchActivated;
    [SerializeField] bool activeStart;
    [SerializeField] float spikeOffset;
    [SerializeField] Animator animator;
    [SerializeField] float waitTime = 0;
    [SerializeField] bool startRoomCage;
    private float amountPressed;
    private bool active;
    private Collider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSourceType(gameObject.tag);
        SetDamageValue(5);
        waitTime -= spikeOffset;
        if (activeStart)
        {
            ActivateSpikes();
        }
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!switchActivated && !startRoomCage)
        {
            Attack();
        }
    }

    void ActivateSpikes()
    {
        if (!active && !roomCleared)
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
        if (!roomCleared)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= 3)
            {
                if (!active)
                {
                    hitbox.SetActive(true);
                    animator.SetTrigger("activate");
                    active = true;
                }
            }
            if (waitTime >= 6)
            {
                hitbox.SetActive(false);
                animator.SetTrigger("deactivate");
                animator.SetTrigger("deactivate");
                waitTime = 0;
                active = false;
            }
        }
    }

    public void Pressed()
    {
        amountPressed += 1;
        if (activeStart)
        {
            DeactivateSpikes();
        }
        else
        {
            ActivateSpikes();
        }
        
    }

    public void Unpressed()
    {
        amountPressed -= 1;
        if (amountPressed == 0)
        {
            if (activeStart)
            {
                ActivateSpikes();
            }
            else
            {
                DeactivateSpikes();
            }
        }
    }
}
