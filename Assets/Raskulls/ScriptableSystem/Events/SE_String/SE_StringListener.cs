using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_StringListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_String Event;
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
        public void OnPreEventRaised(string Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(string Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(string Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<string> { }
    }
}
