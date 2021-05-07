using UnityEngine;

namespace Assets.Scripts.Utility {
    public class FollowTransform : MonoBehaviour {
        [SerializeField] internal Transform ObjectToFollow = null;
        [SerializeField] internal float SmoothSpeed = 4;

        private void Update()
        {
            if (ObjectToFollow == null) return;

            Vector3 desiredPosition = ObjectToFollow.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        }
    }
}