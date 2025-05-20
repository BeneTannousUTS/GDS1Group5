using UnityEngine;

public class HeartHeal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created\
    [SerializeField] float healAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthComponent health = collision.GetComponent<HealthComponent>();
            if (!health.GetIsDead())
            {
                collision.gameObject.GetComponent<HealthComponent>().TakeDamage(-healAmount);
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
