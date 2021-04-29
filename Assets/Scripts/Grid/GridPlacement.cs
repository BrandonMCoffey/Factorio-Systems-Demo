using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Grid {
    [CreateAssetMenu]
    public class GridPlacement : ScriptableObject {
        public FactoryObject SelectedFactory;
        public Direction Direction;
        public bool Active;

        public void SetFactory(FactoryObject factoryObject)
        {
            SelectedFactory = factoryObject;
            Active = true;
        }

        public void DisableFactoryPlacement()
        {
            SelectedFactory = null;
            Active = false;
        }
    }
}