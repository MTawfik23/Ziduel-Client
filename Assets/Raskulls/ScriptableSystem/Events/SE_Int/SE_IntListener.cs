using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_IntListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Int Event;
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
        public void OnPreEventRaised(int Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(int Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(int Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<int> { }
    }
}
