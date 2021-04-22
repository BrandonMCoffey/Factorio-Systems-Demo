using UnityEngine;

namespace Assets.Scripts.Grid {
    public class GridChunk {
        private float _offsetX;
        private float _offsetY;
        private float _cellSize;

        private Vector2 _center;
        private Transform _textGroup;

        private GridElement[,] _grid;

        public GridChunk(int width, int height, float cellSize = 1, Vector2 center = default, Transform textGroup = null)
        {
            _cellSize = cellSize;
            _center = center;
            _textGroup = textGroup;

            _offsetX = -(width - 1) * cellSize / 2 - cellSize / 2;
            _offsetY = -(height - 1) * cellSize / 2 - cellSize / 2;

            _grid = new GridElement[width, height];

            for (int w = 0; w < _grid.GetLength(0); w++) {
                for (int h = 0; h < _grid.GetLength(1); h++) {
                    float x = _center.x + w * cellSize;
                    float y = _center.y + h * cellSize;
                    _grid[w, h] = new GridElement(x, y, textGroup);
                }
            }
            DrawGrid();
            Debug.DrawLine(new Vector3(_center.x + _offsetX, _center.y - _offsetY), new Vector3(_center.x - _offsetX, _center.y - _offsetY), Color.white, 100f);
            Debug.DrawLine(new Vector3(_center.x - _offsetX, _center.y + _offsetY), new Vector3(_center.x - _offsetX, _center.y - _offsetY), Color.white, 100f);
        }

        public void SetValue(Vector3 position, GridObject obj, Direction dir, bool overrideObject = false)
        {
            int x = Mathf.FloorToInt(((position - (Vector3) _center).x - _offsetX) / _cellSize);
            int y = Mathf.FloorToInt(((position - (Vector3) _center).y - _offsetY) / _cellSize);
            if (_grid[x, y].Obj == null || overrideObject) {
                _grid[x, y].Obj = obj;
                _grid[x, y].Dir = dir;
                DrawGrid();
            }
        }

        public void DrawGrid()
        {
            for (int i = _textGroup.childCount - 1; i >= 0; i--) {
                Object.Destroy(_textGroup.GetChild(i).gameObject);
            }
            for (int w = 0; w < _grid.GetLength(0); w++) {
                for (int h = 0; h < _grid.GetLength(1); h++) {
                    _grid[w, h].Draw(_cellSize, new Vector2(_offsetX, _offsetY));
                }
            }
        }
    }

    internal class GridElement {
        public float X;
        public float Y;
        public GridObject Obj;
        public Direction Dir;
        private Transform _parent;

        public GridElement(float x, float y, Transform p)
        {
            X = x;
            Y = y;
            _parent = p;
        }

        public void Draw(float cellSize = 1, Vector2 offset = default)
        {
            Debug.DrawLine(new Vector3(X + offset.x, Y + offset.y), new Vector3(X + offset.x + cellSize, Y + offset.y), Color.white, 100f);
            Debug.DrawLine(new Vector3(X + offset.x, Y + offset.y), new Vector3(X + offset.x, Y + offset.y + cellSize), Color.white, 100f);
            if (Obj != null) {
                Obj.CreateObject(new Vector3(X + offset.x + cellSize / 2, Y + offset.y + cellSize / 2), "Text_(" + X * cellSize + "," + Y * cellSize + ")", _parent, cellSize / 4);
                Obj.DebugText(16);
                Obj.DrawVisual(Dir);
            }
        }
    }
}