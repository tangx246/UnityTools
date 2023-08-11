using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityTools
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioClips : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public AudioClipsDictionary audioClips;
        public UnityEvent<string> audioClipPlayed;

        // Allow audioClips to display in the inspector
        [Serializable] public class AudioClipsDictionary : SerializableDictionary<string, AudioClip> { }

        public void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayClip(string clip)
        {
            PlayClip(clip, false, 10f);
        }

        public void PlayClipNewObj(string clip)
        {
            PlayClip(clip, true, 10f);
        }

        public void PlayClip(string clip, bool newObj = false, float newObjTimedDestroy = 10f)
        {
            AudioSource audioSource;
            if (newObj)
            {
                var soundObj = new GameObject("Sound");
                soundObj.transform.position = transform.position;
                audioSource = soundObj.AddComponent<AudioSource>();
                audioSource.volume = this.audioSource.volume;
                audioSource.pitch = this.audioSource.pitch;
                audioSource.spatialBlend = this.audioSource.spatialBlend;
                audioSource.rolloffMode = this.audioSource.rolloffMode;
                audioSource.maxDistance = this.audioSource.maxDistance;
                audioSource.minDistance = this.audioSource.minDistance;
                audioSource.dopplerLevel = this.audioSource.dopplerLevel;
                audioSource.spread = this.audioSource.spread;
                audioSource.panStereo = this.audioSource.panStereo;
                audioSource.spatialize = this.audioSource.spatialize;
                audioSource.spatializePostEffects = this.audioSource.spatializePostEffects;
                audioSource.reverbZoneMix = this.audioSource.reverbZoneMix;
                audioSource.bypassEffects = this.audioSource.bypassEffects;
                audioSource.bypassListenerEffects = this.audioSource.bypassListenerEffects;
                audioSource.bypassReverbZones = this.audioSource.bypassReverbZones;
                audioSource.priority = this.audioSource.priority;
                audioSource.outputAudioMixerGroup = this.audioSource.outputAudioMixerGroup;

                var timedDestroy = soundObj.AddComponent<TimedDestroy>();
                timedDestroy.enabled = false;
                timedDestroy.maxLifeSeconds = newObjTimedDestroy;
                timedDestroy.enabled = true;
            } else
            {
                audioSource = this.audioSource;
            }

            audioSource.PlayOneShot(audioClips[clip]);
            audioClipPlayed.Invoke(clip);
        }
    }
}