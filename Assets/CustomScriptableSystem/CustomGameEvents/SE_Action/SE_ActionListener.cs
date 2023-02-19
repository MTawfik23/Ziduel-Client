using UnityEngine;
using UnityEngine.Events;
using System;


namespace Raskulls.ScriptableSystem
{
    public class SE_ActionListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Action Event ;
        public UnityEventReborn PreResponse;
        public UnityEventReborn Response;
        public UnityEventReborn PostResponse;
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }
        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }
        public void OnPreEventRaised(Action Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(Action Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(Action Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<Action> { }
    }
}
