using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // BGM Tracks
    private AudioSource menuThemeSource;
    private AudioSource mainThemeSource;
    private AudioSource traitorThemeSource;
    private AudioSource bossThemeSource;

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

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        menuThemeSource = audioSources[0];
        mainThemeSource = audioSources[1];
        traitorThemeSource = audioSources[2];
        bossThemeSource = audioSources[3];

        PlayMainTheme(); // THIS IS ONLY TEMP I WANT THE GAME MANAGER TO CALL THE PLAY THEME.
    }

    public void OptionsUpdated()
    {
        menuThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        mainThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        traitorThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        bossThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
    }

    public void PlayMenuTheme()
    {
        if (menuThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Menu Theme' is yet to be added to the audio source");
        }

        menuThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        menuThemeSource.Play();
    }

    public void PlayMainTheme()
    {
        if (mainThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Main Theme' is yet to be added to the audio source");
        }

        mainThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        mainThemeSource.Play();
    }

    public void PlayTraitorTheme()
    {
        if (traitorThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Traitor Theme' is yet to be added to the audio source");
        }

        traitorThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        traitorThemeSource.Play();
    }

    public void PlayBossTheme()
    {
        if (bossThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Boss Theme' is yet to be added to the audio source");
        }

        bossThemeSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.musicVolumeLevel;
        bossThemeSource.Play();
    }

    public void PlaySoundEffect(string soundEffectName)
    {
        AudioClip audioClip;
        soundEffectDict.TryGetValue(soundEffectName, out audioClip);

        if (audioClip == null)
        {
            Debug.LogWarning($"The sound effect: '{soundEffectName} does not exist. Please make sure the name is correct and is part of the sound effect dictionary.");
            return;
        }

        AudioSource soundEffectSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource.resource = audioClip;
        soundEffectSource.volume = 1f * SettingsManager.instance.masterVolumeLevel * SettingsManager.instance.effectVolumeLevel;
        soundEffectSource.Play();

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
        StartCoroutine(DuckBGM(audioClip.length));

        StartCoroutine(DestroyAudioSource(soundJingleSource, 7.5f));
    }

    IEnumerator DestroyAudioSource(AudioSource audioSource, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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
        else if (bossThemeSource.isPlaying)
            currentTheme = bossThemeSource;

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
