using System.Collections.Generic;
using Assets.Scripts.Factory.Base;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryCreativeOutput : FactoryObject {
        public FactoryItem Item = null;

        private List<FactoryBelt> _outputBelts = new List<FactoryBelt>();

        public override void Setup(GridObject gridObject, Direction dir)
        {
            base.Setup(gridObject, dir);
            FactoryObject northFactory = Neighbors.GetNeighbor(Direction.North);
            if (northFactory != null) {
                Debug.Log("Found north belt");
                FactoryBelt belt = northFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.North) {
                    Debug.Log("Added north belt");
                    _outputBelts.Add(belt);
                }
            }
            FactoryObject eastFactory = Neighbors.GetNeighbor(Direction.East);
            if (eastFactory != null) {
                Debug.Log("Found East belt");
                FactoryBelt belt = eastFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.East) {
                    Debug.Log("Added East belt");
                    _outputBelts.Add(belt);
                }
            }
            FactoryObject southFactory = Neighbors.GetNeighbor(Direction.South);
            if (southFactory != null) {
                Debug.Log("Found South belt");
                FactoryBelt belt = southFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.South) {
                    Debug.Log("Added South belt");
                    _outputBelts.Add(belt);
                }
            }
            FactoryObject westFactory = Neighbors.GetNeighbor(Direction.West);
            if (westFactory != null) {
                Debug.Log("Found West belt");
                FactoryBelt belt = westFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.West) {
                    Debug.Log("Added West belt");
                    _outputBelts.Add(belt);
                }
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public void Update()
        {
            if (Item == null || Item.Item == null) return;
            foreach (var belt in _outputBelts) {
                belt.LeftItems[3].GiveItem(Item.Item);
                belt.RightItems[3].GiveItem(Item.Item);
            }
        }
    }
}