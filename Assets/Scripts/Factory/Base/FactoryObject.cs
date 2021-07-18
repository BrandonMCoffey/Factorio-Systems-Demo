using System.Collections.Generic;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory.Base {
    public class FactoryObject : MonoBehaviour {
        public Sprite PreviewSprite;
        public SpriteRenderer PreviewRenderer = null;
        public Direction Dir;
        public int GridSize = 1;
        internal FactoryNeighbors Neighbors;

        public List<GridObject> ConnectedObjects = new List<GridObject>();

        public virtual void Setup(GridObject gridObject, Direction dir, FactoryNeighbors customNeighbors = null)
        {
            transform.localPosition = new Vector3(0.5f, 0.5f);
            transform.rotation = Position.GetRotationFromDirection(dir);
            Neighbors = customNeighbors ?? new FactoryNeighbors(gridObject, dir);
            Dir = dir;
        }

        public void CustomNeighbors(GridObject north, GridObject east, GridObject south, GridObject west, Direction dir)
        {
            Neighbors = new FactoryNeighbors(north, east, south, west, dir);
        }

        public virtual void OnSelect()
        {
        }

        public virtual void OnBreak()
        {
            foreach (var obj in ConnectedObjects) {
                obj.Deconstruct();
            }
            Destroy(gameObject);
        }
    }

    public class FactoryNeighbors {
        private FactoryObject _north;
        private FactoryObject _east;
        private FactoryObject _south;
        private FactoryObject _west;
        private Direction _facing;

        public FactoryNeighbors(GridObject obj, Direction facing)
        {
            _north = obj.NorthNeighbor != null ? obj.NorthNeighbor.FactoryObject : null;
            _east = obj.EastNeighbor != null ? obj.EastNeighbor.FactoryObject : null;
            _south = obj.SouthNeighbor != null ? obj.SouthNeighbor.FactoryObject : null;
            _west = obj.WestNeighbor != null ? obj.WestNeighbor.FactoryObject : null;
            _facing = facing;
        }

        public FactoryNeighbors(GridObject north, GridObject east, GridObject south, GridObject west, Direction facing)
        {
            _north = north != null ? north.FactoryObject : null;
            _east = east != null ? east.FactoryObject : null;
            _south = south != null ? south.FactoryObject : null;
            _west = west != null ? west.FactoryObject : null;
            _facing = facing;
        }

        public FactoryObject GetNeighbor(Direction dir)
        {
            return dir switch
            {
                Direction.North => _north,
                Direction.East  => _east,
                Direction.South => _south,
                Direction.West  => _west,
                _               => null
            };
        }

        public FactoryObject GetSide(Direction dir)
        {
            return dir switch
            {
                Direction.North => GetFront(),
                Direction.East  => GetRight(),
                Direction.South => GetBack(),
                Direction.West  => GetLeft(),
                _               => GetFront()
            };
        }

        public FactoryObject GetFront()
        {
            return _facing switch
            {
                Direction.North => _north,
                Direction.East  => _east,
                Direction.South => _south,
                Direction.West  => _west,
                _               => null
            };
        }

        public FactoryObject GetRight()
        {
            return _facing switch
            {
                Direction.North => _east,
                Direction.East  => _south,
                Direction.South => _west,
                Direction.West  => _north,
                _               => null
            };
        }

        public FactoryObject GetBack()
        {
            return _facing switch
            {
                Direction.North => _south,
                Direction.East  => _west,
                Direction.South => _north,
                Direction.West  => _east,
                _               => null
            };
        }

        public FactoryObject GetLeft()
        {
            return _facing switch
            {
                Direction.North => _west,
                Direction.East  => _north,
                Direction.South => _east,
                Direction.West  => _south,
                _               => null
            };
        }
    }
}