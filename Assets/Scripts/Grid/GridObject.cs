using System;
using UnityEngine;

namespace Assets.Scripts.Grid {
    [CreateAssetMenu]
    public class GridObject : ScriptableObject {
        public string DebugString;
        public GameObject Visual;
        public bool Active;

        private Transform _transform;

        public void DrawVisual(Direction dir)
        {
            if (Visual == null) return;
            Transform modelObj = Instantiate(Visual).transform;
            modelObj.SetParent(_transform);
            modelObj.localPosition = Vector3.zero;
            modelObj.eulerAngles = dir switch
            {
                Direction.North => new Vector3(0, 0, 0),
                Direction.East  => new Vector3(0, 0, 270),
                Direction.South => new Vector3(0, 0, 180),
                Direction.West  => new Vector3(0, 0, 90),
                _               => new Vector3(0, 0, 0)
            };
        }

        public void DebugText(int fontSize = 20)
        {
            TextMesh textMesh = _transform.GetComponent<TextMesh>();
            if (textMesh == null) textMesh = _transform.gameObject.AddComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.text = DebugString;
            textMesh.fontSize = fontSize;
            textMesh.color = Color.white;
        }

        public void CreateObject(Vector3 position, string objName = "WorldText", Transform parent = null, float scale = 1)
        {
            _transform = new GameObject(objName).transform;
            _transform.SetParent(parent, false);
            _transform.position = position;
            _transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}