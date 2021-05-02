using System;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory.Base {
    public class FactoryObject : MonoBehaviour {
        public GameObject Art;
        public Direction Dir;
        internal FactoryNeighbors Neighbors;

        public virtual void Setup(GridObject gridObject, Direction dir)
        {
            transform.localPosition = new Vector3(0.5f, 0.5f);
            transform.rotation = Position.GetRotationFromDirection(dir);
            Neighbors = new FactoryNeighbors(gridObject, dir);
            Dir = dir;
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public void OnSelect()
        {
            Debug.Log("Selected");
        }

        public virtual void OnBreak()
        {
            Destroy(gameObject);
        }
    }

    internal class FactoryNeighbors {
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
                Direction.North => _east,
                Direction.East  => _south,
                Direction.South => _west,
                Direction.West  => _north,
                _               => null
            };
        }
    }
}