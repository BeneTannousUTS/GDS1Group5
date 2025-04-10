using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Win(collision.gameObject);
    }
}
