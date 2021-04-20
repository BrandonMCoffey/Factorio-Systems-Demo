using UnityEngine;

namespace Assets.Scripts.Grid {
    public enum Direction {
        North,
        East,
        South,
        West
    }

    [CreateAssetMenu]
    public class GridObject : ScriptableObject {
        public string DebugString;
        public Direction Direction;
        public GameObject Visual;

        private Transform _transform;

        public void DrawVisual()
        {
            if (Visual == null) return;
            Transform modelObj = Instantiate(Visual).transform;
            modelObj.SetParent(_transform);
            modelObj.localPosition = Vector3.zero;
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
            if (_transform == null) return;
            _transform = new GameObject(objName).transform;
            _transform.SetParent(parent, false);
            _transform.position = position;
            _transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}