using UnityEngine;
using UnityTools;

[RequireComponent(typeof(AudioSource))]
public class AudioSourcePlayerPrefsLink : MonoBehaviour
{
    public const string SFX_VOLUME_KEY = "SFXVolume";
    public const string MUSIC_VOLUME_KEY = "MusicVolume";
    public const string MASTER_VOLUME_KEY = "MasterVolume";

    public SoundVolumeType soundVolumeType;
    public float defaultValue = 1f;
    public EventEmitter<float> playerPrefsUpdatedEmitter;

    private AudioSource audioSource;

    private void OnValidate()
    {
        if (playerPrefsUpdatedEmitter == null)
        {
            Debug.LogWarning($"Player prefs updated emitter for {name} is null", gameObject);
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateVolume();

        if (playerPrefsUpdatedEmitter != null)
            playerPrefsUpdatedEmitter.eventEmitter.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float _)
    {
        UpdateVolume();
    }

    private void OnDestroy()
    {
        if (playerPrefsUpdatedEmitter != null)
            playerPrefsUpdatedEmitter.eventEmitter.RemoveListener(UpdateVolume);
    }

    private void UpdateVolume()
    {
        string volumeKey = GetKey(soundVolumeType);
        var masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        audioSource.volume = PlayerPrefs.GetFloat(volumeKey, defaultValue * masterVolume);
    }

    public static string GetKey(SoundVolumeType soundVolumeType)
    {
        switch (soundVolumeType)
        {
            case SoundVolumeType.SFX:
                return SFX_VOLUME_KEY;
            case SoundVolumeType.Music:
                return MUSIC_VOLUME_KEY;
            default:
                return MASTER_VOLUME_KEY;
        }
    }

    public enum SoundVolumeType
    { 
        SFX,
        Music,
        Master
    }
}