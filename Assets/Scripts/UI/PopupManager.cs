using System;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    GameObject smallPopupPrefab;
    [SerializeField]
    GameObject largePopupPrefab;
    float smallPopupTime = 1.2f;
    float largePopupTime = 5.0f;

    public void SpawnSmallPopup(GameObject entityObject, string text, Color colour)
    {
        SpawnSmallPopup(entityObject, text, colour, 1f);
    }

    public void SpawnSmallPopup(GameObject entityObject, string text, Color colour, float scale)
    {
        GameObject smallPopup = Instantiate(smallPopupPrefab);
        float adjustedScale = Math.Clamp(scale, 1f, 5f);
        smallPopup.transform.position = entityObject.transform.position;
        //smallPopup.transform.localScale *= adjustedScale;
        smallPopup.GetComponent<PopupHandler>().SetText(text, colour);
        Destroy(smallPopup, smallPopupTime);
    }

    public void SpawnLargePopup(string text, Color colour)
    {
        GameObject largePopup = Instantiate(largePopupPrefab, transform.position, transform.rotation);
        largePopup.GetComponent<PopupHandler>().SetText(text, colour);
        Destroy(largePopup, largePopupTime);
    }
}
