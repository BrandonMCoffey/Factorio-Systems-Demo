using UnityEngine;

namespace Assets.Scripts.Grid {
    public enum Direction {
        North,
        East,
        South,
        West
    }

    [CreateAssetMenu]
    public class GridPlacement : ScriptableObject {
        public GridObject SelectedObject;
        public Direction Direction;
        public bool Active;

        public void SelectObject(GridObject gridObject)
        {
            SelectedObject = gridObject;
            Active = gridObject.Active;
        }
    }
}