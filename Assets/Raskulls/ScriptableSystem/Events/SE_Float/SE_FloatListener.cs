using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_FloatListener : GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Float Event;
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
        public void OnPreEventRaised(float Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(float Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(float Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<float> { }
    }
}
