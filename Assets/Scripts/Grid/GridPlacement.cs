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
            if (PreviewPlacement == null) CreateNewPreview();
            PreviewPlacement.SetActive(true);
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
            if (PreviewPlacement != null) PreviewPlacement.SetActive(false);
        }

        public void CreateNewPreview()
        {
            if (PreviewPlacement != null) Destroy(PreviewPlacement);
            if (SelectedFactory == null) return;
            if (SelectedFactory.PreviewRenderer != null) {
                PreviewPlacement = Instantiate(SelectedFactory.PreviewRenderer.gameObject);
                PreviewPlacement.GetComponent<SpriteRenderer>().sprite = SelectedFactory.PreviewSprite;
            } else {
                GameObject obj = SelectedFactory.transform.Find("Art").gameObject;
                PreviewPlacement = Instantiate(obj != null ? obj : SelectedFactory.gameObject);
            }
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