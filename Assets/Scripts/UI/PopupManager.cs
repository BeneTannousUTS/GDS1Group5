using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    GameObject smallPopupPrefab;
    float smallPopupTime = 1.2f;

    public void SpawnSmallPopup(GameObject entityObject, string text, Color colour)
    {
        GameObject smallPopup = Instantiate(smallPopupPrefab);
        smallPopup.transform.position = entityObject.transform.position;
        smallPopup.GetComponent<PopupHandler>().SetText(text, colour);
        Destroy(smallPopup, smallPopupTime);
    }
}
