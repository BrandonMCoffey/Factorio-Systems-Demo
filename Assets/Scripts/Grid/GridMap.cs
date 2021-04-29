using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Grid {
    public class GridMap : MonoBehaviour {
        private Dictionary<Position, GridChunk> _chunkGrid;

        private void Start()
        {
            _chunkGrid = new Dictionary<Position, GridChunk>();
            AddChunk(0, 0);
            AddChunk(-1, 0);
            AddChunk(-1, -1);
            AddChunk(0, -1);
        }

        public void AddChunk(int x, int y)
        {
            Position pos = new Position(x, y);
            if (_chunkGrid.ContainsKey(pos)) return;

            GridChunk gridChunk = new GameObject("Chunk(" + x + "," + y + ")").AddComponent<GridChunk>();
            gridChunk.transform.SetParent(transform);

            gridChunk.NorthNeighbor = FindNeighbor(pos, Direction.North);
            gridChunk.EastNeighbor = FindNeighbor(pos, Direction.East);
            gridChunk.SouthNeighbor = FindNeighbor(pos, Direction.South);
            gridChunk.WestNeighbor = FindNeighbor(pos, Direction.West);
            if (gridChunk.NorthNeighbor != null) gridChunk.NorthNeighbor.SouthNeighbor = gridChunk;
            if (gridChunk.EastNeighbor != null) gridChunk.EastNeighbor.WestNeighbor = gridChunk;
            if (gridChunk.SouthNeighbor != null) gridChunk.SouthNeighbor.NorthNeighbor = gridChunk;
            if (gridChunk.WestNeighbor != null) gridChunk.WestNeighbor.EastNeighbor = gridChunk;

            Vector3 worldPos = Vector3.zero;
            worldPos.x = x * 16;
            worldPos.y = y * 16;
            gridChunk.transform.position = worldPos;
            _chunkGrid.Add(pos, gridChunk);
        }

        private GridChunk FindNeighbor(Position pos, Direction dir)
        {
            switch (dir) {
                case Direction.North:
                    pos.Y++;
                    break;
                case Direction.East:
                    pos.X++;
                    break;
                case Direction.South:
                    pos.Y--;
                    break;
                case Direction.West:
                    pos.X--;
                    break;
            }

            return _chunkGrid.ContainsKey(pos) ? _chunkGrid[pos] : null;
        }
    }
}