using UnityEngine;
using UnityTools;

[RequireComponent(typeof(AudioSource))]
public class AudioSourcePlayerPrefsLink : MonoBehaviour
{
    public const string SFX_VOLUME_KEY = "SFXVolume";
    public const string MUSIC_VOLUME_KEY = "MusicVolume";

    public SoundVolumeType soundVolumeType;
    public float defaultValue = 1f;
    public EventEmitter<float> playerPrefsUpdatedEmitter;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateVolume();

        if (playerPrefsUpdatedEmitter != null)
            playerPrefsUpdatedEmitter.eventEmitter.AddListener((_) => UpdateVolume());
    }

    private void UpdateVolume()
    {
        string volumeKey = soundVolumeType == SoundVolumeType.SFX ? SFX_VOLUME_KEY : MUSIC_VOLUME_KEY;
        audioSource.volume = PlayerPrefs.GetFloat(volumeKey, defaultValue);
    }

    public enum SoundVolumeType 
    { 
        SFX,
        Music
    }
}