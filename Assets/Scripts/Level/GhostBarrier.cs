using System.Collections;
using UnityEngine;

public class GhostBarrier : MonoBehaviour
{
    IEnumerator DisplayBarrier() 
    {
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        int numOfTraitors = gameManager.traitorManager.GetTraitorAmount();

        if (!(collision.tag.Equals("Player") || collision.tag.Equals("Traitor")))
        {
            return;
        }

        if (collision.GetComponent<HealthComponent>().GetIsDead())
        {
            collision.gameObject.transform.position = collision.gameObject.transform.position + new Vector3(0f, -1f, 0f);
            StartCoroutine(DisplayBarrier());
        }
    }
}
