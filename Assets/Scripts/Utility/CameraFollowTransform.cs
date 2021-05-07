using UnityEngine;

namespace Assets.Scripts.Utility {
    [RequireComponent(typeof(Camera))]
    public class CameraFollowTransform : FollowTransform {
        [SerializeField] private Vector2 _zMinMax = new Vector2(4, 25);

        private float _initialZ;
        private float _zOffset = 10;
        private Camera _camera;

        private void Awake()
        {
            _initialZ = transform.position.z;
            _zOffset = Mathf.Abs(_initialZ);
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (ObjectToFollow == null) return;

            Vector3 desiredPosition = ObjectToFollow.position;

            _zOffset -= Input.GetAxis("Mouse ScrollWheel");
            _zOffset = Mathf.Clamp(_zOffset, _zMinMax.x, _zMinMax.y);
            if (_camera.orthographic) {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zOffset, SmoothSpeed * Time.deltaTime);
                desiredPosition.z = _initialZ;
            } else {
                desiredPosition.z = -_zOffset;
            }

            transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        }
    }
}