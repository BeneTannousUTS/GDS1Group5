using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject hitbox;
    bool switchActivated = false;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] connectedObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!switchActivated && collision.CompareTag("Player"))
        {
            switchActivated = true;
            animator.SetTrigger("down");
            foreach (GameObject obj in connectedObjects)
            {
                obj.GetComponent<IPressed>().Pressed();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (switchActivated && collision.CompareTag("Player"))
        {
            switchActivated = false;
            animator.SetTrigger("up");
            foreach (GameObject obj in connectedObjects)
            {
                obj.GetComponent<IPressed>().Unpressed();
            }
        }
    }
}
