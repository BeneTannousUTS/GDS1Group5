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
        GameObject smallPopup = Instantiate(smallPopupPrefab);
        smallPopup.transform.position = entityObject.transform.position;
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
