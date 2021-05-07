using UnityEngine;

namespace Assets.Scripts.Audio {
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance;

        [SerializeField] private AudioClipSettings _buildSound = null;
        [SerializeField] private AudioClipSettings _deconstructSound = null;

        private AudioSource _source;

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
                _source = GetComponent<AudioSource>();
            } else {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClipSettings a)
        {
            if (a == null) return;
            _source.PlayOneShot(a.Clip, a.Volume);
        }

        public void PlayBuildSound()
        {
            PlaySound(_buildSound);
        }

        public void PlayDeconstructSound()
        {
            PlaySound(_deconstructSound);
        }
    }
}