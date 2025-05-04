// AUTHOR: Zac
// Handles the change of options in the settings menu

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OptionsMenuManager : MonoBehaviour
{
    // Reference these in the Inspector by dragging your Slider components
    public Button audioTabButton;
    public Button visualTabButton;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;
    public TMP_Dropdown windowModeDropdown;
    public Button backButton;
    public GameObject confirmQuitPrefab;
    private bool isConfirming = false;

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
        Navigation backNav = backButton.navigation;
        backNav.selectOnUp = effectVolumeSlider;
        backButton.navigation = backNav;

        Navigation audioNav = audioTabButton.navigation;
        audioNav.selectOnDown = masterVolumeSlider;
        audioTabButton.navigation = audioNav;

        Navigation visualNav = visualTabButton.navigation;
        visualNav.selectOnDown = masterVolumeSlider;
        visualTabButton.navigation = visualNav;
    }

    public void LoadVisuals()
    {
        Navigation backNav = backButton.navigation;
        backNav.selectOnUp = windowModeDropdown;
        backButton.navigation = backNav;

        Navigation audioNav = audioTabButton.navigation;
        audioNav.selectOnDown = windowModeDropdown;
        audioTabButton.navigation = audioNav;

        Navigation visualNav = visualTabButton.navigation;
        visualNav.selectOnDown = windowModeDropdown;
        visualTabButton.navigation = visualNav;
    }

    public void OnToMenuButton()
    {
        Debug.Log("To Menu Pressed");
        StartCoroutine(MenuConfirmSequence());
    }

    IEnumerator MenuConfirmSequence()
    {
        isConfirming = true;
        GameObject quitConfirmObject = Instantiate(confirmQuitPrefab, transform);
        QuitConfirmHandler quitConfirmHandler = quitConfirmObject.GetComponent<QuitConfirmHandler>();
        PlayerInput activeInput = FindAnyObjectByType<InGameOptionsManager>().GetActiveInput();
        quitConfirmHandler.init(activeInput);
        EventSystem.current.SetSelectedGameObject(null);

        ConfirmManager.Instance.SetPriorityHandler(quitConfirmHandler);
        List<BaseConfirmHandler> handlerList = new List<BaseConfirmHandler> { quitConfirmHandler };
        yield return ConfirmManager.Instance.WaitForAllConfirmations(handlerList);

        Debug.Log($"Confirmed Choice: {quitConfirmHandler.confirmedChoice}");
        isConfirming = false;

        if (quitConfirmHandler.confirmedChoice == true)
        {
            FindAnyObjectByType<InGameOptionsManager>().ToggleOptionsMenu(activeInput);

            GameObject playerHUD = FindAnyObjectByType<LobbyHudHelper>().transform.parent.gameObject;

            if (playerHUD)
            {
                Destroy(playerHUD);
            }

            PlayerManager.instance.ResetPlayers();

            SceneManager.LoadScene("MainMenu");
        } else
        {
            Destroy(quitConfirmObject);
            backButton.Select();
            activeInput.SwitchCurrentActionMap("Menu");
        }
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

    public bool GetIsConfirming() => isConfirming;
}