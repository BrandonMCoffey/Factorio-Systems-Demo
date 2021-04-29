using System.Collections.Generic;
using Assets.Scripts.Factory.Base;
using Assets.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryBelt : FactoryObject {
        public FactoryBeltItem[] LeftItems = new FactoryBeltItem[4];
        public FactoryBeltItem[] RightItems = new FactoryBeltItem[4];

        public override void Setup(GridObject gridObject, Direction dir)
        {
            base.Setup(gridObject, dir);
            if (Neighbors.GetFront() != null) {
                FactoryBelt frontBelt = Neighbors.GetFront().GetComponent<FactoryBelt>();
                if (frontBelt != null && Dir == frontBelt.Dir) {
                    Debug.Log("Added front neighboring belt");
                    LeftItems[0].NextPosition = frontBelt.LeftItems[3];
                    RightItems[0].NextPosition = frontBelt.RightItems[3];
                }
            }
            if (Neighbors.GetBack() != null) {
                FactoryBelt backBelt = Neighbors.GetBack().GetComponent<FactoryBelt>();
                if (backBelt != null && Dir == backBelt.Dir) {
                    Debug.Log("Added back neighboring belt");
                    backBelt.LeftItems[0].NextPosition = LeftItems[3];
                    backBelt.RightItems[0].NextPosition = RightItems[3];
                }
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            /*
            foreach (var item in LeftItems) {
                item.OnUpdate(deltaTime);
            }
            foreach (var item in RightItems) {
                item.OnUpdate(deltaTime);
            }
            */
        }
    }
}