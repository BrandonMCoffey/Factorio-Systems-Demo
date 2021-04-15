using UnityEngine;

namespace Assets.Scripts.Utility.Cursor {
    public class CursorObject : MonoBehaviour {
        [SerializeField] private CursorSettings _cursorSettings = null;
        [SerializeField] private int _cursorNumber = 0;

        private void OnMouseEnter()
        {
            if (_cursorSettings != null) _cursorSettings.SetCursor(_cursorNumber);
        }

        private void OnMouseExit()
        {
            if (_cursorSettings != null) _cursorSettings.RevertCursor();
        }
    }
}