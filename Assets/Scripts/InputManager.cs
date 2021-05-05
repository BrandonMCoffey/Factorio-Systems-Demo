using Assets.Scripts.Factory.UI;
using Assets.Scripts.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts {
    public class InputManager : MonoBehaviour {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GridPlacement _gridPlacement = null;
        [SerializeField] private KeyCode _rotateKey = KeyCode.R;
        [SerializeField] private KeyCode _closeMenuKey = KeyCode.Escape;
        [SerializeField] private bool _previewPlacement = true;

        private void Start()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_gridPlacement != null) _gridPlacement.SetDirection(Direction.North);
        }

        private void Update()
        {
            if (_gridPlacement == null || !_gridPlacement.Active) return;
            bool mouseAvailable = !IsMouseOverUI();
            if (_previewPlacement && _gridPlacement.Active) PreviewPlacement(mouseAvailable);
            if (Input.GetMouseButtonDown(0) && mouseAvailable) LeftClickRaycast();
            if (Input.GetMouseButtonDown(1) && mouseAvailable) RightClickRaycast();
            if (Input.GetKeyDown(_rotateKey)) Rotate();
            if (Input.GetKeyDown(_closeMenuKey)) {
                FactoryUIController.Instance.CloseMenu();
            }
        }

        private void LeftClickRaycast()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, 100f);
            if (hit.collider == null) return;

            GridChunk gridChunk = hit.collider.GetComponent<GridChunk>();
            if (gridChunk != null) {
                gridChunk.PlaceObject(_gridPlacement, hit.point);
            }
            GridObject gridObject = hit.collider.GetComponent<GridObject>();
            if (gridObject != null) {
                gridObject.OnSelect();
            }
        }

        private void RightClickRaycast()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, 100f);
            if (hit.collider == null) return;

            GridObject gridObject = hit.collider.GetComponent<GridObject>();
            if (gridObject != null) {
                gridObject.OnBreak();
            }
        }

        private void PreviewPlacement(bool active = false)
        {
            if (!active) {
                _gridPlacement.DisablePreview();
                return;
            }
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, 100f);
            if (hit.collider == null) return;

            GridChunk gridChunk = hit.collider.GetComponent<GridChunk>();
            if (gridChunk != null) {
                gridChunk.PreviewPlacement(_gridPlacement, hit.point);
            } else {
                _gridPlacement.DisablePreview();
            }
        }

        public static bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void Rotate()
        {
            _gridPlacement.SetDirection(_gridPlacement.Direction switch
            {
                Direction.North => Direction.East,
                Direction.East  => Direction.South,
                Direction.South => Direction.West,
                Direction.West  => Direction.North,
                _               => Direction.North
            });
        }
    }
}