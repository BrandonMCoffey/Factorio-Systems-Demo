using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Factory.UI {
    public class FactoryUIController : MonoBehaviour {
        public static FactoryUIController Instance;
        public FactoryObject FactoryObject;

        [SerializeField] private CreativeOutputMenu _creativeOutputMenu = null;

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public void SetFactoryObject(FactoryObject obj)
        {
            FactoryCreativeOutput creativeOutput = obj.GetComponent<FactoryCreativeOutput>();
            if (creativeOutput != null) {
                _creativeOutputMenu.gameObject.SetActive(true);
                _creativeOutputMenu.Setup(creativeOutput);
            }
        }

        public bool CloseMenu()
        {
            if (_creativeOutputMenu.isActiveAndEnabled) {
                _creativeOutputMenu.gameObject.SetActive(false);
                return true;
            }
            return false;
        }
    }
}