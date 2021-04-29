using Assets.Scripts.Factory.Base;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Assets.Scripts.Factory {
    [RequireComponent(typeof(SpriteRenderer))]
    public class FactoryBeltItem : MonoBehaviour {
        public FactoryBeltItem NextPosition = null;
        public FactoryBeltItem PreviousPosition = null;
        public ItemObject Item;
        public float DistToNextPosition = 0.25f;
        public float MaxOffset = 0.4f;
        public float Offset;
        public float Speed = 0.5f;

        private SpriteRenderer _spriteRenderer;
        private Sprite _baseSprite;

        private Vector3 _basePosition;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _baseSprite = _spriteRenderer.sprite;
            if (Item != null) _spriteRenderer.sprite = Item.Sprite;
            _basePosition = transform.localPosition;
        }

        public void Update()
        {
            if (Item == null || NextPosition == null || NextPosition.Item != null) return;
            Offset += Time.deltaTime * Speed;
            // Move sprite along towards DistToNextPosition
            transform.localPosition = _basePosition + new Vector3(0, DistToNextPosition * Offset / MaxOffset, 0);
            if (Offset > MaxOffset) {
                Offset = MaxOffset;
                if (NextPosition != null && NextPosition.Item == null) {
                    NextPosition.SetItem(Item);
                    RemoveItem();
                }
            }
            // Add Optimization : CanMove variable that is updated by Neighbors
        }

        public void SetItem(ItemObject item)
        {
            Item = item;
            Offset = 0;
            _spriteRenderer.sprite = Item.Sprite;
        }

        public void RemoveItem()
        {
            Item = null;
            Offset = 0;
            _spriteRenderer.sprite = _baseSprite;
        }

        public void GiveItem(ItemObject item)
        {
            if (Item == null) SetItem(item);
        }
    }
}