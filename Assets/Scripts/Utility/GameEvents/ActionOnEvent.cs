using Assets.Scripts.Utility.GameEvents.Logic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Utility.GameEvents {
    public class ActionOnEvent : GameEventListener {
        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        public override void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}