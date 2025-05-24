using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject hitbox;
    bool switchActivated = false;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] connectedObjects;
    float pressedCount;
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
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            pressedCount++;
            if (!switchActivated && collision.CompareTag("Player") && pressedCount == 1)
            {
                switchActivated = true;
                animator.SetTrigger("down");
                AudioManager.instance.PlaySoundEffect("Click", 1.0f);
                foreach (GameObject obj in connectedObjects)
                {
                    obj.GetComponent<IPressed>().Pressed();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pressedCount--;
            if (switchActivated && collision.CompareTag("Player") && pressedCount == 0)
            {
                switchActivated = false;
                animator.SetTrigger("up");
                AudioManager.instance.PlaySoundEffect("Click", 0.9f);
                foreach (GameObject obj in connectedObjects)
                {
                    obj.GetComponent<IPressed>().Unpressed();
                }
            }
        }
    }

    public bool GetSwitchActivated()
    {
        return switchActivated;
    }
}
