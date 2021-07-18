using System.Collections.Generic;
using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Grid {
    public class GridChunk : MonoBehaviour {
        private const int Width = 16;
        private const int Height = 16;

        public GridChunk NorthNeighbor = null;
        public GridChunk EastNeighbor = null;
        public GridChunk SouthNeighbor = null;
        public GridChunk WestNeighbor = null;

        private Dictionary<Position, GridObject> _grid = new Dictionary<Position, GridObject>();
        private List<FactoryObject> _factoryObjects = new List<FactoryObject>();

        private BoxCollider _collider;

        private Transform _factoryLargeObjects;

        private void Start()
        {
            DrawGrid();
            _collider = gameObject.AddComponent<BoxCollider>();
            _collider.center = new Vector3(8, 8, 0.1f);
            _collider.size = new Vector3(Width, Height, 0.2f);
            _factoryLargeObjects = new GameObject("Large Factories").transform;
            _factoryLargeObjects.SetParent(transform);
            _factoryLargeObjects.transform.localPosition = Vector3.zero;
        }

        public void PreviewPlacement(GridPlacement placementData, Vector3 worldPos)
        {
            if (placementData.PreviewPlacement) placementData.CreateNewPreview();
            placementData.PreviewPlacement.transform.position = transform.position + WorldToChunkGrid(worldPos.x, worldPos.y).GetLocalPosition() + new Vector3(0.5f, 0.5f);
            placementData.EnablePreview();
        }

        public void PlaceObject(GridPlacement placementData, Vector3 worldPos)
        {
            int size = placementData.SelectedFactory.GridSize;

            if (size == 1) {
                AddObject(WorldToChunkGrid(worldPos.x, worldPos.y), placementData.Direction, placementData.SelectedFactory);
            } else if (size == 3) {
                Position basePos = WorldToChunkGrid(worldPos.x, worldPos.y);
                Direction dir = placementData.Direction;

                GridObject north;
                Position pos = basePos.GetRelativePosition(0, 2);
                if (pos.Y > Height && NorthNeighbor != null) {
                    pos = pos.GetRelativePosition(0, -Height);
                    north = NorthNeighbor._grid.ContainsKey(pos) ? NorthNeighbor._grid[pos] : null;
                } else {
                    north = _grid.ContainsKey(pos) ? _grid[pos] : null;
                }
                pos = basePos.GetRelativePosition(2, 0);
                GridObject east = _grid.ContainsKey(pos) ? _grid[pos] : null;
                pos = basePos.GetRelativePosition(0, -2);
                GridObject south = _grid.ContainsKey(pos) ? _grid[pos] : null;
                pos = basePos.GetRelativePosition(-2, 0);
                GridObject west = _grid.ContainsKey(pos) ? _grid[pos] : null;
                AddObject(basePos, dir, placementData.SelectedFactory, true, new FactoryNeighbors(north, east, south, west, dir));
                FactoryObject factoryObject = _grid[basePos].FactoryObject;

                AddObject(basePos.GetRelativePosition(0, 1), dir, factoryObject, false);
                AddObject(basePos.GetRelativePosition(1, 1), dir, null, false);
                AddObject(basePos.GetRelativePosition(1, 0), dir, factoryObject, false);
                AddObject(basePos.GetRelativePosition(1, -1), dir, null, false);
                AddObject(basePos.GetRelativePosition(0, -1), dir, factoryObject, false);
                AddObject(basePos.GetRelativePosition(-1, -1), dir, null, false);
                AddObject(basePos.GetRelativePosition(-1, 0), dir, factoryObject, false);
                AddObject(basePos.GetRelativePosition(-1, 1), dir, null, false);

                //List<GridObject> northNeighbors = new List<GridObject> {_grid[basePos.GetRelativePosition(-1, 2)], _grid[basePos.GetRelativePosition(0, 2)], _grid[basePos.GetRelativePosition(1, 2)]};
                //List<GridObject> eastNeighbors = new List<GridObject> {_grid[basePos.GetRelativePosition(2, -1)], _grid[basePos.GetRelativePosition(2, 0)], _grid[basePos.GetRelativePosition(2, 1)]};
                //List<GridObject> southNeighbors = new List<GridObject> {_grid[basePos.GetRelativePosition(-1, -2)], _grid[basePos.GetRelativePosition(0, -2)], _grid[basePos.GetRelativePosition(1, -2)]};
                //List<GridObject> westNeighbors = new List<GridObject> {_grid[basePos.GetRelativePosition(-2, -1)], _grid[basePos.GetRelativePosition(-2, 0)], _grid[basePos.GetRelativePosition(-2, 1)]};
            }
        }

        public void RemoveObject(Position pos)
        {
            _grid.Remove(pos);
        }

        private void AddObject(Position pos, Direction dir, FactoryObject factoryObject, bool instantiate = true, FactoryNeighbors customNeighbors = null)
        {
            if (_grid.ContainsKey(pos)) return;

            GridObject gridObject = new GameObject("Object(" + pos.X + "," + pos.Y + ")").AddComponent<GridObject>();
            gridObject.transform.SetParent(transform);
            gridObject.transform.localPosition = pos.GetLocalPosition();

            gridObject.NorthNeighbor = FindNeighbor(pos, Direction.North);
            gridObject.EastNeighbor = FindNeighbor(pos, Direction.East);
            gridObject.SouthNeighbor = FindNeighbor(pos, Direction.South);
            gridObject.WestNeighbor = FindNeighbor(pos, Direction.West);
            if (gridObject.NorthNeighbor != null) gridObject.NorthNeighbor.SouthNeighbor = gridObject;
            if (gridObject.EastNeighbor != null) gridObject.EastNeighbor.WestNeighbor = gridObject;
            if (gridObject.SouthNeighbor != null) gridObject.SouthNeighbor.NorthNeighbor = gridObject;
            if (gridObject.WestNeighbor != null) gridObject.WestNeighbor.EastNeighbor = gridObject;

            if (instantiate) {
                gridObject.FactoryObject = Instantiate(factoryObject, gridObject.transform).GetComponent<FactoryObject>();
                gridObject.FactoryObject.Setup(gridObject, dir, customNeighbors);
            } else {
                gridObject.FactoryObject = factoryObject;
            }
            if (gridObject.FactoryObject != null) gridObject.FactoryObject.ConnectedObjects.Add(gridObject);

            gridObject.Chunk = this;
            gridObject.ChunkPosition = pos;
            _grid.Add(pos, gridObject);
        }

        public GridObject FindNeighbor(Position pos, Direction dir)
        {
            switch (dir) {
                case Direction.North:
                    pos.Y++;
                    if (pos.Y >= Height) {
                        pos.Y = 0;
                        return NorthNeighbor._grid.ContainsKey(pos) ? NorthNeighbor._grid[pos] : null;
                    }
                    break;
                case Direction.East:
                    pos.X++;
                    if (pos.X >= Width) {
                        pos.X = 0;
                        return EastNeighbor._grid.ContainsKey(pos) ? EastNeighbor._grid[pos] : null;
                    }
                    break;
                case Direction.South:
                    pos.Y--;
                    if (pos.Y < 0) {
                        pos.Y = Height - 1;
                        return SouthNeighbor._grid.ContainsKey(pos) ? SouthNeighbor._grid[pos] : null;
                    }
                    break;
                case Direction.West:
                    pos.X--;
                    if (pos.X < 0) {
                        pos.X = Width - 1;
                        return WestNeighbor._grid.ContainsKey(pos) ? WestNeighbor._grid[pos] : null;
                    }
                    break;
            }

            return _grid.ContainsKey(pos) ? _grid[pos] : null;
        }

        public void DrawGrid()
        {
            int offsetX = Mathf.FloorToInt(transform.position.x);
            int offsetY = Mathf.FloorToInt(transform.position.y);
            // Draw Rows
            Debug.DrawLine(new Vector3(offsetX, offsetY), new Vector3(offsetX + Width, offsetY), Color.yellow, 100f);
            for (int y = offsetY + 1; y < offsetY + Height; y++) {
                Debug.DrawLine(new Vector3(offsetX, y), new Vector3(offsetX + Width, y), Color.white, 100f);
            }
            Debug.DrawLine(new Vector3(offsetX, offsetY + Height), new Vector3(offsetX + Width, offsetY + Height), Color.yellow, 100f);
            // Draw Columns
            Debug.DrawLine(new Vector3(offsetX, offsetY), new Vector3(offsetX, offsetY + Height), Color.yellow, 100f);
            for (int x = offsetX + 1; x < offsetX + Width; x++) {
                Debug.DrawLine(new Vector3(x, offsetY), new Vector3(x, offsetY + Height), Color.white, 100f);
            }
            Debug.DrawLine(new Vector3(offsetX + Width, offsetY), new Vector3(offsetX + Width, offsetY + Height), Color.yellow, 100f);
        }

        public Position WorldToChunkGrid(float worldX, float worldY)
        {
            return new Position {X = Mathf.FloorToInt(worldX - transform.position.x), Y = Mathf.FloorToInt(worldY - transform.position.y)};
        }
    }
}