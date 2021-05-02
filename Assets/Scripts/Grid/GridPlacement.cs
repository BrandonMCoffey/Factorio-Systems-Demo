using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Grid {
    [CreateAssetMenu]
    public class GridPlacement : ScriptableObject {
        public FactoryObject SelectedFactory;
        public Direction Direction;
        public bool Active;

        public GameObject PreviewPlacement;
        public Material PreviewMaterial;

        public void SetFactory(FactoryObject factoryObject)
        {
            SelectedFactory = factoryObject;
            Active = true;
        }

        public void SetDirection(Direction dir)
        {
            Direction = dir;
            CreateNewPreview();
        }

        public void DisableFactoryPlacement()
        {
            SelectedFactory = null;
            Active = false;
        }

        public void CreateNewPreview()
        {
            if (PreviewPlacement != null) Destroy(PreviewPlacement);
            PreviewPlacement = Instantiate(SelectedFactory.Art);
            PreviewPlacement.gameObject.name = "PreviewFactoryPlacement";
            PreviewPlacement.transform.rotation = Position.GetRotationFromDirection(Direction);
            foreach (Transform obj in PreviewPlacement.transform) {
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                if (meshRenderer != null) {
                    meshRenderer.material = PreviewMaterial;
                }
            }
        }

        public void EnablePreview()
        {
            PreviewPlacement.SetActive(true);
        }

        public void DisablePreview()
        {
            PreviewPlacement.SetActive(false);
        }
    }
}