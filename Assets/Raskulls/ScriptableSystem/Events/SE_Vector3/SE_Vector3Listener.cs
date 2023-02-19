using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_Vector3Listener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_Vector3 Event ;
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
        public void OnPreEventRaised(Vector3 Value)
        {
            PreResponse.Invoke(Value);
        }
        public void OnEventRaised(Vector3 Value)
        {
            Response.Invoke(Value);
        }
        public void OnPostEventRaised(Vector3 Value)
        {
            PostResponse.Invoke(Value);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<Vector3> { }
    }
}
