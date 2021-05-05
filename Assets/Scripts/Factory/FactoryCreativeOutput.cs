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
                FactoryBelt belt = northFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.North) {
                    AddBelt(belt);
                }
            }
            FactoryObject eastFactory = Neighbors.GetNeighbor(Direction.East);
            if (eastFactory != null) {
                FactoryBelt belt = eastFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.East) {
                    AddBelt(belt);
                }
            }
            FactoryObject southFactory = Neighbors.GetNeighbor(Direction.South);
            if (southFactory != null) {
                FactoryBelt belt = southFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.South) {
                    AddBelt(belt);
                }
            }
            FactoryObject westFactory = Neighbors.GetNeighbor(Direction.West);
            if (westFactory != null) {
                FactoryBelt belt = westFactory.GetComponent<FactoryBelt>();
                if (belt != null && belt.Dir == Direction.West) {
                    AddBelt(belt);
                }
            }
        }

        public void AddBelt(FactoryBelt belt)
        {
            _outputBelts.Add(belt);
        }

        private void Update()
        {
            if (Item == null || Item.Item == null) return;
            foreach (var belt in _outputBelts) {
                belt.LeftInputSlot.GiveItem(Item.Item);
                belt.RightInputSlot.GiveItem(Item.Item);
            }
        }
    }
}