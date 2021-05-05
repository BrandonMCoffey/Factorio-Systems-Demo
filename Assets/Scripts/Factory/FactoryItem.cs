using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Factory {
    [RequireComponent(typeof(SpriteRenderer))]
    public class FactoryItem : MonoBehaviour {
        public ItemObject Item;

        internal SpriteRenderer SpriteRenderer;
        internal Sprite BaseSprite;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            BaseSprite = SpriteRenderer.sprite;
            if (Item != null) SpriteRenderer.sprite = Item.Sprite;
        }

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public virtual void SetItem(ItemObject item)
        {
            if (item == null) return;
            Item = item;
            SpriteRenderer.sprite = item.Sprite;
        }

        public virtual void RemoveItem()
        {
            Item = null;
            SpriteRenderer.sprite = BaseSprite;
        }
    }
}