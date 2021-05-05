using Assets.Scripts.Factory.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Factory.UI {
    public class CreativeOutputMenu : MonoBehaviour {
        public ItemObject Item;
        public FactoryCreativeOutput CreativeOutput;

        [SerializeField] private Image _renderer = null;

        private void Awake()
        {
            if (Item != null) _renderer.sprite = Item.Sprite;
        }

        public void Setup(FactoryCreativeOutput creativeOutput)
        {
            CreativeOutput = creativeOutput;
            SetItem(creativeOutput.Item.Item);
        }

        public void SetItem(ItemObject item)
        {
            if (item == null) return;
            Item = item;
            if (_renderer != null) _renderer.sprite = item.Sprite;
            if (CreativeOutput != null) CreativeOutput.SetItem(item);
        }
    }
}