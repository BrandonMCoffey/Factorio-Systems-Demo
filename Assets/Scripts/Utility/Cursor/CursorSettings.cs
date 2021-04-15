using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility.Cursor {
    [CreateAssetMenu]
    public class CursorSettings : ScriptableObject {
        public List<CursorSprites> CursorSpriteList;

        public event Action<CursorSprites> OnClicked = delegate { };

        private int _baseNumber;

        public void SetCursor(int listNumber)
        {
            OnClicked.Invoke(CursorSpriteList[listNumber]);
        }

        public void RevertCursor(bool revertBase = false)
        {
            if (revertBase) SetBase(0);
            OnClicked.Invoke(CursorSpriteList[_baseNumber]);
        }

        public void SetBase(int baseNumber)
        {
            _baseNumber = baseNumber;
        }
    }

    [System.Serializable]
    public class CursorSprites {
        public List<Texture2D> TextureArray;
        public float FrameRate;
        public Vector2 Offset;
    }
}