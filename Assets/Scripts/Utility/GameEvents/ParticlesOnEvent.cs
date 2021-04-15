using Assets.Scripts.Utility.GameEvents.Logic;
using Assets.Scripts.Utility.TransformRef;
using UnityEngine;

namespace Assets.Scripts.Utility.GameEvents {
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesOnEvent : GameEventListener {
        [Tooltip("How many particles to emit when Event is raised.")]
        [SerializeField] private int _particlesToEmit = 10;

        [SerializeField] private TransformReference _locationToEmit = null;
        [SerializeField] private Vector3 _transformAdjust = Vector3.zero;

        [SerializeField] private bool _useDirection = false;
        [SerializeField] private Vector3 _rotationAdjust = Vector3.zero;

        private ParticleSystem _system;

        private void Awake()
        {
            _system = GetComponent<ParticleSystem>();
        }

        public override void OnEventRaised()
        {
            if (_locationToEmit == null) return;
            transform.position = _locationToEmit.Position + _transformAdjust;
            if (_useDirection) {
                transform.rotation = _locationToEmit.Rotation * Quaternion.Euler(_rotationAdjust);
            }
            _system.Emit(_particlesToEmit);
        }
    }
}