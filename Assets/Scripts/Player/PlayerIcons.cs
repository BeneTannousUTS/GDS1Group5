using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcons : MonoBehaviour
{
    [SerializeField] private Image iconSlot;
    private float durationTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (durationTimer > 0)
        {
            durationTimer -= Time.deltaTime;
        }
        else
        {
            iconSlot.gameObject.SetActive(false);
        }
    }


    public void SetIcon(Sprite iconImage, float duration) // Sets the icon image to the given sprite
    {
        iconSlot.sprite = iconImage;
        durationTimer = duration;
        iconSlot.gameObject.SetActive(true);
    }
}
