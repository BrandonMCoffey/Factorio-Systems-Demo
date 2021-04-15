using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility.GameEvents.Logic {
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject {
        private List<GameEventListener> _eventListeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--) {
                _eventListeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (_eventListeners.Contains(listener)) return;
            _eventListeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener listener)
        {
            if (!_eventListeners.Contains(listener)) return;
            _eventListeners.Remove(listener);
        }
    }
}