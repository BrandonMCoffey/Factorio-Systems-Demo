using UnityEngine;

namespace Assets.Scripts {
    public class FollowTransform : MonoBehaviour {
        [SerializeField] private Transform _objectToFollow = null;
        [SerializeField] private float _smoothSpeed = 4;

        private Vector3 _offset;

        private void Awake()
        {
            if (_objectToFollow == null) return;
            _offset = transform.position - _objectToFollow.position;
        }

        private void Update()
        {
            if (_objectToFollow == null) return;
            Vector3 desiredPosition = _objectToFollow.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

            //if (Input.mouseScrollDelta != Vector2.zero) Debug.Log(Input.mouseScrollDelta);
        }
    }
}