using UnityEngine;

public class ResetPlayerData : MonoBehaviour
{
    public void ResetPlayerManagerData()
    {
        if (PlayerManager.instance != null)
        {
            Destroy(PlayerManager.instance);
        }
    }
}
