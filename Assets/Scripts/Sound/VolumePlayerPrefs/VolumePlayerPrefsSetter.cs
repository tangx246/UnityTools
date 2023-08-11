using UnityEngine;
using UnityEngine.UI;
using UnityTools;

public class VolumePlayerPrefsSetter : MonoBehaviour
{
    public AudioSourcePlayerPrefsLink.SoundVolumeType soundVolumeType;
    public EventEmitter<float> volumeUpdatedEmitter;
    public Slider slider;

    private void OnValidate()
    {
        if (volumeUpdatedEmitter == null)
        {
            Debug.LogWarning($"Volume updated emitter for {name} is null", gameObject);
        }
    }

    private void Awake()
    {
        if (slider == null)
        {
            // Try our best here
            slider = GetComponent<Slider>();
        }

        if (slider != null && volumeUpdatedEmitter != null)
        {
            volumeUpdatedEmitter.eventEmitter.AddListener(UpdateSlider);
            UpdateSlider(PlayerPrefs.GetFloat(AudioSourcePlayerPrefsLink.GetKey(soundVolumeType), 1f));
        }
    }

    private void OnDestroy()
    {
        if (volumeUpdatedEmitter != null)
        {
            volumeUpdatedEmitter.eventEmitter.RemoveListener(UpdateSlider);
        }
    }

    private void UpdateSlider(float volume)
    {
        if (slider != null)
            slider.value = volume;
    }

    public void SetVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.GetKey(soundVolumeType), volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.GetKey(AudioSourcePlayerPrefsLink.SoundVolumeType.SFX), volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.GetKey(AudioSourcePlayerPrefsLink.SoundVolumeType.Music), volume);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(AudioSourcePlayerPrefsLink.GetKey(AudioSourcePlayerPrefsLink.SoundVolumeType.Master), volume);
    }

    private void SetVolume(string key, float volume)
    {
        PlayerPrefs.SetFloat(key, volume);
        if (volumeUpdatedEmitter != null)
            volumeUpdatedEmitter.eventEmitter.Invoke(volume);
    }
}