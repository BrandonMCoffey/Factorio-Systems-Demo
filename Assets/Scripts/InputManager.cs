using System;
using Assets.Scripts.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts {
    public class InputManager : MonoBehaviour {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GridPlacement _gridPlacement = null;
        [SerializeField] private KeyCode _rotateKey = KeyCode.R;
        [SerializeField] private bool _previewPlacement = true;

        private void Start()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_gridPlacement != null) _gridPlacement.SetDirection(Direction.North);
        }

        private void Update()
        {
            if (_gridPlacement == null || !_gridPlacement.Active) return;
            if (_previewPlacement) PreviewPlacement();
            if (Input.GetMouseButtonDown(0)) LeftClickRaycast();
            if (Input.GetMouseButtonDown(1)) RightClickRaycast();
            if (Input.GetKeyDown(_rotateKey)) Rotate();
        }

        private void LeftClickRaycast()
        {
            if (IsMouseOverUI()) return;
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
            if (IsMouseOverUI()) return;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, 100f);
            if (hit.collider == null) return;

            GridObject gridObject = hit.collider.GetComponent<GridObject>();
            if (gridObject != null) {
                gridObject.OnBreak();
            }
        }

        private void PreviewPlacement()
        {
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