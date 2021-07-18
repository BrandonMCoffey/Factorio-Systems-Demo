using UnityEngine;

namespace Assets.Scripts.Utility.Cursor {
    public class CursorObject : MonoBehaviour {
        public CursorSettings CursorSettings = null;
        public int CursorNumber = 0;

        private void OnMouseEnter()
        {
            if (CursorSettings != null) CursorSettings.SetCursor(CursorNumber);
        }

        private void OnMouseExit()
        {
            if (CursorSettings != null) CursorSettings.RevertCursor();
        }
    }
}