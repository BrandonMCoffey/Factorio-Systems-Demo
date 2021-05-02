using System;
using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Grid {
    [RequireComponent(typeof(BoxCollider))]
    public class GridObject : MonoBehaviour {
        public GridObject NorthNeighbor = null;
        public GridObject EastNeighbor = null;
        public GridObject SouthNeighbor = null;
        public GridObject WestNeighbor = null;

        public GridChunk Chunk = null;

        public Position ChunkPosition;

        public FactoryObject FactoryObject;
        public GameObject FactoryVisual;

        private BoxCollider _collider;

        public void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.center = new Vector3(0.5f, 0.5f);
            _collider.size = new Vector3(1, 1, 0.1f);
        }

        public void OnSelect()
        {
            if (FactoryObject != null) FactoryObject.OnSelect();
        }

        public void OnBreak()
        {
            FactoryObject.OnBreak();
            if (NorthNeighbor != null) NorthNeighbor.SouthNeighbor = null;
            if (EastNeighbor != null) EastNeighbor.WestNeighbor = null;
            if (SouthNeighbor != null) SouthNeighbor.NorthNeighbor = null;
            if (WestNeighbor != null) WestNeighbor.EastNeighbor = null;
            Chunk.RemoveObject(ChunkPosition);
            Destroy(gameObject);
        }
    }

    public struct Position {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector3 GetLocalPosition()
        {
            return new Vector3(X, Y);
        }

        public static Quaternion GetRotationFromDirection(Direction dir)
        {
            return dir switch
            {
                Direction.North => Quaternion.Euler(0, 0, 0),
                Direction.East  => Quaternion.Euler(0, 0, -90),
                Direction.South => Quaternion.Euler(0, 0, -180),
                Direction.West  => Quaternion.Euler(0, 0, -270),
                _               => Quaternion.Euler(0, 0, 0)
            };
        }
    }

    public enum Direction {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
}