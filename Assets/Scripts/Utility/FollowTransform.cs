using UnityEngine;

namespace Assets.Scripts.Utility {
    public class FollowTransform : MonoBehaviour {
        [SerializeField] private Transform _objectToFollow = null;
        [SerializeField] private float _zOffset = 10;
        [SerializeField] private Vector2 _zMinMax = new Vector2(5, 25);
        [SerializeField] private float _smoothSpeed = 4;

        private void Update()
        {
            if (_objectToFollow == null) return;

            _zOffset -= Input.GetAxis("Mouse ScrollWheel");
            _zOffset = Mathf.Clamp(_zOffset, _zMinMax.x, _zMinMax.y);

            Vector3 desiredPosition = _objectToFollow.position;
            desiredPosition.z = -_zOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        }
    }
}