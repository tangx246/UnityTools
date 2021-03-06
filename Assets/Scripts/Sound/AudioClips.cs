﻿using System;
using UnityEditor;
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
            audioSource.PlayOneShot(audioClips[clip]);
            audioClipPlayed.Invoke(clip);
        }
    }
}