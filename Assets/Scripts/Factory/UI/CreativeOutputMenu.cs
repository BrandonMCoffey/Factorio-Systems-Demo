using Assets.Scripts.Audio;
using Assets.Scripts.Factory.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Factory.UI {
    public class CreativeOutputMenu : MonoBehaviour {
        public ItemObject Item;
        public FactoryCreativeOutput CreativeOutput;

        [SerializeField] private Image _renderer = null;

        [Header("Audio")]
        [SerializeField] private AudioClipSettings _selectItem = null;
        [SerializeField] private AudioClipSettings _openMenu = null;
        [SerializeField] private AudioClipSettings _closeMenu = null;

        private void Awake()
        {
            if (Item != null) _renderer.sprite = Item.Sprite;
        }

        private void OnEnable()
        {
            AudioManager.Instance.PlaySound(_openMenu);
        }

        private void OnDisable()
        {
            AudioManager.Instance.PlaySound(_closeMenu);
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
            AudioManager.Instance.PlaySound(_selectItem);
        }
    }
}