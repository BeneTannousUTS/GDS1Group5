// AUTHOR: Zac
// Handles the change of options in the settings menu

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    // Reference these in the Inspector by dragging your Slider components
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;
    public TMP_Dropdown windowModeDropdown;
    public Button backButton;

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

        // Build drop down options
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(new List<string> { "Full Screen", "Windowed", "Borderless" });
        windowModeDropdown.value = GetWindowModeDropdownIndex();
        windowModeDropdown.RefreshShownValue();
    }

    // Called whenever the master volume slider value changes
    public void OnMasterVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.masterVolumeLevel = value;
            FindAnyObjectByType<AudioManager>().OptionsUpdated();
        }
    }

    // Called whenever the music volume slider value changes
    public void OnMusicVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.musicVolumeLevel = value;
            FindAnyObjectByType<AudioManager>().OptionsUpdated();
        }
    }

    // Called whenever the effects volume slider value changes
    public void OnEffectVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.effectVolumeLevel = value;
            FindAnyObjectByType<AudioManager>().OptionsUpdated();
        }
    }

    public void LoadAudio()
    {
        Navigation nav = backButton.navigation;
        nav.selectOnUp = effectVolumeSlider;
        backButton.navigation = nav;
    }

    public void LoadVisuals()
    {
        Navigation nav = backButton.navigation;
        nav.selectOnUp = windowModeDropdown;
        backButton.navigation = nav;
    }

    private int GetWindowModeDropdownIndex()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return 0;
            case FullScreenMode.Windowed:
                return 1;
            case FullScreenMode.FullScreenWindow:
                return 2;
            default:
                return 0;
        }
    }

    public void SetWindowMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Debug.LogWarning("Undefined window mode index");
                break;
        }
    }
}