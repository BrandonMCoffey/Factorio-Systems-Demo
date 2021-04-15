using UnityEngine;

namespace Assets.Scripts.Utility.GameEvents.Logic {
    public class GameEventListener : MonoBehaviour {
        public GameEvent Event;

        private void OnEnable()
        {
            if (Event != null) Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (Event != null) Event.UnRegisterListener(this);
        }

        public virtual void OnEventRaised()
        {
        }
    }
}