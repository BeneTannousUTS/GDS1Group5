// AUTHOR: Zac
// Handles the change of options in the settings menu

using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    // Reference these in the Inspector by dragging your Slider components
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;

    void Start()
    {
        // Check if the SettingsManager exists
        if (SettingsManager.instance == null)
        {
            Debug.LogError("SettingsManager instance not found. Make sure a SettingsManager exists in your scene.");
            return;
        }

        SetOptions();

        // Add listeners for when the slider values change
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        effectVolumeSlider.onValueChanged.AddListener(OnEffectVolumeChanged);
    }

    public void SetOptions()
    {
        // Initialize slider values from the SettingsManager
        masterVolumeSlider.value = SettingsManager.instance.masterVolumeLevel;
        musicVolumeSlider.value = SettingsManager.instance.musicVolumeLevel;
        effectVolumeSlider.value = SettingsManager.instance.effectVolumeLevel;
    }

    // Called whenever the master volume slider value changes
    public void OnMasterVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.masterVolumeLevel = value;
        }
    }

    // Called whenever the music volume slider value changes
    public void OnMusicVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.musicVolumeLevel = value;
        }
    }

    // Called whenever the effects volume slider value changes
    public void OnEffectVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.effectVolumeLevel = value;
        }
    }
}