using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Grid {
    public class GridMap : MonoBehaviour {
        [SerializeField] private int _chunkWidth = 64;
        [SerializeField] private int _chunkHeight = 32;
        [SerializeField] private float _cellSize = 0.5f;
        [SerializeField] private Vector2 _center = Vector2.zero;
        [SerializeField] private GridObject _objectToPlace = null;

        private GridChunk _chunk;

        private void Start()
        {
            Transform textGroup = new GameObject("Grid Display").transform;
            _chunk = new GridChunk(_chunkWidth, _chunkHeight, _cellSize, _center, textGroup);
            _chunk.DrawGrid();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsMouseOverUI()) {
                _chunk.SetValue(GetMouseWorldPosition(Input.mousePosition, Camera.main), _objectToPlace);
            }
            if (Input.GetMouseButtonDown(1) && !IsMouseOverUI()) {
                _chunk.SetValue(GetMouseWorldPosition(Input.mousePosition, Camera.main), null, true);
            }
        }

        public void SelectObjectToPlace(GridObject o)
        {
            _objectToPlace = o;
        }

        private static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0;
            return worldPosition;
        }

        public static bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}