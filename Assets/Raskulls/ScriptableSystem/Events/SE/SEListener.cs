using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SEListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE Event;
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
        public void OnPreEventRaised()
        {
            PreResponse.Invoke();
        }
        public void OnEventRaised()
        {
            Response.Invoke();
        }
        public void OnPostEventRaised()
        {
            PostResponse.Invoke();
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent { }
    }
}
