// AUTHOR: Zac
// Holds all of the game settings

using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    public float masterVolumeLevel = 1.0f;
    public float musicVolumeLevel = 1.0f;
    public float effectVolumeLevel = 1.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
