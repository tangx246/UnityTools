using UnityEngine;

namespace UnityTools
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioClips : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        public AudioClip[] audioClips;

        // Start is called before the first frame update
        void OnValidate()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayClip(int clip)
        {
            audioSource.PlayOneShot(audioClips[clip]);
        }
    }
}