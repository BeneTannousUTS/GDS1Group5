using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio Settings (Should be fetched on Awake)
    private float masterVolumeLevel = 1.0f; // these values are more/less temp for now before we have a settings system
    private float musicVolumeLevel = 0.5f;
    private float efffectVolumeLevel = 0.5f;

    // BGM Tracks
    private AudioSource menuThemeSource;
    private AudioSource mainThemeSource;
    private AudioSource traitorThemeSource;
    private AudioSource bossThemeSource;

    // Sound Effects
    private AudioSource playerAttackSource; // PlayerAttack
    private AudioSource playerDamageSource; // PlayerDamage
    // once multiple enemies have been implemented each with have its own audio effects
    // thus the name of this source will be changed
    private AudioSource enemyAttackSource; // EnemyAttack
    private AudioSource enemyDamageSource; // EnemyDamage

    // Sound Effect Dictionary
    [System.Serializable]
    public class AudioSourceEntry
    {
        public string name;
        public AudioSource audioSource;
    }
    public List<AudioSourceEntry> audioSourceEntries;
    private Dictionary<string, AudioSource> soundEffectDict = new Dictionary<string, AudioSource>();

    void Awake()
    {
        // Fetch init audio settings from setting manager
        // Note these levels can change during the game so these values are not fixed/final

        // Create dictionary from list created in the inspector
        foreach (var entry in audioSourceEntries)
        {
            if (!soundEffectDict.ContainsKey(entry.name) && entry.audioSource != null)
            {
                soundEffectDict.Add(entry.name, entry.audioSource);
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

    public void PlayMenuTheme()
    {
        if (menuThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Menu Theme' is yet to be added to the audio source");
        }

        menuThemeSource.volume = 1f * masterVolumeLevel * musicVolumeLevel;
        menuThemeSource.Play();
    }

    public void PlayMainTheme()
    {
        if (mainThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Main Theme' is yet to be added to the audio source");
        }

        mainThemeSource.volume = 1f * masterVolumeLevel * musicVolumeLevel;
        mainThemeSource.Play();
    }

    public void PlayTraitorTheme()
    {
        if (traitorThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Traitor Theme' is yet to be added to the audio source");
        }

        traitorThemeSource.volume = 1f * masterVolumeLevel * musicVolumeLevel;
        traitorThemeSource.Play();
    }

    public void PlayBossTheme()
    {
        if (bossThemeSource.resource == null)
        {
            Debug.LogWarning("The track for 'Boss Theme' is yet to be added to the audio source");
        }

        bossThemeSource.volume = 1f * masterVolumeLevel * musicVolumeLevel;
        bossThemeSource.Play();
    }

    public void PlaySoundEffect(string soundEffectName)
    {
        AudioSource desiredAudioSource;
        soundEffectDict.TryGetValue(soundEffectName, out desiredAudioSource);

        if (desiredAudioSource == null)
        {
            Debug.LogWarning($"The sound effect: '{soundEffectName} does not exist. Please make sure the name is correct and is part of the sound effect dictionary.");
            return;
        }

        desiredAudioSource.volume = 1f * masterVolumeLevel * efffectVolumeLevel;
        desiredAudioSource.Play();
    }

}
