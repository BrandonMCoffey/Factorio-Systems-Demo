using UnityEngine;

namespace Assets.Scripts.Audio {
    [CreateAssetMenu]
    public class AudioClipSettings : ScriptableObject {
        public AudioClip Clip;
        [Range(0, 1)] public float Volume = 0.5f;
    }
}