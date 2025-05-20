using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // BGM Tracks
    [SerializeField] private AudioSource menuThemeSource;
    [SerializeField] private AudioSource mainThemeSource;
    [SerializeField] private AudioSource traitorThemeSource;
    [SerializeField] private AudioSource resultsThemeSource;

    // Sound Effect Dictionary
    [System.Serializable]
    public class AudioClipEntry
    {
        public string name;
        public AudioClip audioClip;
    }
    public List<AudioClipEntry> effectClipEntries;
    private Dictionary<string, AudioClip> soundEffectDict = new Dictionary<string, AudioClip>();
    public List<AudioClipEntry> jingeClipEntries;
    private Dictionary<string, AudioClip> soundJingleDict = new Dictionary<string, AudioClip>();
    private int activeSounds = 0; // number of sound effects/jingles being played

    void Awake()
    {
        // Fetch init audio settings from setting manager
        // Note these levels can change during the game so these values are not fixed/final

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        // Create dictionary from list created in the inspector
        foreach (var entry in effectClipEntries)
        {
            if (!soundEffectDict.ContainsKey(entry.name) && entry.audioClip != null)
            {
                soundEffectDict.Add(entry.name, entry.audioClip);
            }
            else
            {
                Debug.LogWarning($"Duplicate or null AudioSource entry: {entry.name}");
            }
        }

        foreach (var entry in jingeClipEntries)
        {
            if (!soundJingleDict.ContainsKey(entry.name) && entry.audioClip != null)
            {
                soundJingleDict.Add(entry.name, entry.audioClip);
            }
            else
            {
                Debug.LogWarning($"Duplicate or null AudioSource entry: {entry.name}");
            }
        }
    }

    // void Start()
    // {
    //     AudioSource[] audioSources = GetComponents<AudioSource>();

    //     menuThemeSource = audioSources[0];
    //     mainThemeSource = audioSources[1];
    //     traitorThemeSource = audioSources[2];
    //     resultsThemeSource = audioSources[3];
    // }

    public void OptionsUpdated()
    {
        menuThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        mainThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        traitorThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        resultsThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
    }

    public void PlayMenuTheme()
    {
        StopBGM();
        menuThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        menuThemeSource.Play();
    }

    public void PlayMainTheme()
    {
        StopBGM();
        mainThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        mainThemeSource.Play();
    }

    public void PlayTraitorTheme()
    {
        StopBGM();
        if (traitorThemeSource.clip == null)
        traitorThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        traitorThemeSource.Play();
    }

    public void PlayResultsTheme()
    {
        StopBGM();
        resultsThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        resultsThemeSource.Play();
    }

    public void PlaySoundEffect(string soundEffectName)
    {
        PlaySoundEffect(soundEffectName, 1.0f);
    }

    public void PlaySoundEffect(string soundEffectName, float pitchVariation)
    {
        if (soundEffectName.IsNullOrEmpty()) return;
        
        AudioClip audioClip;
        soundEffectDict.TryGetValue(soundEffectName, out audioClip);

        if (audioClip == null)
        {
            Debug.LogWarning($"The sound effect: '{soundEffectName} does not exist. Please make sure the name is correct and is part of the sound effect dictionary.");
            return;
        }

        AudioSource soundEffectSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource.resource = audioClip;
        soundEffectSource.pitch = pitchVariation;
        soundEffectSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.effectVolumeLevel;
        if (activeSounds > 5) soundEffectSource.volume *= 0.5f;
        soundEffectSource.Play();
        activeSounds++;

        StartCoroutine(DestroyAudioSource(soundEffectSource, 1.5f));
    }

    public void PlaySoundJingle(string soundJingleName)
    {
        AudioClip audioClip;
        soundJingleDict.TryGetValue(soundJingleName, out audioClip);

        if (audioClip == null)
        {
            Debug.LogWarning($"The sound effect: '{soundJingleName} does not exist. Please make sure the name is correct and is part of the sound jingle dictionary.");
            return;
        }

        AudioSource soundJingleSource = gameObject.AddComponent<AudioSource>();
        soundJingleSource.resource = audioClip;
        soundJingleSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.effectVolumeLevel;
        soundJingleSource.Play();
        activeSounds++;
        StartCoroutine(DuckBGM(audioClip.length));

        StartCoroutine(DestroyAudioSource(soundJingleSource, 7.5f));
    }

    public void StopBGM()
    {
        if (menuThemeSource)    menuThemeSource.Stop();
        if (mainThemeSource)    mainThemeSource.Stop();
        if (traitorThemeSource) traitorThemeSource.Stop();
        if (resultsThemeSource) resultsThemeSource.Stop();
    }

    IEnumerator DestroyAudioSource(AudioSource audioSource, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        activeSounds--;
        Destroy(audioSource);
    }

    IEnumerator DuckBGM(float timeDucked) // does not currently handle multiple calls at the same time...
    {
        AudioSource currentTheme = null;
        if (menuThemeSource.isPlaying)
            currentTheme = menuThemeSource;
        else if (mainThemeSource.isPlaying)
            currentTheme = mainThemeSource;
        else if (traitorThemeSource.isPlaying)
            currentTheme = traitorThemeSource;
        else if (resultsThemeSource.isPlaying)
            currentTheme = resultsThemeSource;

        if (currentTheme == null)
        {
            Debug.LogWarning("No theme playing in DuckBGM.");
            yield break;
        }

        float timeToTransition = 0.5f;
        float transitionTimeElapsed = 0f;
        float startVolumeLevel = 1 * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        float targetVolumeLevel = startVolumeLevel * 0.4f;

        // Duck volume
        while (transitionTimeElapsed < timeToTransition)
        {
            transitionTimeElapsed += Time.deltaTime;
            float t = transitionTimeElapsed / timeToTransition;
            currentTheme.volume = Mathf.Lerp(startVolumeLevel, targetVolumeLevel, t);
            yield return null;
        }

        currentTheme.volume = targetVolumeLevel;

        // Wait while jingle plays
        yield return new WaitForSeconds(timeDucked - 0.5f);

        // Restore volume
        transitionTimeElapsed = 0f;
        while (transitionTimeElapsed < timeToTransition)
        {
            transitionTimeElapsed += Time.deltaTime;
            float t = transitionTimeElapsed / timeToTransition;
            currentTheme.volume = Mathf.Lerp(targetVolumeLevel, startVolumeLevel, t);
            yield return null;
        }

        currentTheme.volume = startVolumeLevel;
    }


}
