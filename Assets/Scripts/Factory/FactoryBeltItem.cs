using Assets.Scripts.Factory.Base;
using UnityEngine;

namespace Assets.Scripts.Factory {
    public class FactoryBeltItem : FactoryItem {
        public FactoryBeltItem NextPosition;
        public FactoryBeltItem PreviousPosition;
        public Vector3 PathForAnimation = new Vector3(0, 0.25f);
        public float MaxOffset = 0.4f;
        public float Offset;
        public float Speed = 0.5f;

        private Vector3 _basePosition;
        private bool _freeze;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            BaseSprite = SpriteRenderer.sprite;
            if (Item != null) SpriteRenderer.sprite = Item.Sprite;
            _basePosition = transform.localPosition;
        }

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        private void Update()
        {
            if (_freeze || Item == null || (NextPosition == null && Offset >= MaxOffset)) return;
            Offset += Time.deltaTime * Speed;
            if (Offset >= MaxOffset) {
                Offset = MaxOffset;
                if (NextPosition != null) {
                    if (NextPosition.Item == null) {
                        MoveItem();
                    } else {
                        _freeze = true;
                    }
                }
            }
            transform.localPosition = _basePosition - PathForAnimation + PathForAnimation * Offset / MaxOffset;
        }

        public override void SetItem(ItemObject item)
        {
            base.SetItem(item);
            Offset = 0;
        }

        public override void RemoveItem()
        {
            base.RemoveItem();
            Offset = 0;
            UpdatePreviousItem();
        }

        public void GiveItem(ItemObject item)
        {
            if (Item == null) SetItem(item);
        }

        public void MoveItem()
        {
            if (NextPosition == null) return;
            Offset = MaxOffset;
            if (NextPosition.Item == null) {
                NextPosition.SetItem(Item);
                RemoveItem();
            }
        }

        private void UpdatePreviousItem()
        {
            if (PreviousPosition != null) PreviousPosition._freeze = false;
        }

        public void OnBreakInput()
        {
            if (PreviousPosition != null) {
                PreviousPosition.NextPosition = null;
                PreviousPosition._freeze = false;
            }
        }

        public void OnBreakOutput()
        {
            if (NextPosition != null) NextPosition.PreviousPosition = null;
        }
    }
}