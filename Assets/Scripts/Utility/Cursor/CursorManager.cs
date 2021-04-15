using UnityEngine;

namespace Assets.Scripts.Utility.Cursor {
    [RequireComponent(typeof(Camera))]
    public class CursorManager : MonoBehaviour {
        [SerializeField] private CursorSettings _cursorSettings = null;

        private CursorSprites _cursorSprites;
        private float _frameTimer;
        private int _currentFrame;

        private void OnEnable()
        {
            if (_cursorSettings == null) {
                Debug.Log("[CursorManager] Warning: No Cursor Settings attached to " + gameObject.name);
                return;
            }
            _cursorSettings.OnClicked += SetCursor;
            _currentFrame = 0;
            SetCursor(_cursorSettings.CursorSpriteList[0]);
        }

        private void OnDisable()
        {
            if (_cursorSettings == null) return;
            _cursorSettings.OnClicked -= SetCursor;
            UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void Update()
        {
            if (_cursorSprites == null || _cursorSprites.FrameRate == 0) return;
            _frameTimer -= Time.deltaTime;
            if (_frameTimer <= 0f) {
                _frameTimer += _cursorSprites.FrameRate;
                _currentFrame = (_currentFrame + 1) % _cursorSprites.TextureArray.Count;
                UnityEngine.Cursor.SetCursor(_cursorSprites.TextureArray[_currentFrame], _cursorSprites.Offset, CursorMode.Auto);
            }
        }

        private void SetCursor(CursorSprites cursorSprites)
        {
            if (cursorSprites == null || cursorSprites.TextureArray.Count == 0) {
                Debug.Log("[CursorManager] Warning: Attempting to set an invalid cursor");
                return;
            }
            _cursorSprites = cursorSprites;
            _frameTimer = Time.time + cursorSprites.FrameRate;
            UnityEngine.Cursor.SetCursor(cursorSprites.TextureArray[0], cursorSprites.Offset, CursorMode.Auto);
        }
    }
}