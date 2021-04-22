using System;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts {
    public class InputManager : MonoBehaviour {
        [SerializeField] private GridPlacement _gridPlacement = null;
        [SerializeField] private KeyCode _rotateKey = KeyCode.R;

        private void Update()
        {
            if (_gridPlacement == null) return;
            if (Input.GetKeyDown(_rotateKey)) {
                _gridPlacement.Direction = _gridPlacement.Direction switch
                {
                    Direction.North => Direction.East,
                    Direction.East  => Direction.South,
                    Direction.South => Direction.West,
                    Direction.West  => Direction.North,
                    _               => Direction.North
                };
            }
        }
    }
}