using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Factory {
    [RequireComponent(typeof(SpriteRenderer))]
    public class FactoryItem : MonoBehaviour {
        public ItemObject Item;

        private SpriteRenderer _spriteRenderer;
        private Sprite _baseSprite;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _baseSprite = _spriteRenderer.sprite;
            if (Item != null) _spriteRenderer.sprite = Item.Sprite;
        }

        public void SetItem(ItemObject item)
        {
            Item = item;
            _spriteRenderer.sprite = Item.Sprite;
        }

        public void RemoveItem()
        {
            Item = null;
            _spriteRenderer.sprite = _baseSprite;
        }
    }
}