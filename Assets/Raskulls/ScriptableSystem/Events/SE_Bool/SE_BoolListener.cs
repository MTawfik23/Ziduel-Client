using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_BoolListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Bool Event;
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
        public void OnPreEventRaised(bool Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(bool Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(bool Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<bool> { }
    }
}
