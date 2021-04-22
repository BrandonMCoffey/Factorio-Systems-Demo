using UnityEngine;

namespace Assets.Scripts {
    public class Player : MonoBehaviour {
        [SerializeField] private float _moveSpeed = 4;

        private Vector3 _movementDir = Vector3.zero;

        private void Update()
        {
            _movementDir.x = Input.GetAxisRaw("Horizontal") * _moveSpeed;
            _movementDir.y = Input.GetAxisRaw("Vertical") * _moveSpeed;

            transform.position += _movementDir * Time.deltaTime;
        }
    }
}