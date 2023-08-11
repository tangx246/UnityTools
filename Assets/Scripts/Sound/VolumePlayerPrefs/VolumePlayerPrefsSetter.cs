using UnityEngine;
using UnityTools;

public class VolumePlayerPrefsSetter : MonoBehaviour
{
    public EventEmitter<float> volumeUpdatedEmitter;

    private void OnValidate()
    {
        if (volumeUpdatedEmitter == null)
        {
            Debug.LogWarning($"Volume updated emitter for {name} is null", gameObject);
        }
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.SFX_VOLUME_KEY, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.MUSIC_VOLUME_KEY, volume);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.MASTER_VOLUME_KEY, volume);
    }

    private void SetVolume(string key, float volume)
    {
        PlayerPrefs.SetFloat(key, volume);
        if (volumeUpdatedEmitter != null)
            volumeUpdatedEmitter.eventEmitter.Invoke(volume);
    }
}