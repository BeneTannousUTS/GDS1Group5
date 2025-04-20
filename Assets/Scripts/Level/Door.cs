using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IPressed
{
    [SerializeField] bool switchActivated;
    float playerCount;
    float switchCount;
    bool active = true;

    public void Pressed()
    {
        switchCount++;
        if (switchCount >= playerCount && active)
        {
            OpenDoor();
        }
    }

    public void Unpressed()
    {
        switchCount--;
    }

    public void RoomClear()
    {
        if (!switchActivated)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        if (active)
        {
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlaySoundJingle("RoomClear");
            gameObject.GetComponent<Animator>().SetTrigger("open");
            StartCoroutine(RemoveDoor());
            active = false;
        }
    }

    IEnumerator RemoveDoor()
    {
        yield return new WaitForSeconds(1.2f);
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (switchActivated)
        {
            playerCount = FindAnyObjectByType<GameManager>().GetPlayerList().Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
